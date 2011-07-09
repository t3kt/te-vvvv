using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Animator.UI.Common
{

	#region TypeEqualsConverter

	public sealed class TypeEqualsConverter : IValueConverter
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Type DefaultType { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value == null)
				return false;
			var type = parameter == null ? this.DefaultType : (Type)parameter;
			return value.GetType() == type;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return null;
		}

		#endregion

	}

	#endregion

}
