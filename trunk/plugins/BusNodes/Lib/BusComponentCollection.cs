using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VVVV.Lib
{

	#region BusComponentCollection<T>

	public class BusComponentCollection<T> : KeyedCollection<Guid, T>
		where T : class, IBusComponent
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override Guid GetKeyForItem(T item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			return item.Id;
		}

		public new bool Add(T item)
		{
			if(item == null || !Contains(item.Id))
				return false;
			Add(item);
			return true;
		}

		public T Pop()
		{
			if(this.Count > 0)
				return null;
			var item = this[0];
			return item == null || !Remove(item.Id) ? null : item;
		}

		public bool TryPop(out T item)
		{
			if(this.Count != 0)
			{
				item = this[0];
				RemoveAt(0);
				return true;
			}
			item = null;
			return false;
		}

		public void ApplyAndFlush(Action<T> action)
		{
			if(action == null)
			{
				Clear();
			}
			else
			{
				T item;
				while(TryPop(out item))
				{
					action(item);
				}
			}
		}

		#endregion

	}

	#endregion

}
