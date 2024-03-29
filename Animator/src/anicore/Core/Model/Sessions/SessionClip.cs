using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Model.Clips;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Model.Sessions
{

	#region SessionClip

	public sealed class SessionClip : ClipBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		private int? _Row;
		private ClipState _State;
		private TimeSpan _Duration;

		#endregion

		#region Properties

		public int? Row
		{
			get { return this._Row; }
			set
			{
				if(value != this._Row)
				{
					this._Row = value;
					this.OnPropertyChanged("Row");
				}
			}
		}

		public ClipState State
		{
			get { return this._State; }
			set
			{
				if(value != this._State)
				{
					this._State = value;
					this.OnPropertyChanged("State");
				}
			}
		}

		[Category(TEShared.Names.Category_Transport)]
		public TimeSpan Duration
		{
			get { return this._Duration; }
			set
			{
				Require.ArgNotNegative(value, "value");
				if(value != this._Duration)
				{
					this._Duration = value;
					this.OnPropertyChanged("Duration");
				}
			}
		}

		#endregion

		#region Constructors

		public SessionClip(Guid id)
			: base(id) { }

		public SessionClip([NotNull] XElement element, [CanBeNull] AniHost host)
			: base(element, host)
		{
			this._Row = (int?)element.Attribute(Schema.sesclip_row);
			this._State = (ClipState)Enum.Parse(typeof(ClipState), (string)element.Attribute(Schema.sesclip_state));
			this._Duration = ModelUtil.ParseTimeSpan(element.Attribute(Schema.sesclip_dur));
		}

		#endregion

		#region Methods

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.sesclip,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteOptionalValueAttribute(Schema.sesclip_row, this._Row),
				new XAttribute(Schema.sesclip_state, this._State),
				new XAttribute(Schema.sesclip_dur, this._Duration),
				this.WritePropertiesXElement());
		}

		internal bool IsActive(ITransportController transport)
		{
			return this._State == ClipState.Playing;
		}

		internal double GetPosition(ITransportController transport)
		{
			throw new NotImplementedException();
		}

		internal override void PushTargetValues(TargetObject target, ITransportController transport)
		{
			Require.DBG_ArgNotNull(target, "target");
			Require.DBG_ArgNotNull(transport, "transport");
			if(!this.IsActive(transport))
				return;
			var pos = this.GetPosition(transport);
			foreach(var prop in this.Properties)
				target.SetValue(prop.Name, prop.GetValue(pos));
		}

		#endregion

	}

	#endregion

}
