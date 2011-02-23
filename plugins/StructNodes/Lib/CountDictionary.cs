using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Lib
{

	#region CountChangedEventArgs<TKey>

	internal sealed class CountChangedEventArgs<TKey> : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly TKey _Key;
		private readonly int _Count;

		#endregion

		#region Properties

		public TKey Key
		{
			get { return _Key; }
		}

		public int Count
		{
			get { return _Count; }
		}

		#endregion

		#region Constructors

		public CountChangedEventArgs(TKey key, int count)
		{
			_Key = key;
			_Count = count;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region CountDictionary<TKey>

	internal sealed class CountDictionary<TKey>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Dictionary<TKey, int> _Dictionary;

		#endregion

		#region Properties

		#endregion

		#region Events

		public event EventHandler<CountChangedEventArgs<TKey>> CountChanged;

		private void OnCountChanged(TKey key, int count)
		{
			var handler = this.CountChanged;
			if(handler != null)
				handler(this, new CountChangedEventArgs<TKey>(key, count));
		}

		#endregion

		#region Constructors

		public CountDictionary(IEqualityComparer<TKey> comparer)
		{
			_Dictionary = new Dictionary<TKey, int>(comparer);
		}

		public CountDictionary()
		{
			_Dictionary = new Dictionary<TKey, int>();
		}

		#endregion

		#region Methods

		public bool HasCount(TKey key)
		{
			return _Dictionary.ContainsKey(key);
		}

		public int GetCount(TKey key)
		{
			int count;
			return _Dictionary.TryGetValue(key, out count) ? count : 0;
		}

		public bool IncrementCount(TKey key)
		{
			int count;
			if(_Dictionary.TryGetValue(key, out count))
			{
				_Dictionary[key] = count + 1;
				OnCountChanged(key, count + 1);
				return false;
			}
			_Dictionary.Add(key, 1);
			OnCountChanged(key, 1);
			return true;
		}

		public bool DecrementCount(TKey key)
		{
			int count;
			if(_Dictionary.TryGetValue(key, out count))
			{
				if(count <= 1)
				{
					_Dictionary.Remove(key);
					OnCountChanged(key, 0);
					return true;
				}
				_Dictionary[key] = count - 1;
				OnCountChanged(key, count - 1);
				return false;
			}
			return false;
		}

		#endregion

	}

	#endregion

}
