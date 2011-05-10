using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;
using VVVV.Utils.OSC;

namespace Animator.Osc
{

	#region OscTransmitter

	[OutputTransmitter(Key = "osc", Description = "OSC Transport")]
	public class OscTransmitter : OutputTransmitter
	{

		#region Static / Constant

		private static Tuple<string, int> GetConnectionParams(Output outputModel)
		{
			Debug.Assert(outputModel != null);
			string host;
			if(!outputModel.Parameters.TryGetValue("host", out host))
				return null;
			string portStr;
			int port;
			if(!outputModel.Parameters.TryGetValue("port", out portStr) || !Int32.TryParse(portStr, out port))
				return null;
			return new Tuple<string, int>(host, port);
		}

		private static OSCPacket BuildPacket(OutputMessage message)
		{
			Debug.Assert(message != null);
			var address = message.TargetKey as string;
			if(String.IsNullOrEmpty(address))
				return null;
			var data = message.Data;
			if(data == null)
				return null;
			var packet = new OSCMessage(address);
			foreach(var item in data)
				packet.Append(item);
			return packet;
		}

		#endregion

		#region Fields

		private readonly OscConnection _Connection = new OscConnection();

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Common)]
		public string Host
		{
			get { return this._Connection.Host; }
			set { this._Connection.Host = value; }
		}

		[Category(TEShared.Names.Category_Common)]
		public int Port
		{
			get { return this._Connection.Port; }
			set { this._Connection.Port = value; }
		}

		[Category(TEShared.Names.Category_Common)]
		public bool IsConnected
		{
			get { return this._Connection.IsConnected; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override void Initialize(Output outputModel)
		{
			Debug.Assert(outputModel != null);
			var parms = GetConnectionParams(outputModel);
			if(parms == null)
				_Connection.Close();
			else
				_Connection.Open(parms.Item1, parms.Item2);
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
