using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Animator.Core.Composition;
using Animator.Core.Model;

namespace Animator.Core.IO
{

	#region TraceOutput

	[Output(Key = Export_Key, ElementName = Export_ElementName, Description = Export_Description)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	internal sealed class TraceOutput : Output
	{

		#region Static/Constant

		internal new const string Export_Key = "trace";
		internal new const string Export_ElementName = "traceoutput";
		internal new const string Export_Description = "Debug Trace Output";

		private const string DefaultCategory = "OUTPUT";
		private const string DefaultPrefix = "[msg] ";

		#endregion

		#region Fields

		private string _Category;
		private string _Prefix;

		#endregion

		#region Properties

		public string Category
		{
			get { return this._Category; }
			set
			{
				if(value != this._Category)
				{
					this._Category = value;
					this.OnPropertyChanged("Category");
				}
			}
		}

		public string Prefix
		{
			get { return this._Prefix; }
			set
			{
				if(value != this._Prefix)
				{
					this._Prefix = value;
					this.OnPropertyChanged("Prefix");
				}
			}
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override void ReadXElement(XElement element)
		{
			base.ReadXElement(element);
			this._Category = (string)element.Attribute(Schema.traceoutput_category) ?? DefaultCategory;
			this._Prefix = (string)element.Attribute(Schema.traceoutput_prefix) ?? DefaultPrefix;
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.traceoutput)
				.WithContent(
					ModelUtil.WriteOptionalAttribute(Schema.traceoutput_category, this._Category),
					ModelUtil.WriteOptionalAttribute(Schema.traceoutput_prefix, this._Prefix));
		}

		protected override bool PostMessageInternal(OutputMessage message)
		{
			var str = OutputMessage.FormatTrace(message);
			if(!String.IsNullOrWhiteSpace(this._Prefix))
				str = this._Prefix + " " + str;
			Trace.WriteLine(str, this._Category);
			return true;
		}

		#endregion

	}

	#endregion

}
