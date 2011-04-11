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

	public abstract class DocumentItem : IDocumentItem, INotifyPropertyChanged, ISuspendableNotify,
		IEquatable<IDocumentItem>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private IDocumentItem _Parent;
		private string _Name;
		private bool _NotifySuspended;

		#endregion

		#region Properties

		public Guid Id { get; protected set; }

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

		//~DocumentItem()
		//{
		//    this.Dispose(false);
		//}

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

		public override bool Equals(object obj)
		{
			return Equals(obj as IDocumentItem);
		}

		public override int GetHashCode()
		{
			//unchecked
			//{
			//    return ((this._Name != null ? this._Name.GetHashCode() : 0) * 397) ^ this.Id.GetHashCode();
			//}
			return this.Id.GetHashCode();
		}

		#endregion

		#region IEquatable<DocumentItem> Members

		public bool Equals(IDocumentItem other)
		{
			return other != null &&
				   other.Id == this.Id &&
				   other.Name == this.Name;
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

		public Document Document
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

		public abstract XElement WriteXElement(XName name = null);

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				this.PropertyChanged = null;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
