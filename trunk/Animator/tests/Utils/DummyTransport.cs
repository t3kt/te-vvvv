using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.Runtime;
using Animator.Core.Transport;
using Animator.Tests.Utils;

[assembly: RegisteredImplementation(typeof(ITransport), "dummy", typeof(DummyTransport))]

namespace Animator.Tests.Utils
{

	#region DummyTransport

	[Transport(Key = "dummy")]
	internal sealed class DummyTransport : ITransport
	{

		public bool IsPlaying { get; set; }

		private Time _Position;

		public Time Position
		{
			get { return this._Position; }
			set
			{
				if(value != this._Position)
				{
					this._Position = value;
					this.FirePositionChangedEvent();
				}
			}
		}

		private TransportState _State;
		public TransportState State
		{
			get { return this._State; }
			set
			{
				if(value != this._State)
				{
					this._State = value;
					this.FireStateChangedEvent();
				}
			}
		}

		public event EventHandler Tick;

		public event EventHandler StateChanged;

		public event EventHandler PositionChanged;

		public void FireTickEvent()
		{
			var handler = this.Tick;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public void FireStateChangedEvent()
		{
			var handler = this.StateChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public void FirePositionChangedEvent()
		{
			var handler = this.PositionChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public void SetParameters(IDictionary<string, string> parameters)
		{
		}

		public void Play()
		{
			this.State = TransportState.Playing;
		}

		public void Stop()
		{
			this.State = TransportState.Stopped;
			this.Position = 0;
		}

		public void Pause()
		{
			//this.State = TransportState.Paused;
			this.State = TransportState.Stopped;
		}

	}

	#endregion

}
