using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Runtime;
using Animator.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: RegisteredImplementation(typeof(IOutputTransmitter), "test", typeof(IOUnitTest.TestTransmitter))]

namespace Animator.Tests
{
	[TestClass]
	public class IOUnitTest
	{

		[TestMethod]
		[TestCategory("IO")]
		public void CreateNullTransmitter()
		{
			var outputModel = new Output(null, Guid.Empty) { OutputType = null };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			Assert.IsInstanceOfType(transmitter, typeof(OutputTransmitter.NullTransmitter));
		}

		#region TestTransmitter

		internal class TestTransmitter : OutputTransmitter
		{

			protected override bool PostMessageInternal(OutputMessage message)
			{
				return false;
			}

		}

		#endregion

		[TestMethod]
		[TestCategory("IO")]
		public void RegisterTransmitter()
		{
			OutputTransmitter.RegisterTypes(this.GetType().Assembly);
			var outputModel = new Output(null, Guid.Empty) { OutputType = "test" };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			Assert.IsInstanceOfType(transmitter, typeof(TestTransmitter));
		}

		[TestMethod]
		[TestCategory("IO")]
		public void CreateTraceTransmitter()
		{
			var outputModel = new Output(null, Guid.Empty) { OutputType = "trace" };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			Assert.IsInstanceOfType(transmitter, typeof(OutputTransmitter.TraceTransmitter));
		}

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

		[TestMethod]
		[TestCategory("IO")]
		public void TraceTransmitterOutput()
		{
			var outputModel = new Output(null, Guid.Empty) { OutputType = "trace" };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			int writeCount = 0;
			var listener = new CallbackTraceListener(msg =>
														{
															writeCount++;
														});
			Trace.Listeners.Add(listener);
			try
			{
				transmitter.PostMessage(new OutputMessage("key1", 5));
				transmitter.PostMessage(new OutputMessage("key2", new object[] { "x", 5 }));
				Assert.AreEqual(2, writeCount);
			}
			finally
			{
				Trace.Listeners.Remove(listener);
			}
		}

	}
}
