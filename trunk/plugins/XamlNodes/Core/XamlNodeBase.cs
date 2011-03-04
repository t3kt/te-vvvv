using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using VVVV.PluginInterfaces.V1;
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
		private NodePinCollection _Pins;

		#endregion

		#region Properties

		public NodePinCollection Pins
		{
			get { return _Pins ?? (_Pins = new NodePinCollection()); }
			set { _Pins = value; }
		}

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

		public virtual void Evaluate(int spreadMax)
		{
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(_ConfigPins != null)
				{
					foreach(var pin in _ConfigPins)
						pin.PropertyChanged -= this.Input_Changed;
					_ConfigPins.Dispose();
				}
				if(_InputPins != null)
				{
					foreach(var pin in _InputPins)
						pin.PropertyChanged -= this.Input_Changed;
					_InputPins.Dispose();
				}
				if(_OutputPins != null)
					_OutputPins.Dispose();
				_ConfigPins = null;
				_InputPins = null;
				_OutputPins = null;
				if(_Pins != null)
				{
					_Pins.Dispose();
					_Pins = null;
				}
			}
		}

		protected internal void Log(string message, params object[] args)
		{
			Log(TLogType.Debug, message, args);
		}

		protected internal void Log(TLogType logType, string message, params object[] args)
		{
			if(_Host != null)
			{
				_Host.PluginHost.Log(logType, String.Format(message, args));
			}
		}

		private void Input_Changed(object sender, EventArgs e)
		{
			//this.UpdateLayout();
			if(_Host != null)
				_Host.Refresh();
		}

		#endregion

		#region IXamlNode Members

		public void SetHost(IXamlNodeHost host)
		{
			_Host = host;
			Log("XNb: setting host");

			Log("XNb: {0} config pins {1} input pins {2} output pins", this.ConfigPins.Count, this.InputPins.Count, this.OutputPins.Count);
			this.ConfigPins.AttachHost(host);
			foreach(var pin in this.ConfigPins)
				pin.PropertyChanged += this.Input_Changed;
			this.InputPins.AttachHost(host);
			foreach(var pin in this.InputPins)
				pin.PropertyChanged += this.Input_Changed;
			this.OutputPins.AttachHost(host);

			Log("XNb: {0} pins ({1} configs {2} inputs {3} outputs)", this.Pins.Count,
				this.Pins.Count(p => p.PinTypeInternal == PinType.Config),
				this.Pins.Count(p => p.PinTypeInternal == PinType.Input),
				this.Pins.Count(p => p.PinTypeInternal == PinType.Output));
			this.Pins.AttachHost(host);
			Log("XNb: {0} pins have IO, {1} pins do not have IO",
				this.Pins.Count(p => p.HasIO),
				this.Pins.Count(p => !p.HasIO));
			Log("XNb: pins: ");
			foreach(var pin in this.Pins)
				Log("* {0}", pin.DebugDump());
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
