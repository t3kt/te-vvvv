using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Animator.UI.Common
{

	#region DurationToWidthConverter

	public sealed class DurationToWidthConverter : MultiplyConverter
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

		#region IValueConverter Members

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is TimeSpan)
				value = ((TimeSpan)value).TotalSeconds;
			return base.Convert(value, targetType, parameter, culture);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is TimeSpan)
				value = ((TimeSpan)value).TotalSeconds;
			return base.ConvertBack(value, targetType, parameter, culture);
		}

		#endregion

	}

	#endregion

}
