using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using VVVV.PluginInterfaces.V2;

namespace XamlNodes.Core.Pins
{

	#region XamlNodeConfigPin

	public sealed class XamlNodeConfigPin : XamlNodePin
	{

		#region Static / Constant

		private static readonly DependencyPropertyKey ValuePropertyKey;
		public static readonly DependencyProperty ValueProperty;

		static XamlNodeConfigPin()
		{
			ValuePropertyKey = DependencyProperty.RegisterReadOnly("ValueC", typeof(object), typeof(XamlNodeConfigPin), new FrameworkPropertyMetadata(0));
			ValueProperty = ValuePropertyKey.DependencyProperty;
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
			return InitPinAttribute(new ConfigAttribute(this.ActualPinName));
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

}
