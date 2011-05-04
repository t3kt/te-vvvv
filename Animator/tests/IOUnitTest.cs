using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Runtime;
using Animator.Tests;
using Animator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: RegisteredImplementation(typeof(IOutputTransmitter), "test", typeof(IOUnitTest.TestTransmitter))]

namespace Animator.Tests
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression

	[TestClass]
	public class IOUnitTest
	{

		[ClassInitialize]
		public static void RegOutputTypes(TestContext testContext)
		{
			OutputTransmitter.TypeRegistry.RegisterTypes(typeof(IOUnitTest).Assembly);
		}

		[TestMethod]
		[TestCategory("IO")]
		public void CreateNullTransmitter()
		{
			var outputModel = new Output(Guid.Empty) { OutputType = null };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			Assert.IsInstanceOfType(transmitter, typeof(OutputTransmitter.NullTransmitter));
		}

		#region TestTransmitter

		[OutputTransmitter(Key = "test")]
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
			OutputTransmitter.TypeRegistry.RegisterTypes(this.GetType().Assembly);
			var outputModel = new Output(Guid.Empty) { OutputType = "test" };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			Assert.IsInstanceOfType(transmitter, typeof(TestTransmitter));
		}

		[TestMethod]
		[TestCategory("IO")]
		public void CreateTraceTransmitter()
		{
			var outputModel = new Output(Guid.Empty) { OutputType = "trace" };
			var transmitter = OutputTransmitter.CreateTransmitter(outputModel);
			Assert.IsInstanceOfType(transmitter, typeof(OutputTransmitter.TraceTransmitter));
		}

		[TestMethod]
		[TestCategory("IO")]
		public void TraceTransmitterOutput()
		{
			var outputModel = new Output(Guid.Empty) { OutputType = "trace" };
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

	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
