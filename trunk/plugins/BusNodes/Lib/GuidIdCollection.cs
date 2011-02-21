using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VVVV.Lib
{

	#region GuidIdCollection<T>

	public class GuidIdCollection<T> : KeyedCollection<Guid, T>
		where T : IGuidId
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
			//if(item == null)
			//    throw new ArgumentNullException("item");
			return item.Id;
		}

		public new bool Add(T item)
		{
			if(item == null || !Contains(item.Id))
				return false;
			Add(item);
			return true;
		}

		#endregion

	}

	#endregion

}
