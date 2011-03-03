using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using VVVV.Hosting.Pins;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace XamlNodes.Core.Pins
{

	#region XamlNodeInputPin

	public sealed class XamlNodeInputPin : XamlNodePin
	{

		#region Static / Constant

		private static readonly DependencyPropertyKey ValuePropertyKey;
		public static readonly DependencyProperty ValueProperty;
		private static readonly RoutedEvent ValueChangedEvent;

		static XamlNodeInputPin()
		{
			ValuePropertyKey = DependencyProperty.RegisterReadOnly("ValueI", typeof(object), typeof(XamlNodeInputPin), new FrameworkPropertyMetadata(0, Value_Changed));
			ValueProperty = ValuePropertyKey.DependencyProperty;
			ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChangedI", RoutingStrategy.Tunnel, typeof(XamlNodePinValueChangedEventHandler), typeof(XamlNodeInputPin));
		}

		private static void Value_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		#endregion

		#region Fields

		private InputPinHolder _PinHolder;

		#endregion

		#region Properties

		public object Value
		{
			get { return GetValue(ValueProperty); }
			private set { SetValue(ValuePropertyKey, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal override PinAttribute CreatePinAttribute()
		{
			return InitPinAttribute(new InputAttribute(this.ActualPinName));
		}

		internal override void AttachHost(IXamlNodeHost host)
		{
			_PinHolder = InputPinHolder.Create(host.PluginHost, CreatePinAttribute(), this.ActualDataType);
			_PinHolder.Changed += this.PinHolder_Changed;
		}

		private void PinHolder_Changed(object sender, EventArgs e)
		{
			if(_PinHolder != null)
				this.Value = _PinHolder.GetValue();
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing && _PinHolder != null)
			{
				_PinHolder.Changed -= this.PinHolder_Changed;
				_PinHolder.Dispose();
				_PinHolder = null;
			}
		}

		#endregion

	}

	#endregion

	#region XamlNodeInputPin<T>

	internal class XamlNodeInputPin<T> : XamlNodePin<T>
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		internal DiffPin<T> DiffPin { get { return (DiffPin<T>)this.Pin; } }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal override PinAttribute CreatePinAttribute()
		{
			return InitPinAttribute(new InputAttribute(this.ActualPinName));
		}

		protected override Pin<T> CreatePin(IPluginHost host)
		{
			return PinFactory.CreateDiffPin<T>(host, CreatePinAttribute());
		}

		#endregion

	}

	#endregion

}
