using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Animator.Common.Diagnostics;
using Animator.Common.Threading;
using TESharedAnnotations;

namespace Animator.Core.Transport
{

	#region MediaTransport

	public sealed class MediaTransport : ITransport, IDisposable
	{

		#region Static / Constant

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
			get { return this._State; }
			private set
			{
				var changed = false;
				using(this._Lock.UpgradeableReadScope())
				{
					if(value != this._State)
					{
						this._Lock.EnterWriteLock();
						this._State = value;
						changed = true;
					}
				}
				if(changed)
					this.OnStateChanged();
			}
		}

		public Time Position
		{
			get { return this._Position; }
			set
			{
				if(value != this._Position)
				{
					//Interlocked.CompareExchange()
					this._Position = value;
					this.OnPositionChanged();
				}
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
			var handler = this.Tick;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		private void OnStateChanged()
		{
			var handler = this.StateChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		private void OnPositionChanged()
		{
			var handler = this.PositionChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Constructors

		public MediaTransport([CanBeNull] ReaderWriterLockSlim lok = null)
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

		private void Timer_Tick(object sender, EventArgs e)
		{
			if(this._State == TransportState.Playing)
			{
				throw new NotImplementedException();
				//this.UpdatePosition();
				this.OnTick();
			}
		}

		private void UpdatePositionInUpgradeableLock()
		{
			var now = NativeMethods.timeGetTime();
			var dur = now - this._LastUpdate;
			if(dur > 0)
			{
				this._Lock.EnterWriteLock();
				this.Position = this._Position + Time.TicksToBeats(now, this._BeatsPerMinute);
				this._LastUpdate = now;
			}
		}

		public void Play()
		{
			using(this._Lock.UpgradeableReadScope())
			{
				switch (this._State)
				{
					case TransportState.Playing:
						return;
					case TransportState.Paused:
					case TransportState.Stopped:
						throw new NotImplementedException();
				}
			}
		}

		public void Stop()
		{
			using(this._Lock.UpgradeableReadScope())
			{
				switch(this._State)
				{
				case TransportState.Playing:
					throw new NotImplementedException();
				case TransportState.Paused:
					throw new NotImplementedException();
				case TransportState.Stopped:
					throw new NotImplementedException();
				}
			}
		}

		public void Pause()
		{
			using(this._Lock.UpgradeableReadScope())
			{
				switch (this._State)
				{
					case TransportState.Playing:
						throw new NotImplementedException();
					case TransportState.Paused:
						return;
					case TransportState.Stopped:
						throw new NotImplementedException();
				}
			}
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
