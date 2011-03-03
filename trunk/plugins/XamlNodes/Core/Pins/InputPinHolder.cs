using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Hosting.Pins;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace XamlNodes.Core.Pins
{

	#region InputPinHolder

	internal abstract class InputPinHolder : IDisposable
	{

		#region Nested Types

		#region Holder<T>

		private sealed class Holder<T> : InputPinHolder
		{

			#region Static / Constant

			#endregion

			#region Fields

			private readonly DiffPin<T> _Pin;

			#endregion

			#region Properties

			internal override object Pin
			{
				get { return _Pin; }
			}

			public override IPluginIO InternalIO
			{
				get { return _Pin.PluginIO; }
			}

			public override int SliceCount
			{
				get { return _Pin.SliceCount; }
			}

			public override bool IsChanged
			{
				get { return _Pin.IsChanged; }
			}

			#endregion

			#region Events

			public override event EventHandler Changed;

			#endregion

			#region Constructors

			public Holder(IPluginHost host, Attribute attribute)
			{
				_Pin = PinFactory.CreateDiffPin<T>(host, attribute);
				_Pin.Changed += this.Pin_Changed;
			}

			#endregion

			#region Methods

			public override void NotifyChanged()
			{
				var handler = this.Changed;
				if(handler != null)
					handler(this, EventArgs.Empty);
			}

			private void Pin_Changed(IDiffSpread<T> spread)
			{
				NotifyChanged();
			}

			public override object GetValue()
			{
				if(_Pin.SliceCount == 0)
					return default(T);
				if(_Pin.SliceCount == 1)
					return _Pin[0];
				return _Pin.ToList();
			}

			public override object GetValue(int sliceIndex)
			{
				return _Pin[sliceIndex];
			}

			protected override void Dispose(bool disposing)
			{
				if(disposing)
				{
					_Pin.Changed -= this.Pin_Changed;
					_Pin.Dispose();
				}
			}

			#endregion

		}

		#endregion

		#endregion

		#region Static / Constant

		internal static InputPinHolder Create<T>(IPluginHost host, Attribute attribute)
		{
			return new Holder<T>(host, attribute);
		}

		internal static InputPinHolder Create(IPluginHost host, Attribute attribute, Type type)
		{
			var holderType = typeof(Holder<>).MakeGenericType(type);
			return (InputPinHolder)Activator.CreateInstance(holderType, host, attribute);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		internal abstract object Pin { get; }

		public abstract IPluginIO InternalIO { get; }

		public abstract int SliceCount { get; }

		public abstract bool IsChanged { get; }

		#endregion

		#region Events

		public abstract event EventHandler Changed;

		#endregion

		#region Constructors

		private InputPinHolder()
		{
		}

		#endregion

		#region Methods

		public abstract void NotifyChanged();

		public abstract object GetValue();

		public abstract object GetValue(int sliceIndex);

		#endregion

		#region IDisposable Members

		protected abstract void Dispose(bool disposing);

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
