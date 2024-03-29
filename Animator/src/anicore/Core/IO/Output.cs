using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Model;
using TESharedAnnotations;

namespace Animator.Core.IO
{

	#region Output

	[AniExport(typeof(Output), Key = Export_Key, ElementName = Export_ElementName, Description = Export_Description)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class Output : DocumentItem, IEquatable<Output>
	{

		#region TargetCollection

		private sealed class TargetCollection : DocumentNodeCollection<TargetObject>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly Output _Output;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public TargetCollection(Output output)
				: base(output)
			{
				this._Output = output;
			}

			#endregion

			#region Methods

			protected override void Attach(TargetObject item)
			{
				Require.DBG_ArgNotNull(item, "item");
				item.PropertyValueChanged += this._Output.Target_PropertyValueChanged;
			}

			protected override void Detach(TargetObject item)
			{
				Require.DBG_ArgNotNull(item, "item");
				item.PropertyValueChanged += this._Output.Target_PropertyValueChanged;
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		internal const string Export_Key = "output";
		internal const string Export_ElementName = "output";
		internal const string Export_Description = "Generic Output";

		#endregion

		#region Fields

		private readonly TargetCollection _Targets;

		#endregion

		#region Properties

		public ObservableCollection<TargetObject> Targets
		{
			get { return this._Targets; }
		}

		#endregion

		#region Events

		public event EventHandler<OutputMessageEventArgs> MessageDropped;

		protected void OnMessageDropped(OutputMessage message)
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
			this._Targets = new TargetCollection(this);
			this.ObserveChildCollection("Targets", this._Targets);
		}

		#endregion

		#region Methods

		private void Target_PropertyValueChanged(object sender, TargetPropertyValueChangedEventArgs e)
		{
			Debug.Assert(sender is TargetObject);
			this.HandleTargetChange((TargetObject)sender, e);
		}

		protected virtual void HandleTargetChange(TargetObject targetObject, TargetPropertyValueChangedEventArgs e)
		{
			var message = e.BuildOutputMessage();
			this.PostMessage(message);
		}

		protected virtual bool PostMessageInternal(OutputMessage message)
		{
			return false;
		}

		public void PostMessage(OutputMessage message)
		{
			if(message != null)
			{
				if(!this.PostMessageInternal(message))
					this.OnMessageDropped(message);
			}
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
