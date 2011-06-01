using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.IO;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region Output

	[Output(ElementName = "output", Key = "output", Description = "Generic Output")]
	public class Output : DocumentItem, IEquatable<Output>
	{

		#region TraceOutput

		[Output(ElementName = "traceoutput", Key = "trace", Description = "Debug Trace Output")]
		internal sealed class TraceOutput : Output
		{

			#region Static/Constant

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

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ObservableCollection<TargetObject> _Targets;

		#endregion

		#region Properties

		public ObservableCollection<TargetObject> Targets
		{
			get { return this._Targets; }
		}

		#endregion

		#region Events

		public event EventHandler<OutputMessageEventArgs> MessageDropped;

		protected virtual void OnMessageDropped(OutputMessage message)
		{
			if(message == null)
				return;
			var handler = this.MessageDropped;
			if(handler != null)
				handler(this, new OutputMessageEventArgs(message));
		}

		#endregion

		#region Constructors

		public Output()
			: this(Guid.NewGuid()) { }

		public Output(Guid id)
			: base(id)
		{
			this._Targets = new ObservableCollection<TargetObject>();
			this._Targets.CollectionChanged += this.Targets_CollectionChanged;
		}

		#endregion

		#region Methods

		protected virtual bool PostMessageInternal(OutputMessage message)
		{
			return false;
		}

		public virtual void Initialize(Output outputModel)
		{
			Require.ArgNotNull(outputModel, "outputModel");
		}

		public void PostMessage(OutputMessage message)
		{
			if(message != null)
			{
				if(!this.PostMessageInternal(message))
					this.OnMessageDropped(message);
			}
		}

		private void Targets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Targets");
		}

		public TargetObject GetTargetObject(Guid id)
		{
			return this._Targets.FindById(id);
		}

		public virtual void ReadXElement([NotNull] XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.ReadCommonXAttributes(element);
			this._Targets.AddRange(element.Elements(Schema.target).Select(e => new TargetObject(e)));
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.output,
				this.WriteCommonXAttributes(),
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

		public virtual bool Equals(Output other)
		{
			if(!base.Equals(other))
				return false;
			return this._Targets.ItemsEqual(other._Targets);
		}

		#endregion

	}

	#endregion

}
