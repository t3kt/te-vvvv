using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Model
{

	#region TargetPropertyCollection

	public sealed class TargetPropertyCollection : ObservableCollection<TargetProperty>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Dictionary<string, TargetProperty> _PropsById = new Dictionary<string, TargetProperty>(TargetObject.PropertyNameComparer);

		#endregion

		#region Properties

		public TargetProperty this[string name]
		{
			get
			{
				TargetProperty prop;
				return this._PropsById.TryGetValue(name, out prop) ? prop : null;
			}
		}

		#endregion

		#region Constructors

		public TargetPropertyCollection() { }

		public TargetPropertyCollection([NotNull] IEnumerable<TargetProperty> collection)
			: base(collection) { }

		#endregion

		#region Methods

		public bool TryGetProperty(string name, out TargetProperty property)
		{
			return this._PropsById.TryGetValue(name, out property);
		}

		public bool ContainsKey(string name)
		{
			return this._PropsById.ContainsKey(name);
		}

		protected override void InsertItem(int index, TargetProperty item)
		{
			this.AddToLookup(item);
			base.InsertItem(index, item);
		}

		protected override void RemoveItem(int index)
		{
			var item = this[index];
			if(item != null)
				this._PropsById.Remove(item.Name);
			base.RemoveItem(index);
		}

		protected override void SetItem(int index, TargetProperty item)
		{
			Require.ArgNotNull(item, "item");
			this.AddToLookup(item);
			base.SetItem(index, item);
		}

		private void AddToLookup(TargetProperty item)
		{
			if(item == null)
				return;
			if(!this._PropsById.ContainsKey(item.Name))
				this._PropsById.Add(item.Name, item);
		}

		#endregion

	}

	#endregion

}
