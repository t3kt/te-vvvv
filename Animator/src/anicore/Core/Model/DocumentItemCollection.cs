using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region DocumentItemCollection<T>

	public class DocumentItemCollection<T> : ObservableCollection<T>, IDisposable, ISuspendableNotify
		where T : DocumentItem
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly IDocumentItem _Parent;
		private readonly Dictionary<Guid, T> _Lookup;
		private bool _NotifySuspended;

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

		internal void ReplaceAll([CanBeNull] IEnumerable<T> items)
		{
			Clear();
			if(items != null)
				AddRange(items);
		}

		protected override void InsertItem(int index, T item)
		{
			Require.ArgNotNull(item, "item");
			_Lookup.Add(item.Id, item);
			item.Parent = _Parent;
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
			//#warning not sure about this...
			if(oldId == item.Id)
			{
				_Lookup[item.Id] = item;
			}
			else
			{
				_Lookup.Remove(oldId);
				_Lookup.Add(item.Id, item);
			}
			item.Parent = _Parent;
			base.SetItem(index, item);
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if(!_NotifySuspended)
				base.OnCollectionChanged(e);
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if(!_NotifySuspended)
				base.OnPropertyChanged(e);
		}

		internal bool ItemsEqual(DocumentItemCollection<T> other)
		{
			if(other == null || other.Count == 0)
				return this.Count == 0;
			if(other.Count != this.Count)
				return false;
			foreach(var entry in this._Lookup)
			{
				T otherValue;
				if(!other._Lookup.TryGetValue(entry.Key, out otherValue) || !entry.Value.Equals(otherValue))
					return false;
			}
			return true;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.SuspendNotify();
			foreach(var item in this)
			{
				if(item != null)
					item.Dispose();
			}
			Clear();
			GC.SuppressFinalize(this);
		}

		#endregion

		#region ISuspendableNotify Members

		public void SuspendNotify()
		{
			_NotifySuspended = true;
		}

		public void ResumeNotify()
		{
			_NotifySuspended = false;
		}

		#endregion

	}

	#endregion

}
