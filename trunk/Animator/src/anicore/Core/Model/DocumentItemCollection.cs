using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region DocumentItemCollection<T>

	internal class DocumentItemCollection<T> : ObservableCollection<T>
		where T : class, IDocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly IDocumentItem _Parent;
		private readonly Dictionary<Guid, T> _Lookup;

		#endregion

		#region Properties

		public IDocumentItem Parent
		{
			get { return _Parent; }
		}

		//public T this[Guid id]
		//{
		//    get { return _Lookup[id]; }
		//}

		#endregion

		#region Constructors

		public DocumentItemCollection(IDocumentItem parent)
		{
			Require.ArgNotNull(parent, "parent");
			_Parent = parent;
			_Lookup = new Dictionary<Guid, T>();
		}

		#endregion

		#region Methods

		protected virtual void Attach(IInternalDocumentItem item)
		{
			if(item != null)
				item.SetParent(_Parent);
		}

		public bool Contains(Guid id)
		{
			return _Lookup.ContainsKey(id);
		}

		public bool TryGetItem(Guid id, out T value)
		{
			return _Lookup.TryGetValue(id, out value);
		}

		[CanBeNull]
		public T GetItem(Guid id)
		{
			T item;
			return TryGetItem(id, out item) ? item : null;
		}

		public bool Remove(Guid id)
		{
			T item;
			return TryGetItem(id, out item) && this.Remove(item);
		}

		public void AddRange([NotNull] IEnumerable<T> items)
		{
			Require.ArgNotNull(items, "items");
			foreach(var item in items)
				Add(item);
		}

		internal void ReplaceAll([NotNull] IEnumerable<T> items)
		{
			Require.ArgNotNull(items, "items");
			Clear();
			AddRange(items);
		}

		protected override void InsertItem(int index, T item)
		{
			Require.ArgNotNull(item, "item");
			_Lookup.Add(item.Id, item);
			Attach(item as IInternalDocumentItem);
			base.InsertItem(index, item);
		}

		protected override void ClearItems()
		{
			base.ClearItems();
			_Lookup.Clear();
		}

		protected override void RemoveItem(int index)
		{
			var item = base[index];
			base.RemoveItem(index);
			_Lookup.Remove(item.Id);
		}

		protected override void SetItem(int index, T item)
		{
			Require.ArgNotNull(item, "item");
			var oldId = this.Items[index].Id;
#warning not sure about this...
			if(oldId == item.Id)
			{
				_Lookup[item.Id] = item;
			}
			else
			{
				_Lookup.Remove(oldId);
				_Lookup.Add(item.Id, item);
			}
			Attach(item as IInternalDocumentItem);
			base.SetItem(index, item);
		}

		#endregion

	}

	#endregion

}
