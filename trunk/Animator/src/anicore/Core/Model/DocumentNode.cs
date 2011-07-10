using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region DocumentNode

	public abstract class DocumentNode : INotifyPropertyChanged
	{

		#region Static / Constant

		protected static void DisposeIfNeeded(object obj)
		{
			var d = obj as IDisposable;
			if(d != null)
				d.Dispose();
		}

		#endregion

		#region Fields

		private DocumentNode _Parent;

		#endregion

		#region Properties

		[CanBeNull]
		[Browsable(false)]
		public DocumentNode Parent
		{
			get { return this._Parent; }
			internal set { this._Parent = value; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected void ObserveChildCollection([NotNull] string name, [NotNull] INotifyCollectionChanged children)
		{
			Require.DBG_ArgNotNull(children, "children");
			children.CollectionChanged += (s, e) => this.OnPropertyChanged(name);
		}

		public virtual bool TryDeleteChild(DocumentNode node)
		{
			return false;
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string name)
		{
			var e = new PropertyChangedEventArgs(name);
			this.NotifyPropertyChanged(this, e);
			this.BubblePropertyChanged(this, e);
		}

		private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(sender, e);
		}

		protected virtual void BubblePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if(this._Parent != null)
				this._Parent.NotifyPropertyChanged(sender, e);
		}

		protected virtual void ClearPropertyChangedListeners()
		{
			this.PropertyChanged = null;
		}

		#endregion

	}

	#endregion

}
