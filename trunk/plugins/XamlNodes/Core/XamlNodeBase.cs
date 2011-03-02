using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using XamlNodes.Core.Pins;

namespace XamlNodes.Core
{

	#region XamlNodeBase

	public class XamlNodeBase : UserControl, IXamlNode
	{

		#region Static / Constant

		#endregion

		#region Fields

		private IXamlNodeHost _Host;
		private XamlNodePinCollection<XamlNodeConfigPin> _ConfigPins;
		private XamlNodePinCollection<XamlNodeInputPin> _InputPins;
		private XamlNodePinCollection<XamlNodeOutputPin> _OutputPins;

		#endregion

		#region Properties

		public XamlNodePinCollection<XamlNodeConfigPin> ConfigPins
		{
			get { return _ConfigPins ?? (_ConfigPins = new XamlNodePinCollection<XamlNodeConfigPin>()); }
			set
			{
				AssertNoHost();
				_ConfigPins = value;
			}
		}

		public XamlNodePinCollection<XamlNodeInputPin> InputPins
		{
			get { return _InputPins ?? (_InputPins = new XamlNodePinCollection<XamlNodeInputPin>()); }
			set
			{
				AssertNoHost();
				_InputPins = value;
			}
		}

		public XamlNodePinCollection<XamlNodeOutputPin> OutputPins
		{
			get { return _OutputPins ?? (_OutputPins = new XamlNodePinCollection<XamlNodeOutputPin>()); }
			set
			{
				AssertNoHost();
				_OutputPins = value;
			}
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void AssertNoHost()
		{
			if(_Host != null)
				throw new InvalidOperationException("Operation invalid once host is attached");
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(_ConfigPins != null)
					_ConfigPins.Dispose();
				if(_InputPins != null)
					_InputPins.Dispose();
				if(_OutputPins != null)
					_OutputPins.Dispose();
				_ConfigPins = null;
				_InputPins = null;
				_OutputPins = null;
			}
		}

		#endregion

		#region IXamlNode Members

		public void SetHost(IXamlNodeHost host)
		{
			_Host = host;
			this.ConfigPins.AttachHost(host);
			this.InputPins.AttachHost(host);
			this.OutputPins.AttachHost(host);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
