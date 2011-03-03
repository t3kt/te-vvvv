using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VVVV.Hosting.Pins;
using VVVV.PluginInterfaces.V1;
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
			return InitPinAttribute(new OutputAttribute(this.ActualPinName));
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

	#region XamlNodeOutputPin<T>

	public sealed class XamlNodeOutputPin<T> : XamlNodePin<T>
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal override PinAttribute CreatePinAttribute()
		{
			return InitPinAttribute(new OutputAttribute(this.ActualPinName));
		}

		protected override Pin<T> CreatePin(IPluginHost host)
		{
			return PinFactory.CreatePin<T>(host, CreatePinAttribute());
		}

		#endregion

	}

	#endregion

}
