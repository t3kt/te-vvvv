using System;
using System.Collections;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression
	// ReSharper disable RedundantArgumentName

	[TestClass]
	public class IOUnitTest
	{

		#region TestOutput

		[Output(Key = Export_Key)]
		[PartCreationPolicy(CreationPolicy.NonShared)]
		internal class TestOutput : Output
		{

			internal new const string Export_Key = "test";
			internal new const string Export_ElementName = null;
			internal new const string Export_Description = "TEST Output";

			protected override bool PostMessageInternal(OutputMessage message)
			{
				return false;
			}

		}

		#endregion

		[TestMethod]
		[TestCategory(CategoryNames.IO)]
		public void RegisterTransmitter()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var output = host.CreateOutputByKey(TestOutput.Export_Key);
			Assert.IsInstanceOfType(output, typeof(TestOutput));
		}

		[TestMethod]
		[TestCategory(CategoryNames.IO)]
		public void CreateTraceOutput()
		{
			var host = CompositionUnitTest.CreateHost(test: false, core: true);
			var output = host.CreateOutputByKey(TraceOutput.Export_Key);
			Assert.IsInstanceOfType(output, typeof(TraceOutput));
		}

		[TestMethod]
		[TestCategory(CategoryNames.IO)]
		public void TraceOutputMessaging()
		{
			var host = CompositionUnitTest.CreateHost(test: false, core: true);
			var output = host.CreateOutputByKey(TraceOutput.Export_Key);
			Assert.IsInstanceOfType(output, typeof(TraceOutput));
			int writeCount = 0;
			var listener = new CallbackTraceListener(msg =>
														{
															writeCount++;
														});
			Trace.Listeners.Add(listener);
			try
			{
				output.PostMessage(new OutputMessage("key1", 5));
				output.PostMessage(new OutputMessage("key2", new object[] { "x", 5 }));
				Assert.AreEqual(2, writeCount);
			}
			finally
			{
				Trace.Listeners.Remove(listener);
			}
		}

	}

	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
