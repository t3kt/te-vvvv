using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace Animator.UI.Common
{

	#region MultiplyConverter

	public class MultiplyConverter : IValueConverter
	{

		#region Static / Constant

		private static bool TryGetDouble(object value, out double d)
		{
			if(value is double)
				d = (double)value;
			else if(value is float)
				d = (float)value;
			else if(value is int)
				d = (int)value;
			else if(value is uint)
				d = (uint)value;
			else
			{
				d = 0;
				return false;
			}
			return true;
		}

		#endregion

		#region Fields

		private double _DefaultMultiplier = 1;

		#endregion

		#region Properties

		public double DefaultMultiplier
		{
			get { return this._DefaultMultiplier; }
			set { this._DefaultMultiplier = value; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IValueConverter Members

		public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			double v;
			if(TryGetDouble(value, out v))
			{
				double p;
				if(TryGetDouble(parameter, out p))
					return v * p;
				return v * this._DefaultMultiplier;
			}
			return value;
		}

		public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			double v;
			if(TryGetDouble(value, out v))
			{
				double p;
				if(TryGetDouble(parameter, out p))
					return v / p;
				return v / this._DefaultMultiplier;
			}
			return value;
		}

		#endregion

	}

	#endregion

}
