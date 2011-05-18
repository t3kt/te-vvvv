using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;

namespace Animator.Core.Transport
{

	#region ManualTransport

	[Transport(Key = "manual", Description = "Manual Transport")]
	public sealed class ManualTransport : Transport
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Time _Position;
		private TransportState _State;

		#endregion

		#region Properties

		public override Time Position
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

		public override TransportState State
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

		#region Constructors

		#endregion

		#region Methods

		public void SendTick()
		{
			this.OnTick();
		}

		#endregion

	}

	#endregion

}
