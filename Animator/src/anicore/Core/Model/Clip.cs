using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region Clip

	public class Clip : DocumentItem, IClip
	{

		#region Static / Constant

		#endregion

		#region Fields

		private string _ClipType;
		private Dictionary<string, string> _Parameters;
		private Time _Duration;
		private int _TriggerAlignment;

		#endregion

		#region Properties

		public string ClipType
		{
			get { return _ClipType; }
			internal set
			{
				if(value != _ClipType)
				{
					_ClipType = value;
					OnClipTypeChanged();
				}
			}
		}

		public Dictionary<string, string> Parameters
		{
			get { return _Parameters ?? (_Parameters = new Dictionary<string, string>()); }
			protected set { _Parameters = value; }
		}

		public Time Duration
		{
			get { return _Duration; }
			set
			{
				if(value != _Duration)
				{
					_Duration = value;
					OnPropertyChanged("Duration");
				}
			}
		}

		public int TriggerAlignment
		{
			get { return _TriggerAlignment; }
			set
			{
				if(value != _TriggerAlignment)
				{
					_TriggerAlignment = value;
					OnPropertyChanged("TriggerAlignment");
				}
			}
		}

		#endregion

		#region Constructors

		public Clip(IDocumentItem parent, Guid id)
		{
			this.Parent = parent;
			this.Id = id;
		}

		public Clip(IDocumentItem parent, XElement element)
		{
			this.Parent = parent;
			ReadXElement(element);
		}

		#endregion

		#region Methods

		protected virtual void OnClipTypeChanged()
		{
			OnPropertyChanged("ClipType");
		}

		protected void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			SuspendNotify();
			try
			{
				this.Id = (Guid)element.Attribute(Schema.clip_id);
				this.Name = (string)element.Attribute(Schema.clip_name);
				this.Duration = (float)element.Attribute(Schema.clip_dur);
				this.TriggerAlignment = (int?)element.Attribute(Schema.clip_align) ?? Model.Document.NoAlignment;
				this.ClipType = (string)element.Attribute(Schema.clip_type);
				_Parameters = ModelUtil.ReadParametersXElement(element.Element(Schema.clip_params));
				//.. other misc attributes?
			}
			finally
			{
				ResumeNotify();
			}
		}

		public override XElement WriteXElement(XName name)
		{
			//return new XElement(name??Schema.clip,
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
