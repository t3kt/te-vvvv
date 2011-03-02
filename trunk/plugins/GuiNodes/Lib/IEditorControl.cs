using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Lib
{

	#region IEditorControl

	interface IEditorControl
	{
		object Value { get; set; }
		event EventHandler ValueChanged;
	}

	#endregion

	#region IEditorControl<T>

	interface IEditorControl<T> : IEditorControl
	{
		new T Value { get; set; }
	}

	#endregion

}
