using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
