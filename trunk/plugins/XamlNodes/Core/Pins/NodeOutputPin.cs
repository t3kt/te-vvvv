using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using VVVV.Hosting.Pins;
using VVVV.PluginInterfaces.V2;

namespace XamlNodes.Core.Pins
{

	#region NodeOutputPin<T>

	public class NodeOutputPin<T> : NodePin
	{

		#region Static / Constant

		public static readonly DependencyProperty ValueProperty;
		public static readonly DependencyProperty ValuesProperty;

		static NodeOutputPin()
		{
			ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(NodeOutputPin<T>), new UIPropertyMetadata(default(T), Value_Changed));
			ValuesProperty = DependencyProperty.Register("Values", typeof(IList<T>), typeof(NodeOutputPin<T>), new UIPropertyMetadata(null, Value_Changed));
		}

		private static void Value_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var p = d as NodeOutputPin<T>;
			if(p != null)
				p.UpdatePin();
		}

		#endregion

		#region Fields

		private Pin<T> _Pin;

		#endregion

		#region Properties

		internal override PinType PinTypeInternal
		{
			get { return PinType.Output; }
		}

		internal override bool HasIO
		{
			get { return _Pin != null; }
		}

		public T Value
		{
			get { return (T)this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}

		public IList<T> Values
		{
			get { return (IList<T>)this.GetValue(ValuesProperty); }
			set { this.SetValue(ValuesProperty, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal override string DebugDump()
		{
			var sb = new StringBuilder("[OUT] ");
			sb.AppendFormat("PinName: '{0}' ", this.ActualPinName);
			sb.AppendFormat("HasIO: {0} ", this.HasIO);
			if(this.HasIO)
			{
				sb.AppendFormat("InnerType: {0} ", _Pin.GetType());
				sb.AppendFormat("InnerIOType: {0} ", _Pin.PluginIO.GetType());
			}
			return sb.ToString();
		}

		private void UpdatePin()
		{
			if(_Pin != null)
			{
				var values = this.Values;
				if(values == null)
				{
					_Pin.SliceCount = 1;
					_Pin[0] = this.Value;
				}
				else
				{
					_Pin.AssignFrom(values);
				}
			}
		}

		internal override void AttachHost(IXamlNodeHost host)
		{
			_Pin = PinFactory.CreatePin<T>(host.PluginHost, InitPinAttribute(new OutputAttribute(this.ActualPinName)));
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing && _Pin != null)
			{
				_Pin.Dispose();
				_Pin = null;
			}
		}

		#endregion

	}

	#endregion

	#region StringNodeOutputPin

	public sealed class StringNodeOutputPin : NodeOutputPin<string> { }

	#endregion

	#region ValueNodeOutputPin

	public sealed class ValueNodeOutputPin : NodeOutputPin<double> { }

	#endregion

}
