using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Lib
{

	#region ISpread

	internal interface ISpread : IEnumerable
	{
		object this[int index] { get; set; }
		int SliceCount { get; set; }
	}

	#endregion

	#region IDiffSpread

	internal interface IDiffSpread : ISpread
	{
		bool IsChanged { get; }
	}

	#endregion

}
