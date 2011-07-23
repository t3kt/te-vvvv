using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.Model.Clips
{

	#region ConstData

	[ClipPropertyData(
		Key = Export_Key,
		ElementName = Export_ElementName,
		Description = Export_Description)]
	[ClipPropertyDataEditor("Animator.UI.Clips.ConstDataEditor, " + TEShared.AssemblyRef.ani)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public sealed class ConstData : ClipPropertyData
	{
		#region Static / Constant

		internal const string Export_Key = "const";
		internal const string Export_ElementName = "constprop";
		internal const string Export_Description = "Constant Value Data";

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
				if(!CommonUtil.AreValuesClose(value, this._Value))
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
