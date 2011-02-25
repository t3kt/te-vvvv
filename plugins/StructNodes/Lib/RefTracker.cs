using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VVVV.Lib
{

	#region RefTracker<T>

	internal sealed class RefTracker<T>
		where T : class
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly T _Item;
		private int _RefCount;

		#endregion

		#region Properties

		public int RefCount
		{
			get { return this._RefCount; }
		}

		#endregion

		#region Events

		public event EventHandler Emptied;

		#endregion

		#region Constructors

		public RefTracker(T item)
		{
			this._Item = item;
		}

		#endregion

		#region Methods

		public T RequestRef()
		{
			_RefCount++;
			return _Item;
		}

		public bool ReleaseRef()
		{
			_RefCount--;
			if(_RefCount > 0)
				return false;
			var handler = this.Emptied;
			if(handler != null)
				handler(this, EventArgs.Empty);
			return true;
		}

		#endregion

	}

	#endregion

}
