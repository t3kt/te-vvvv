using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Transport
{

	#region TransportBase

	public abstract class TransportBase : ITransport, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Time _Position;
		private TransportState _State;

		#endregion

		#region Properties

		public virtual Time Position
		{
			get { return this._Position; }
			set
			{
				if(value != this._Position)
				{
					this._Position = value;
					this.OnPositionChanged();
				}
			}
		}

		public virtual TransportState State
		{
			get { return this._State; }
			protected set
			{
				if(value != this._State)
				{
					this._State = value;
					this.OnStateChanged();
				}
			}
		}

		#endregion

		#region Events

		public event EventHandler Tick;

		public event EventHandler StateChanged;

		public event EventHandler PositionChanged;

		protected virtual void OnTick()
		{
			var handler = this.Tick;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		protected virtual void OnStateChanged()
		{
			var handler = this.StateChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		protected virtual void OnPositionChanged()
		{
			var handler = this.PositionChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Constructors

		protected TransportBase()
		{

		}

		~TransportBase()
		{
			this.Dispose(false);
		}

		#endregion

		#region Methods

		public virtual void Play()
		{
			this.State = TransportState.Playing;
		}

		public virtual void Stop()
		{
			this.State = TransportState.Stopped;
			this.Position = 0;
		}

		public virtual void Pause()
		{
			this.State = TransportState.Paused;
		}

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{

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
