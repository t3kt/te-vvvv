using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.Utils.VColor;

namespace XamlNodes.Core.Pins
{

	#region IPinWrapper

	internal interface IPinWrapper : IPluginIO
	{
		int SliceCount { get; set; }
	}

	#endregion

	#region IInputPinWrapper<T>

	internal interface IInputPinWrapper<T> : IPinWrapper
	{
		T GetValue(int index);
	}

	#endregion

	#region IOutputPinWrapper<T>

	internal interface IOutputPinWrapper<T> : IPinWrapper
	{
		void SetValue(int index, T value);
	}

	#endregion

	#region PinWrapper

	internal abstract class PinWrapper : IPinWrapper
	{

		#region Nested Types

		#region ValueInWrapper

		private sealed class ValueInWrapper : PinWrapper, IInputPinWrapper<double>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IValueIn _Input;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Input.SliceCount; }
				set { throw new NotSupportedException(); }
			}

			#endregion

			#region Constructors

			public ValueInWrapper(IValueIn pin)
				: base(pin)
			{
				_Input = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IInputPinWrapper<double> Members

			public double GetValue(int index)
			{
				double value;
				_Input.GetValue(index, out value);
				return value;
			}

			#endregion

		}

		#endregion

		#region ValueConfigWrapper

		private sealed class ValueConfigWrapper : PinWrapper, IInputPinWrapper<double>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IValueConfig _Config;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Config.SliceCount; }
				set { _Config.SliceCount = value; }
			}

			#endregion

			#region Constructors

			public ValueConfigWrapper(IValueConfig pin)
				: base(pin)
			{
				_Config = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IInputPinWrapper<double> Members

			public double GetValue(int index)
			{
				double value;
				_Config.GetValue(index, out value);
				return value;
			}

			#endregion

		}

		#endregion

		#region ValueOutWrapper

		private sealed class ValueOutWrapper : PinWrapper, IOutputPinWrapper<double>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IValueOut _Output;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Output.SliceCount; }
				set { throw new NotSupportedException(); }
			}

			#endregion

			#region Constructors

			public ValueOutWrapper(IValueOut pin)
				: base(pin)
			{
				_Output = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IOutputPinWrapper<double> Members

			public void SetValue(int index, double value)
			{
				_Output.SetValue(index, value);
			}

			#endregion

		}

		#endregion

		#region ColorInWrapper

		private sealed class ColorInWrapper : PinWrapper, IInputPinWrapper<RGBAColor>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IColorIn _Input;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Input.SliceCount; }
				set { throw new NotSupportedException(); }
			}

			#endregion

			#region Constructors

			public ColorInWrapper(IColorIn pin)
				: base(pin)
			{
				_Input = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IInputPinWrapper<RGBAColor> Members

			public RGBAColor GetValue(int index)
			{
				RGBAColor value;
				_Input.GetColor(index, out value);
				return value;
			}

			#endregion

		}

		#endregion

		#region ColorConfigWrapper

		private sealed class ColorConfigWrapper : PinWrapper, IInputPinWrapper<RGBAColor>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IColorConfig _Config;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Config.SliceCount; }
				set { _Config.SliceCount = value; }
			}

			#endregion

			#region Constructors

			public ColorConfigWrapper(IColorConfig pin)
				: base(pin)
			{
				_Config = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IInputPinWrapper<RGBAColor> Members

			public RGBAColor GetValue(int index)
			{
				RGBAColor value;
				_Config.GetColor(index, out value);
				return value;
			}

			#endregion

		}

		#endregion

		#region ColorOutWrapper

		private sealed class ColorOutWrapper : PinWrapper, IOutputPinWrapper<RGBAColor>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IColorOut _Output;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Output.SliceCount; }
				set { throw new NotSupportedException(); }
			}

			#endregion

			#region Constructors

			public ColorOutWrapper(IColorOut pin)
				: base(pin)
			{
				_Output = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IOutputPinWrapper<RGBAColor> Members

			public void SetValue(int index, RGBAColor value)
			{
				_Output.SetColor(index, value);
			}

			#endregion

		}

		#endregion

		#region StringInWrapper

		private sealed class StringInWrapper : PinWrapper, IInputPinWrapper<string>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IStringIn _Input;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Input.SliceCount; }
				set { throw new NotSupportedException(); }
			}

			#endregion

			#region Constructors

			public StringInWrapper(IStringIn pin)
				: base(pin)
			{
				_Input = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IInputPinWrapper<string> Members

			public string GetValue(int index)
			{
				string value;
				_Input.GetString(index, out value);
				return value;
			}

			#endregion

		}

		#endregion

		#region StringConfigWrapper

		private sealed class StringConfigWrapper : PinWrapper, IInputPinWrapper<string>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IStringConfig _Config;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Config.SliceCount; }
				set { _Config.SliceCount = value; }
			}

			#endregion

			#region Constructors

			public StringConfigWrapper(IStringConfig pin)
				: base(pin)
			{
				_Config = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IInputPinWrapper<string> Members

			public string GetValue(int index)
			{
				string value;
				_Config.GetString(index, out value);
				return value;
			}

			#endregion

		}

		#endregion

		#region StringOutWrapper

		private sealed class StringOutWrapper : PinWrapper, IOutputPinWrapper<string>
		{

			#region Static/Constant

			#endregion

			#region Fields

			private readonly IStringOut _Output;

			#endregion

			#region Properties

			public override int SliceCount
			{
				get { return _Output.SliceCount; }
				set { throw new NotSupportedException(); }
			}

			#endregion

			#region Constructors

			public StringOutWrapper(IStringOut pin)
				: base(pin)
			{
				_Output = pin;
			}

			#endregion

			#region Methods

			#endregion

			#region IOutputPinWrapper<string> Members

			public void SetValue(int index, string value)
			{
				_Output.SetString(index, value);
			}

			#endregion

		}

		#endregion

		#endregion

		#region Static/Constant

		internal static IPinWrapper WrapInputPin(IPluginIO pin)
		{
			Debug.Assert(pin != null);
			if(pin is IPinWrapper)
				return (IPinWrapper)pin;
			if(pin is IValueIn)
				return new ValueInWrapper((IValueIn)pin);
			if(pin is IValueConfig)
				return new ValueConfigWrapper((IValueConfig)pin);
			if(pin is IColorIn)
				return new ColorInWrapper((IColorIn)pin);
			if(pin is IColorConfig)
				return new ColorConfigWrapper((IColorConfig)pin);
			if(pin is IStringIn)
				return new StringInWrapper((IStringIn)pin);
			if(pin is IStringConfig)
				return new StringConfigWrapper((IStringConfig)pin);
			throw new ArgumentException("pin");
		}

		internal static IInputPinWrapper<T> WrapInputPin<T>(IPluginIO pin)
		{
			var wrapper = WrapInputPin(pin);
			Debug.Assert(wrapper is IInputPinWrapper<T>);
			return (IInputPinWrapper<T>)wrapper;
		}

		internal static IPinWrapper WrapOutputPin(IPluginIO pin)
		{
			Debug.Assert(pin != null);
			if(pin is IPinWrapper)
				return (IPinWrapper)pin;
			if(pin is IValueOut)
				return new ValueOutWrapper((IValueOut)pin);
			if(pin is IColorOut)
				return new ColorOutWrapper((IColorOut)pin);
			if(pin is IStringOut)
				return new StringOutWrapper((IStringOut)pin);
			throw new ArgumentException("pin");
		}

		internal static IOutputPinWrapper<T> WrapOutputPin<T>(IPluginIO pin)
		{
			var wrapper = WrapOutputPin(pin);
			Debug.Assert(wrapper is IOutputPinWrapper<T>);
			return (IOutputPinWrapper<T>)wrapper;
		}

		#endregion

		#region Fields

		protected readonly IPluginIO _Pin;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		private PinWrapper(IPluginIO pin)
		{
			if(pin == null)
				throw new ArgumentNullException("pin");
			_Pin = pin;
		}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			return _Pin == obj;
		}

		public override int GetHashCode()
		{
			return _Pin.GetHashCode();
		}

		#endregion

		#region IPinWrapper Members

		public abstract int SliceCount { get; set; }

		#endregion

		#region IPluginIO Members

		public bool IsConnected
		{
			get { return _Pin.IsConnected; }
		}

		public string Name
		{
			get { return _Pin.Name; }
			set { _Pin.Name = value; }
		}

		public int Order
		{
			get { return _Pin.Order; }
			set { _Pin.Order = value; }
		}

		public void SetPinUpdater(IPinUpdater pinUpdater)
		{
			_Pin.SetPinUpdater(pinUpdater);
		}

		#endregion

	}

	#endregion

}
