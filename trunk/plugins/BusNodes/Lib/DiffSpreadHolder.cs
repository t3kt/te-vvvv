using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region DiffSpreadHolder<T>

	public sealed class DiffSpreadHolder<T> : IDiffSpread<T>, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private IDiffSpread<T> _Spread;

		#endregion

		#region Properties

		#endregion

		#region Events

		public event SpreadChangedEventHander<T> SpreadChanged;

		#endregion

		#region Constructors

		public DiffSpreadHolder() { }

		public DiffSpreadHolder(IDiffSpread<T> spread)
		{
			SetSpread(spread);
		}

		#endregion

		#region Methods

		public void SetSpread(IDiffSpread<T> spread)
		{
			if(_Spread != spread)
			{
				if(_Spread != null)
					_Spread.Changed -= this.Spread_Changed;
				_Spread = spread;
				spread.Changed += this.Spread_Changed;
				NotifySpreadChanged();
			}
			var handler = this.SpreadChanged;
			if(handler != null)
				handler(spread);
		}

		public void NotifySpreadChanged()
		{
			var handler = this.Changed;
			if(handler != null)
				handler(this);
		}

		private void Spread_Changed(IDiffSpread<T> spread)
		{
			//Debug.Assert(spread == _Spread);
			//Debug.Assert(spread != null);
			if(spread != _Spread || spread != null)
			{
				var handler = this.Changed;
				if(handler != null)
					handler(_Spread);
			}
		}

		#endregion

		#region IDiffSpread<T> Members

		public event SpreadChangedEventHander<T> Changed;

		public bool IsChanged
		{
			get { return _Spread != null && _Spread.IsChanged; }
		}

		event SpreadChangedEventHander IDiffSpread.Changed
		{
			add { throw new NotImplementedException(); }
			remove { throw new NotImplementedException(); }
		}

		#endregion

		#region ISpread<T> Members

		object ISpread.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (T)value; }
		}

		public int SliceCount
		{
			get { return _Spread == null ? 0 : _Spread.SliceCount; }
			set
			{
				if(_Spread != null)
					_Spread.SliceCount = value;
			}
		}

		public T this[int index]
		{
			get
			{
				if(_Spread == null)
					throw new InvalidOperationException();
				return _Spread[index];
			}
			set
			{
				if(_Spread == null)
					throw new InvalidOperationException();
				_Spread[index] = value;
			}
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return _Spread == null ? Enumerable.Empty<T>().GetEnumerator() : _Spread.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(_Spread != null)
				_Spread.Changed -= this.Spread_Changed;
			_Spread = null;
		}

		#endregion
	}

	#endregion

}
