using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VVVV.PluginInterfaces.V2;

namespace XamlNodes.Core.Pins
{

	#region XamlNodeOutputPin

	public sealed class XamlNodeOutputPin : XamlNodePin
	{

		#region Static / Constant

		public static readonly DependencyProperty ValueProperty;

		static XamlNodeOutputPin()
		{
			ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(XamlNodeOutputPin), new UIPropertyMetadata(0, ValueProperty_Changed));
		}

		private static void ValueProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var pin = d as XamlNodeOutputPin;
			if(pin != null && pin._PinHolder != null)
				pin._PinHolder.SetValue(e.NewValue);
		}

		#endregion

		#region Fields

		private OutputPinHolder _PinHolder;

		#endregion

		#region Properties

		public object Value
		{
			get { return GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal override PinAttribute CreatePinAttribute()
		{
			return new OutputAttribute(this.PinName);
		}

		internal override void AttachHost(IXamlNodeHost host)
		{
			_PinHolder = OutputPinHolder.Create(host.PluginHost, CreatePinAttribute(), this.ActualDataType);
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing && _PinHolder != null)
			{
				_PinHolder.Dispose();
				_PinHolder = null;
			}
		}

		#endregion

	}

	#endregion

}
