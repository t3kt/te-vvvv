using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Lib
{

	#region SpreadWrapper<T>

	internal class SpreadWrapper<T> : ISpread<T>, ISpread
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ISpread<T> _Spread;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public SpreadWrapper(ISpread<T> spread)
		{
			_Spread = spread;
		}

		#endregion

		#region Methods

		#endregion

		#region ISpread<T> Members

		public int SliceCount
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public T this[int index]
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region ISpread Members

		object ISpread.this[int index]
		{
			get { return this[index]; }
			set
			{
				Debug.Assert(value is T);
				this[index] = (T)value;
			}
		}

		#endregion

	}

	#endregion

}
