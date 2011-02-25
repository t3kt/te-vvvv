using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Lib
{

	#region StructTypeEventArgs

	internal class StructTypeEventArgs : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly StructTypeDefinition _TypeDefinition;

		#endregion

		#region Properties

		public StructTypeDefinition TypeDefinition
		{
			get { return _TypeDefinition; }
		}

		#endregion

		#region Constructors

		public StructTypeEventArgs(StructTypeDefinition typeDefinition)
		{
			_TypeDefinition = typeDefinition;
		}

		#endregion

		#region Methods

		#endregion
	}

	#endregion

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

}
