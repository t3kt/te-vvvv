using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OscSeq.Core.Model
{

	#region IKeyedCollection<TKey, TValue>

	public interface IKeyedCollection<in TKey, TValue> : IList<TValue>
	{
		bool ContainsKey(TKey key);
		TValue this[TKey key] { get; }
	}

	#endregion

}
