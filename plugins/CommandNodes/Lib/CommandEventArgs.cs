using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region CommandEventArgs

	public sealed class CommandEventArgs : EventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly string _CommandId;

		#endregion

		#region Properties

		public string CommandId
		{
			get { return _CommandId; }
		}

		#endregion

		#region Constructors

		internal CommandEventArgs(string commandId)
		{
			_CommandId = commandId;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
