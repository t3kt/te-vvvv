using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VVVV.Lib
{

	#region ValueUpDown

	internal class ValueUpDown : NumericUpDown
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ValueUpDown()
		{
			this.DecimalPlaces = 4;
			this.Maximum = 1000;
			this.Minimum = -1000;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
