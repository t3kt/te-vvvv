using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
		private ObservableCollection<TargetObject> _Targets;

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

		public ObservableCollection<TargetObject> Targets
		{
			get
			{
				if(this._Targets == null)
				{
					this._Targets = new ObservableCollection<TargetObject>();
					this.AttachTargets(this._Targets);
				}
				return this._Targets;
			}
			set
			{
				if(value != this._Targets)
				{
					this.DetachTargets(this._Targets);
					this._Targets = value;
					this.AttachTargets(this._Targets);
					this.OnPropertyChanged("Targets");
				}
			}
		}

		#endregion

		#region Constructors

		public Output()
			: this(Guid.NewGuid()) { }

		public Output(Guid id)
			: base(id) { }

		public Output(XElement element)
			: base(element)
		{
			ReadXElement(element);
		}

		#endregion

		#region Methods

		private void AttachTargets(ObservableCollection<TargetObject> targets)
		{
			if(targets != null)
				targets.CollectionChanged += this.Targets_CollectionChanged;
		}

		private void DetachTargets(ObservableCollection<TargetObject> targets)
		{
			if(targets != null)
				targets.CollectionChanged -= this.Targets_CollectionChanged;
		}

		private void Targets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Targets");
		}

		internal string GetParameter(string key)
		{
			if(this._Parameters == null)
				return null;
			string value;
			return this._Parameters.TryGetValue(key, out value) ? value : null;
		}

		public TargetObject GetTargetObject(Guid id)
		{
			return this._Targets.FindById(id);
		}

		private void ReadXElement(XElement element)
		{
			this.OutputType = (string)element.Attribute(Schema.output_type);
			this.Parameters = ModelUtil.ReadParametersXElement(element.Element(Schema.output_params));
			this.Targets = new ObservableCollection<TargetObject>(element.Elements(Schema.target).Select(e => new TargetObject(e)));
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.output,
				this.WriteCommonXAttributes(),
				ModelUtil.WriteOptionalAttribute(Schema.output_type, this.OutputType),
				ModelUtil.WriteParametersXElement(Schema.output_params, this._Parameters),
				ModelUtil.WriteXElements(this._Targets));
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
				   ModelUtil.ParametersEqual(other._Parameters, this._Parameters) &&
				   this._Targets.ItemsEqual(other._Targets);
		}

		#endregion

	}

	#endregion

}
