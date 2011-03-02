using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VVVV.Lib
{

	#region StringEditorTextBox

	public class StringEditorTextBox : TextBox, IEditorControl<string>
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

		#endregion

		#region IEditorControl<string> Members

		string IEditorControl<string>.Value
		{
			get { return this.Text; }
			set { this.Text = value; }
		}

		#endregion

		#region IEditorControl Members

		object IEditorControl.Value
		{
			get { return this.Text; }
			set
			{
				Debug.Assert(value is string);
				this.Text = (string)value;
			}
		}

		public event EventHandler ValueChanged
		{
			add { this.TextChanged += value; }
			remove { this.TextChanged -= value; }
		}

		#endregion

	}

	#endregion

}
