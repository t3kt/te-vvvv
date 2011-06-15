using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Animator.Common.Diagnostics;
using Animator.Common.Threading;
using Animator.Core.Composition;
using TESharedAnnotations;

namespace Animator.Core.Transport
{

	#region MediaTransport

	[Transport(Key = "media", Description = "Media Transport")]
	public sealed class MediaTransport : Transport
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

		[DllImport(TEShared.AssemblyRef.winmm_dll)]
		private static extern uint timeGetTime();

		#endregion

		#region Fields

		private readonly Sanford.Multimedia.Timers.Timer _Timer;
		private TransportState _State;
		private uint _LastUpdate;
		private TimeSpan _Position;
		private float _BeatsPerMinute = Model.Document.DefaultBeatsPerMinute;
		private readonly ReaderWriterLockSlim _Lock;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Common)]
		public override float BeatsPerMinute
		{
			get
			{
				using(this._Lock.ReadScope())
					return this._BeatsPerMinute;
			}
			internal set
			{
				Require.ArgPositive(value, "value");
				using(this._Lock.WriteScope())
					this._BeatsPerMinute = value;
			}
		}

		[Category(TEShared.Names.Category_Common)]
		public override TransportState State
		{
			get
			{
				using(this._Lock.ReadScope())
					return this._State;
			}
			protected set
			{
				using(this._Lock.WriteScope())
					this._State = value;
			}
		}

		[Category(TEShared.Names.Category_Transport)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override TimeSpan Position
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

		[Category(TEShared.Names.Category_Transport)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int Period
		{
			get { return this._Timer.Period; }
			set { this._Timer.Period = value; }
		}

		[Category(TEShared.Names.Category_Transport)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int Resolution
		{
			get { return this._Timer.Resolution; }
			set { this._Timer.Resolution = value; }
		}

		#endregion

		#region Events

		protected override void OnTick(TimeSpan interval)
		{
			var handler = this.TickHandler;
			if(handler != null)
			{
				var sync = this._Timer.SynchronizingObject;
				var e = new TransportTickEventArgs(interval);
				if(sync != null)
					sync.BeginInvoke(handler, new object[] { this, e });
				else
					handler(this, e);
			}
		}

		protected override void OnStateChanged()
		{
			this.FireEvent(this.StateChangedHandler);
		}

		protected override void OnPositionChanged()
		{
			this.FireEvent(this.PositionChangedHandler);
		}

		private void FireEvent(EventHandler handler)
		{
			if(handler == null)
				return;
			var sync = this._Timer.SynchronizingObject;
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

		#endregion

		#region Methods

		private void Timer_Tick(object sender, EventArgs e)
		{
			var posChanged = false;
			var shouldTick = false;
			uint dur = 0;
			using(var scope = this._Lock.UpgradeableReadScope())
			{
				if(this._State == TransportState.Playing)
				{
					var now = timeGetTime();
					dur = now - this._LastUpdate;
					if(dur > 0)
					{
						scope.UpgradeToWriteLock();
						this._Position += TimeSpan.FromTicks(now);
						posChanged = true;
					}
					shouldTick = true;
				}
			}
			if(posChanged)
				this.OnPositionChanged();
			if(shouldTick)
				this.OnTick(TimeSpan.FromTicks(dur));
		}

		public override void Play()
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
					this._LastUpdate = timeGetTime();
					this._Timer.Start();
					break;
				}
			}
			this.OnStateChanged();
		}

		public override void Stop()
		{
			using(this._Lock.WriteScope())
			{
				switch(this._State)
				{
				case TransportState.Playing:
					this._Position = TimeSpan.Zero;
					this._State = TransportState.Stopped;
					this._Timer.Stop();
					break;
				//case TransportState.Paused:
				//    throw new NotImplementedException();
				case TransportState.Stopped:
					this._Position = TimeSpan.Zero;
					return;
				}
			}
			this.OnPositionChanged();
			this.OnStateChanged();
		}

		public override void Pause()
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

		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				this.Stop();
				this._Lock.Dispose();
			}
			this._Timer.Dispose();
		}

		#endregion

	}

	#endregion

}
