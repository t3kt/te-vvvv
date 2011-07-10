using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Animator.Core.Model
{

	#region DocumentNodeCollection<T>

	internal class DocumentNodeCollection<T> : ObservableCollection<T>
		where T : DocumentNode
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly DocumentNode _Parent;

		#endregion

		#region Properties

		protected DocumentNode Parent
		{
			get { return this._Parent; }
		}

		#endregion

		#region Constructors

		public DocumentNodeCollection(DocumentNode parent)
		{
			this._Parent = parent;
		}

		#endregion

		#region Methods

		protected virtual void Attach(T item)
		{
			if(item != null)
				item.Parent = this._Parent;
		}

		protected virtual void Detach(T item)
		{
			if(item != null)
				item.Parent = null;
		}

		protected override void InsertItem(int index, T item)
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

		protected override void SetItem(int index, T item)
		{
			var oldItem = this[index];
			this.Detach(oldItem);
			this.Attach(item);
			base.SetItem(index, item);
		}

		#endregion

	}

	#endregion

}
