using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;
using TESharedAnnotations;

namespace Animator.Osc
{

	#region OscOutput

	[Output(
		Key = Export_Key,
		ElementName = Export_ElementName,
		Description = Export_Description)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public sealed class OscOutput : Output
	{

		#region Schema

		private static class Schema
		{
			public static readonly XName oscoutput = Export_ElementName;
			public static readonly XName oscoutput_host = "host";
			public static readonly XName oscoutput_port = "port";
		}

		#endregion

		#region Static / Constant

		internal const string Export_Key = "osc";
		internal const string Export_ElementName = "oscoutput";
		internal const string Export_Description = "OSC Output";

		private static OscPacket BuildPacket(OutputMessage message)
		{
			Debug.Assert(message != null);
			var address = message.TargetKey as string;
			if(String.IsNullOrEmpty(address))
				return null;
			var data = message.Data;
			if(data == null)
				return null;
			var packet = new OscMessage(address);
			foreach(var item in data)
				packet.Append(item);
			return packet;
		}

		#endregion

		#region Fields

		private readonly OscConnection _Connection = new OscConnection();

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_IO)]
		public string Host
		{
			get { return this._Connection.Host; }
			set
			{
				Require.ArgNotNullOrEmpty(value, "value");
				if(value != this._Connection.Host)
				{
					this._Connection.Host = value;
					this.OnPropertyChanged("Host");
				}
			}
		}

		[Category(TEShared.Names.Category_IO)]
		public int Port
		{
			get { return this._Connection.Port; }
			set
			{
				Require.ArgNotNegative(value, "value");
				if(value != this._Connection.Port)
				{
					this._Connection.Port = value;
					this.OnPropertyChanged("Port");
				}
			}
		}

		[Category(TEShared.Names.Category_IO)]
		[Browsable(false)]
		public bool IsConnected
		{
			get { return this._Connection.IsConnected; }
		}

		#endregion

		#region Constructors

		public OscOutput() { }

		public OscOutput(Guid id, [NotNull] string host, int port)
			: base(id)
		{
			this.Host = host;
			this.Port = port;
		}

		#endregion

		#region Methods

		public override void ReadXElement(XElement element)
		{
			base.ReadXElement(element);
			var host = (string)element.Attribute(Schema.oscoutput_host);
			var port = (int)element.Attribute(Schema.oscoutput_port);
			this._Connection.Open(host, port);
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.oscoutput)
				.WithContent(
					new XAttribute(Schema.oscoutput_host, this.Host),
					new XAttribute(Schema.oscoutput_port, this.Port));
		}

		protected override bool PostMessageInternal(OutputMessage message)
		{
			Debug.Assert(message != null);
			if(!_Connection.IsConnected)
				return false;
			var packet = BuildPacket(message);
			if(packet == null)
				return false;
			try
			{
				_Connection.Send(packet);
				return true;
			}
			catch
			{
				return false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing)
				_Connection.Dispose();
		}

		#endregion

	}

	#endregion

}
