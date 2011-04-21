using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Core.IO
{

	#region OutputMessageEventArgs

	public sealed class OutputMessageEventArgs : EventArgs
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly OutputMessage _Message;

		#endregion

		#region Properties

		public OutputMessage Message
		{
			get { return this._Message; }
		}

		#endregion

		#region Constructors

		public OutputMessageEventArgs(OutputMessage message)
		{
			this._Message = message;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region OutputMessage

	public sealed class OutputMessage
	{

		#region Static / Constant

		internal static string FormatTrace(OutputMessage message)
		{
			if(message == null)
				return null;
			return String.Format("TargetKey: '{0}' Data: {1}",
								 message.TargetKey ?? "(null)",
								 message.Data == null || message.Data.Length == 0 ? "(no data)" : String.Join(",", message.Data));
		}

		#endregion

		#region Fields

		private readonly object _TargetKey;
		private readonly object[] _Data;

		#endregion

		#region Properties

		public object TargetKey
		{
			get { return this._TargetKey; }
		}

		public object[] Data
		{
			get { return this._Data; }
		}

		#endregion

		#region Constructors

		public OutputMessage(object targetKey, object[] data)
		{
			this._TargetKey = targetKey;
			this._Data = data;
		}

		public OutputMessage(object targetKey, object data)
		{
			this._TargetKey = targetKey;
			if(data == null)
				this._Data = null;
			else if(data is object[])
				this._Data = (object[])data;
			else
				this._Data = new[] { data };
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
