using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Model
{

	#region Output

	public sealed class Output : DocumentItem, IEquatable<Output>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private string _OutputType;
		private Dictionary<string, string> _Parameters;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Common)]
		public string OutputType
		{
			get { return _OutputType; }
			set
			{
				if(value != _OutputType)
				{
					_OutputType = value;
					OnPropertyChanged("OutputType");
				}
			}
		}

		[Category(TEShared.Names.Category_Output)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IDictionary<string, string> Parameters
		{
			get { return this._Parameters ?? (this._Parameters = new Dictionary<string, string>()); }
			set
			{
				if(value != this._Parameters)
				{
					this._Parameters = value == null ? null : value.ToDictionary(x => x.Key, x => x.Value);
					this.OnPropertyChanged("Parameters");
				}
			}
		}

		#endregion

		#region Constructors

		public Output()
			: this(Guid.NewGuid()) { }

		public Output(Guid id)
		{
			this.Id = id;
		}

		public Output(XElement element)
		{
			ReadXElement(element);
		}

		#endregion

		#region Methods

		internal string GetParameter(string key)
		{
			if(this._Parameters == null)
				return null;
			string value;
			return this._Parameters.TryGetValue(key, out value) ? value : null;
		}

		private void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.Id = (Guid)element.Attribute(Schema.output_id);
			this.Name = (string)element.Attribute(Schema.output_name);
			this.OutputType = (string)element.Attribute(Schema.output_type);
			this.Parameters = ModelUtil.ReadParametersXElement(element.Element(Schema.output_params));
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.output,
				new XAttribute(Schema.output_id, this.Id),
				ModelUtil.WriteOptionalAttribute(Schema.output_name, this.Name),
				ModelUtil.WriteOptionalAttribute(Schema.output_type, this.OutputType),
				ModelUtil.WriteParametersXElement(Schema.output_params, this._Parameters));
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Output);
		}

		#endregion

		#region IEquatable<Output> Members

		public bool Equals(Output other)
		{
			if(!base.Equals(other))
				return false;
			return other._OutputType == this._OutputType &&
				   ModelUtil.ParametersEqual(other._Parameters, this._Parameters);
		}

		#endregion

	}

	#endregion

}
