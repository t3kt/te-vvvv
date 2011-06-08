using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Animator.Core.Composition;
using TESharedAnnotations;

namespace Animator.Core.Model.Clips
{

	#region ConstData

	[ClipPropertyData(Key = "const", Description = "Constant Value Data", ElementName = "constprop")]
	public sealed class ConstData : ClipPropertyData
	{

		#region Static / Constant

		#endregion

		#region Fields

		private float _Value;

		#endregion

		#region Properties

		public float Value
		{
			get { return this._Value; }
			set
			{
				if(value != this._Value)
				{
					this._Value = value;
					this.OnPropertyChanged("Value");
				}
			}
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override void ReadXElement([NotNull] XElement element)
		{
			base.ReadXElement(element);
			this._Value = (float)element.Attribute(Schema.constprop_value);
		}

		public override object GetValue(float position)
		{
			return this._Value;
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.constprop,
				this.WriteCommonXAttributes(),
				new XAttribute(Schema.constprop_value, this._Value));
		}

		#endregion

	}

	#endregion

}
