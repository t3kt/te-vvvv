using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Animator.Tests.Utils
{

	#region CallbackTraceListener

	internal class CallbackTraceListener : TraceListener
	{

		private readonly Action<string> _Write;
		private readonly Action<string> _WriteLine;

		public CallbackTraceListener(Action<string> write, Action<string> writeLine)
		{
			this._Write = write;
			this._WriteLine = writeLine;
		}

		public CallbackTraceListener(Action<string> write)
		{
			this._Write = write;
			this._WriteLine = write;
		}

		public override void Write(string message)
		{
			this._Write(message);
		}

		public override void WriteLine(string message)
		{
			this._WriteLine(message);
		}
	}

	#endregion

}
