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

	#region OutputPinHolder

	internal abstract class OutputPinHolder : IEnumerable, IDisposable
	{

		#region Nested Types

		#region Holder<T>

		private sealed class Holder<T> : OutputPinHolder
		{

			#region Static / Constant

			#endregion

			#region Fields

			private readonly Pin<T> _Pin;

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
				set { _Pin.SliceCount = value; }
			}

			#endregion

			#region Constructors

			public Holder(IPluginHost host, Attribute attribute)
			{
				_Pin = PinFactory.CreatePin<T>(host, attribute);
			}

			#endregion

			#region Methods

			private void SetMulti(IEnumerable<T> values)
			{
				var list = values as IList<T> ?? values.ToList();
				_Pin.AssignFrom(list);
			}

			private void SetSingle(T value)
			{
				_Pin.SliceCount = 1;
				_Pin[0] = value;
			}

			public override void SetValue(object value)
			{
				if(value is IEnumerable<T>)
					SetMulti((IEnumerable<T>)value);
				else if(value is T)
					SetSingle((T)value);
				else
					throw new NotSupportedException("Value type not supported");
			}

			public override IEnumerator GetEnumerator()
			{
				return _Pin.GetEnumerator();
			}

			protected override void Dispose(bool disposing)
			{
				if(disposing)
					_Pin.Dispose();
			}

			#endregion

		}

		#endregion

		#endregion

		#region Static / Constant

		internal static OutputPinHolder Create<T>(IPluginHost host, Attribute attribute)
		{
			return new Holder<T>(host, attribute);
		}

		internal static OutputPinHolder Create(IPluginHost host, Attribute attribute, Type type)
		{
			var holderType = typeof(Holder<>).MakeGenericType(type);
			return (OutputPinHolder)Activator.CreateInstance(holderType, host, attribute);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		internal abstract object Pin { get; }

		public abstract IPluginIO InternalIO { get; }

		public abstract int SliceCount { get; set; }

		#endregion

		#region Constructors

		private OutputPinHolder()
		{
		}

		#endregion

		#region Methods

		public abstract void SetValue(object value);

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region IEnumerable Members

		public abstract IEnumerator GetEnumerator();

		#endregion

	}

	#endregion

}
