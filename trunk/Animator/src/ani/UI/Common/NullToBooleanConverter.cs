using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Animator.UI.Common
{

	#region NullToBooleanConverter

	public sealed class NullToBooleanConverter : IValueConverter
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public bool NullIsTrue { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			//if(((parameter is bool) && (bool)parameter) || this.NullIsTrue)
			if(this.NullIsTrue)
				return value == null;
			return value != null;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion

	}

	#endregion

}
