using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Lib
{

	#region ISpreadEditorControl

	internal interface ISpreadEditorControl
	{
		int SliceCount { get; }
		object this[int index] { get; set; }
		event EventHandler ValueChanged;
	}

	#endregion

	#region ISpreadEditorControl<T>

	internal interface ISpreadEditorControl<T> : ISpreadEditorControl
	{
		new T this[int index] { get; set; }
	}

	#endregion

}
