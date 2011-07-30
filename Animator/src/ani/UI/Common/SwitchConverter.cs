using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Animator.UI.Common
{

	#region SwitchCaseBase

	[ContentProperty("Value")]
	public abstract class SwitchCaseBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public object Value { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal abstract bool TestMatch(object input);

		#endregion

	}

	#endregion

	#region SwitchCase

	public class SwitchCase : SwitchCaseBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public object Test { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal override bool TestMatch(object input)
		{
			return Object.Equals(input, this.Test);
		}

		#endregion

	}

	#endregion

	#region TypeCase

	public sealed class TypeCase : SwitchCaseBase
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Type Type { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal override bool TestMatch(object input)
		{
			return this.Type != null && this.Type.IsInstanceOfType(input);
		}

		#endregion

	}

	#endregion

	#region SwitchConverter

	[ContentProperty("Cases")]
	public class SwitchConverter : IValueConverter
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly List<SwitchCaseBase> _Cases = new List<SwitchCaseBase>();

		#endregion

		#region Properties

		public List<SwitchCaseBase> Cases
		{
			get { return this._Cases; }
		}

		public object DefaultValue { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IValueConverter Members

		public object Convert(object input, Type targetType, object parameter, CultureInfo culture)
		{
			foreach(var c in this._Cases)
			{
				if(c.TestMatch(input))
					return c.Value;
			}
			return this.DefaultValue;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion

	}

	#endregion

	#region SwitchDataTemplateSelector

	[ContentProperty("Cases")]
	public class SwitchDataTemplateSelector : DataTemplateSelector
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly List<SwitchCaseBase> _Cases = new List<SwitchCaseBase>();

		#endregion

		#region Properties

		public List<SwitchCaseBase> Cases
		{
			get { return this._Cases; }
		}

		public object DefaultValue { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			foreach(var c in this._Cases)
			{
				if(c.TestMatch(item) && c.Value is DataTemplate)
					return (DataTemplate)c.Value;
			}
			return base.SelectTemplate(item, container);
		}

		#endregion

	}

	#endregion

}
