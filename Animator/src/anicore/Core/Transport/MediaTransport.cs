using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Animator.Common.Diagnostics;
using Animator.Common.Threading;
using Animator.Core.Composition;
using Animator.Core.Runtime;
using Animator.Core.Transport;
using TESharedAnnotations;

[assembly: RegisteredImplementation(typeof(ITransport), "media", typeof(MediaTransport))]

namespace Animator.Core.Transport
{

	#region MediaTransport

	[Transport(Key = "media")]
	public sealed class MediaTransport : IInternalTransport, IDisposable
	{

		#region Static / Constant

		private static int DefaultPeriod
		{
			get { return Sanford.Multimedia.Timers.Timer.Capabilities.periodMin; }
		}

		private static int DefaultResolution
		{
			get { return 1; }
		}

		#endregion

		#region Fields

		private readonly Sanford.Multimedia.Timers.Timer _Timer;
		private TransportState _State;
		private uint _LastUpdate;
		private Time _Position;
		private float _BeatsPerMinute = Model.Document.DefaultBeatsPerMinute;
		private readonly ReaderWriterLockSlim _Lock;

		#endregion

		#region Properties

		public float BeatsPerMinute
		{
			get
			{
				using(this._Lock.ReadScope())
					return this._BeatsPerMinute;
			}
			set
			{
				Require.ArgPositive(value, "value");
				using(this._Lock.WriteScope())
					this._BeatsPerMinute = value;
			}
		}

		public TransportState State
		{
			get
			{
				using(this._Lock.ReadScope())
					return this._State;
			}
		}

		public Time Position
		{
			get
			{
				using(this._Lock.ReadScope())
					return this._Position;
			}
			set
			{
				var posChanged = false;
				using(var scope = this._Lock.UpgradeableReadScope())
				{
					if(value != this._Position)
					{
						scope.UpgradeToWriteLock();
						this._Position = value;
						posChanged = true;
					}
				}
				if(posChanged)
					this.OnPositionChanged();
			}
		}

		public int Period
		{
			get { return this._Timer.Period; }
			set { this._Timer.Period = value; }
		}

		public int Resolution
		{
			get { return this._Timer.Resolution; }
			set { this._Timer.Resolution = value; }
		}

		public ISynchronizeInvoke SynchronizingObject
		{
			get { return this._Timer.SynchronizingObject; }
			set { this._Timer.SynchronizingObject = value; }
		}

		#endregion

		#region Events

		public event EventHandler Tick;

		public event EventHandler StateChanged;

		public event EventHandler PositionChanged;

		private void OnTick()
		{
			//var handler = this.Tick;
			//if(handler != null)
			//{
			//    var sync = this.SynchronizingObject;
			//    if(sync != null)
			//        sync.BeginInvoke(handler, new object[] { EventArgs.Empty });
			//    else
			//        handler(this, EventArgs.Empty);
			//}
			this.FireEvent(this.Tick);
		}

		private void OnStateChanged()
		{
			//var handler = this.StateChanged;
			//if(handler != null)
			//    handler(this, EventArgs.Empty);
			this.FireEvent(this.StateChanged);
		}

		private void OnPositionChanged()
		{
			//var handler = this.PositionChanged;
			//if(handler != null)
			//    handler(this, EventArgs.Empty);
			this.FireEvent(this.PositionChanged);
		}

		private void FireEvent(EventHandler handler)
		{
			if(handler == null)
				return;
			var sync = this.SynchronizingObject;
			if(sync != null)
				sync.BeginInvoke(handler, new object[] { this, EventArgs.Empty });
			else
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Constructors

		public MediaTransport()
			: this(null) { }

		internal MediaTransport([CanBeNull] ReaderWriterLockSlim lok)
		{
			this._Lock = lok ?? new ReaderWriterLockSlim();
			this._Timer = new Sanford.Multimedia.Timers.Timer();
			this._Timer.Tick += this.Timer_Tick;
		}

		~MediaTransport()
		{
			this.Dispose(false);
		}

		#endregion

		#region Methods

		public void SetParameters(IDictionary<string, string> parameters)
		{
			if(parameters != null)
			{
				string str;
				int i;
				if(parameters.TryGetValue("Period", out str))
				{
					if(String.IsNullOrEmpty(str))
						this.Period = DefaultPeriod;
					else if(Int32.TryParse(str, out i))
						this.Period = i;
				}
				if(parameters.TryGetValue("Resolution", out str))
				{
					if(String.IsNullOrEmpty(str))
						this.Period = DefaultResolution;
					else if(Int32.TryParse(str, out i))
						this.Resolution = i;
				}
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			var posChanged = false;
			var shouldTick = false;
			using(var scope = this._Lock.UpgradeableReadScope())
			{
				if(this._State == TransportState.Playing)
				{
					var now = NativeMethods.timeGetTime();
					var dur = now - this._LastUpdate;
					if(dur > 0)
					{
						scope.UpgradeToWriteLock();
						this._Position += Time.TicksToBeats(now, this._BeatsPerMinute);
						posChanged = true;
					}
					shouldTick = true;
				}
			}
			if(posChanged)
				this.OnPositionChanged();
			if(shouldTick)
				this.OnTick();
		}

		public void Play()
		{
			using(var scope = this._Lock.UpgradeableReadScope())
			{
				switch(this._State)
				{
				case TransportState.Playing:
					return;
				//case TransportState.Paused:
				case TransportState.Stopped:
					scope.UpgradeToWriteLock();
					this._State = TransportState.Playing;
					this._LastUpdate = NativeMethods.timeGetTime();
					this._Timer.Start();
					break;
				}
			}
			this.OnStateChanged();
		}

		public void Stop()
		{
			using(this._Lock.WriteScope())
			{
				switch(this._State)
				{
				case TransportState.Playing:
					this._Position = 0;
					this._State = TransportState.Stopped;
					this._Timer.Stop();
					break;
				//case TransportState.Paused:
				//    throw new NotImplementedException();
				case TransportState.Stopped:
					this._Position = 0;
					return;
				}
			}
			this.OnPositionChanged();
			this.OnStateChanged();
		}

		public void Pause()
		{
			using(this._Lock.UpgradeableReadScope())
			{
				switch(this._State)
				{
				case TransportState.Playing:
					this._State = TransportState.Stopped;
					this._Timer.Stop();
					break;
				//case TransportState.Paused:
				//    return;
				case TransportState.Stopped:
					return;
				}
			}
			this.OnStateChanged();
		}

		#endregion

		#region IDisposable Members

		private void Dispose(bool disposing)
		{
			if(disposing)
			{
				this.Stop();
				this._Lock.Dispose();
			}
			this._Timer.Dispose();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
