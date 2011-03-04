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

	#region NodeInputPin<T>

	public class NodeInputPin<T> : NodePin
	{

		#region Static / Constant

		private static readonly DependencyPropertyKey ValuePropertyKey;
		public static readonly DependencyProperty ValueProperty;
		private static readonly DependencyPropertyKey ValuesPropertyKey;
		public static readonly DependencyProperty ValuesProperty;

		static NodeInputPin()
		{
			ValuePropertyKey = DependencyProperty.RegisterReadOnly("Value", typeof(T), typeof(NodeInputPin<T>), new UIPropertyMetadata(default(T)));
			ValueProperty = ValuePropertyKey.DependencyProperty;
			ValuesPropertyKey = DependencyProperty.RegisterReadOnly("Values", typeof(IList<T>), typeof(NodeInputPin<T>), new UIPropertyMetadata(default(T)));
			ValuesProperty = ValuesPropertyKey.DependencyProperty;
		}

		#endregion

		#region Fields

		private DiffPin<T> _Pin;
		private PinType _PinType = PinType.Input;

		#endregion

		#region Properties

		internal override PinType PinTypeInternal
		{
			get { return _PinType; }
		}

		internal override bool HasIO
		{
			get { return _Pin != null; }
		}

		public PinType PinType
		{
			get { return _PinType; }
			set
			{
				if(value == PinType.Output)
					throw new NotSupportedException();
				_PinType = value;
			}
		}

		public T Value
		{
			get { return (T)GetValue(ValueProperty); }
			private set { SetValue(ValuePropertyKey, value); }
		}

		public IList<T> Values
		{
			get { return (IList<T>)GetValue(ValuesProperty); }
			private set { SetValue(ValuesPropertyKey, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal override string DebugDump()
		{
			var sb = new StringBuilder();
			if(_PinType == PinType.Input)
				sb.Append("[IN] ");
			else if(_PinType == PinType.Config)
				sb.Append("[CFG] ");
			else
				sb.Append("[??] ");
			sb.AppendFormat("PinName: '{0}' ", this.ActualPinName);
			sb.AppendFormat("HasIO: {0} ", this.HasIO);
			if(this.HasIO)
			{
				sb.AppendFormat("InnerType: {0} ", _Pin.GetType());
				sb.AppendFormat("InnerIOType: {0} ", _Pin.PluginIO.GetType());
			}
			return sb.ToString();
		}

		private PinAttribute CreatePinAttribute()
		{
			switch(_PinType)
			{
			case PinType.Input:
				return new InputAttribute(this.ActualPinName);
			case PinType.Config:
				return new ConfigAttribute(this.ActualPinName);
			default:
				throw new NotSupportedException();
			}
		}

		internal override void AttachHost(IXamlNodeHost host)
		{
			_Pin = PinFactory.CreateDiffPin<T>(host.PluginHost, InitPinAttribute(this.CreatePinAttribute()));
			_Pin.Changed += this.Pin_Changed;
		}

		private void UpdateValues()
		{
			if(_Pin != null)
			{
				this.Value = _Pin.FirstOrDefault();
				this.Values = _Pin.ToList();
			}
		}

		private void Pin_Changed(IDiffSpread<T> spread)
		{
			UpdateValues();
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing && _Pin != null)
			{
				_Pin.Changed -= this.Pin_Changed;
			}
		}

		#endregion

	}

	#endregion

	#region StringNodeInputPin

	public sealed class StringNodeInputPin : NodeInputPin<string> { }

	#endregion

	#region ValueNodeInputPin

	public sealed class ValueNodeInputPin : NodeInputPin<string> { }

	#endregion

}
