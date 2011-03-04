using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace XamlNodes.Core.Pins
{

	#region XamlNodePinCollection<TPin>

	public sealed class XamlNodePinCollection<TPin> : Collection<TPin>, IDisposable
		where TPin : XamlNodePin
	{

		#region Static / Constant

		#endregion

		#region Fields

		private bool _IsFrozen;

		#endregion

		#region Properties

		internal bool IsFrozen
		{
			get { return _IsFrozen; }
		}

		#endregion

		#region Constructors

		public XamlNodePinCollection()
		{
		}

		public XamlNodePinCollection(IList<TPin> pins)
			: base(pins)
		{
		}

		#endregion

		#region Methods

		private void AssertNotFrozen()
		{
			if(_IsFrozen)
				throw new InvalidOperationException("Collection is frozen");
		}

		private void AssertFrozen()
		{
			if(!_IsFrozen)
				throw new InvalidOperationException("Collection is not frozen");
		}

		internal void AttachHost(IXamlNodeHost host)
		{
			AssertNotFrozen();
			this.ToDictionary(p => p.ActualPinName);
			foreach(var pin in this)
				pin.AttachHost(host);
			_IsFrozen = true;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(_IsFrozen)
			{
				//AssertFrozen();
				foreach(var pin in this)
					pin.Dispose();
				Clear();
				_IsFrozen = false;
			}
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
