using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Animator.UI.Common
{

	#region AdvancedBooleanToVisibilityConverter

	public sealed class AdvancedBooleanToVisibilityConverter : IValueConverter
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Visibility FalseVisibility { get; set; }

		public Visibility TrueVisibility { get; set; }

		#endregion

		#region Constructors

		public AdvancedBooleanToVisibilityConverter()
		{
			this.FalseVisibility = Visibility.Collapsed;
			this.TrueVisibility = Visibility.Visible;
		}

		#endregion

		#region Methods

		#endregion

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value != null && (bool)value)
				return this.TrueVisibility;
			return this.FalseVisibility;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value == null)
				return false;
			var vis = (Visibility)value;
			if(vis == this.FalseVisibility)
				return false;
			if(vis == this.TrueVisibility)
				return true;
			return value;
		}

		#endregion

	}

	#endregion

}
