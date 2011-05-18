using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.Transport;

namespace Animator.Tests.Utils
{

	#region DummyTransport

	[Transport(Key = "dummy")]
	internal sealed class DummyTransport : Transport
	{

		public bool IsPlaying { get; set; }

		private Time _Position;

		public override Time Position
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
		public override TransportState State
		{
			get { return this._State; }
			protected set
			{
				if(value != this._State)
				{
					this._State = value;
					this.FireStateChangedEvent();
				}
			}
		}

		internal void SetState(TransportState state)
		{
			this.State = state;
		}

		public void FireTickEvent()
		{
			this.OnTick();
		}

		public void FireStateChangedEvent()
		{
			this.OnStateChanged();
		}

		public void FirePositionChangedEvent()
		{
			this.OnPositionChanged();
		}

	}

	#endregion

}
