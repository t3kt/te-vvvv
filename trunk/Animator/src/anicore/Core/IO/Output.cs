using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Model;
using Animator.Core.Runtime;
using TESharedAnnotations;

namespace Animator.Core.IO
{

	#region Output

	[Output(Key = Export_Key, ElementName = Export_ElementName, Description = Export_Description)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class Output : DocumentItem, IEquatable<Output>
	{

		#region TargetCollection

		private sealed class TargetCollection : ObservableCollection<TargetObject>
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
			{
				this._Output = output;
			}

			#endregion

			#region Methods

			private void Attach(TargetObject item)
			{
				Require.DBG_ArgNotNull(item, "item");
				item.PropertyValueChanged += this._Output.Target_PropertyValueChanged;
			}

			private void Detach(TargetObject item)
			{
				Require.DBG_ArgNotNull(item, "item");
				item.PropertyValueChanged += this._Output.Target_PropertyValueChanged;
			}

			protected override void InsertItem(int index, TargetObject item)
			{
				base.InsertItem(index, item);
				this.Attach(item);
			}

			protected override void ClearItems()
			{
				foreach(var item in this)
					this.Detach(item);
				base.ClearItems();
			}

			protected override void RemoveItem(int index)
			{
				var item = this[index];
				this.Detach(item);
				base.RemoveItem(index);
			}

			protected override void SetItem(int index, TargetObject item)
			{
				var oldItem = this[index];
				this.Detach(oldItem);
				this.Attach(item);
				base.SetItem(index, item);
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		private static readonly ItemTypeInfo _ItemType = new ItemTypeInfo(typeof(Output));

		internal const string Export_Key = "output";
		internal const string Export_ElementName = "output";
		internal const string Export_Description = "Generic Output";

		#endregion

		#region Fields

		private readonly TargetCollection _Targets;

		#endregion

		#region Properties

		public sealed override ItemTypeInfo ItemType
		{
			get { return _ItemType; }
		}

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
			this._Targets.CollectionChanged += this.Targets_CollectionChanged;
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

		private void Targets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Targets");
		}

		public TargetObject GetTargetObject(Guid id)
		{
			return this._Targets.FindById(id);
		}

		internal override bool TryDeleteItem(IDocumentItem item)
		{
			return item is TargetObject && this._Targets.Remove((TargetObject)item);
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
