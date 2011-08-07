using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.Transport;

namespace Animator.Tests.Utils
{

	#region ManualTransport

	[AniExport(typeof(ITransportController), Key = "manual", Description = "Manual Transport")]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public sealed class ManualTransport : TransportBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		private TimeSpan _Position;
		private TransportState _State;

		#endregion

		#region Properties

		public override TimeSpan Position
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

		public void SendTick(TimeSpan interval)
		{
			this.OnTick(interval);
		}

		#endregion

	}

	#endregion

}
