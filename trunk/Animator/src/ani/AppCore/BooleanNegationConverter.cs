using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Animator.AppCore
{

	#region BooleanNegationConverter

	public sealed class BooleanNegationConverter : IValueConverter
	{

		#region Static / Constant

		private static object Negate(object value)
		{
			if(value is bool)
				return !(bool)value;
			if(value is bool?)
			{
				var b = (bool?)value;
				return b != true;
			}
			return value;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Negate(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Negate(value);
		}

		#endregion
	}

	#endregion

}
