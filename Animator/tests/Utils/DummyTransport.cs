using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.Transport;

namespace Animator.Tests.Utils
{

	#region DummyTransport

	[AniExport(typeof(Transport), Key = "dummy")]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	internal sealed class DummyTransport : Transport
	{

		public bool IsPlaying { get; set; }

		private TimeSpan _Position;

		public override TimeSpan Position
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

		public void FireTickEvent(TimeSpan interval)
		{
			this.OnTick(interval);
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
