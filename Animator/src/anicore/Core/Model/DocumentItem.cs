using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common;

namespace Animator.Core.Model
{

	#region DocumentItem

	public abstract class DocumentItem : IDocumentItem, IInternalDocumentItem, INotifyPropertyChanged, ISuspendableNotify
	{

		#region Static / Constant

		#endregion

		#region Fields

		private Guid _Id;
		private IDocumentItem _Parent;
		private string _Name;
		private bool _NotifySuspended;

		#endregion

		#region Properties

		public Guid Id
		{
			get { return _Id; }
			protected set
			{
				if(value != _Id)
				{
					_Id = value;
					OnPropertyChanged("Id");
				}
			}
		}

		public string Name
		{
			get { return _Name; }
			set
			{
				if(value != _Name)
				{
					_Name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		protected bool NotifySuspended
		{
			get { return _NotifySuspended; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal IDisposable SuspendNotifyScope()
		{
			if(_NotifySuspended)
				return ActionScope.Null;
			_NotifySuspended = true;
			return new ActionScope(() => _NotifySuspended = false);
		}

		internal void SuspendNotify()
		{
			_NotifySuspended = true;
		}

		internal void ResumeNotify()
		{
			_NotifySuspended = false;
		}

		#endregion

		#region IDocumentItem Members

		public IDocumentItem Parent
		{
			get { return _Parent; }
			internal set
			{
				if(value != _Parent)
					OnParentChanging(value);
				_Parent = value;
			}
		}

		protected virtual void OnParentChanging(IDocumentItem parent)
		{
		}

		public IDocument Document
		{
			get
			{
				var doc = ModelUtil.GetDocument(this);
				Debug.Assert(doc != null);
				return doc;
			}
		}

		public virtual IEnumerable<IDocumentItem> Children
		{
			get { return Enumerable.Empty<IDocumentItem>(); }
		}

		public virtual IDocumentItem GetItem(Guid id)
		{
			if(id == this.Id)
				return this;
			return null;
		}

		#endregion

		#region IInternalDocumentItem Members

		void IInternalDocumentItem.SetParent(IDocumentItem parent)
		{
			this.Parent = parent;
		}

		#endregion

		#region INotifyPropertyChanged Members

		protected virtual void OnPropertyChanged(string name)
		{
			if(!_NotifySuspended)
			{
				var handler = this.PropertyChanged;
				if(handler != null)
					handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region ISuspendableNotify Members

		void ISuspendableNotify.SuspendNotify()
		{
			SuspendNotify();
		}

		void ISuspendableNotify.ResumeNotify()
		{
			ResumeNotify();
		}

		#endregion

		#region IXElementWritable Members

		public virtual XElement WriteXElement(XName name)
		{
			throw new NotSupportedException();
		}

		#endregion

	}

	#endregion

}
