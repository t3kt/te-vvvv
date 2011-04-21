using System;
using System.Collections;
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
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression

	[TestClass]
	public class IOUnitTest
	{

		[TestMethod]
		[TestCategory("IO")]
		public void CreateNullTransmitter()
		{
			var outputModel = new Output(Guid.Empty) { OutputType = null };
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


		#region OutputMessageComparer

		internal class OutputMessageComparer : EqualityComparer<OutputMessage>, IComparer<OutputMessage>, IComparer
		{

			public static readonly OutputMessageComparer Instance = new OutputMessageComparer();

			public override bool Equals(OutputMessage x, OutputMessage y)
			{
				if(x == null)
					return y == null;
				if(y == null)
					return true;
				if(x.TargetKey != y.TargetKey)
					return false;
				if(x.Data == y.Data)
					return true;
				if(x.Data == null)
					return y.Data == null;
				if(y.Data == null)
					return false;
				return x.Data.SequenceEqual(y.Data);
			}

			public override int GetHashCode(OutputMessage obj)
			{
				if(obj == null || obj.TargetKey == null)
					return 0;
				return obj.TargetKey.GetHashCode();
			}

			public int Compare(OutputMessage x, OutputMessage y)
			{
				if(x == null)
					return y == null ? 0 : -1;
				if(y == null)
					return 1;
				var cmp = Comparer<object>.Default;
				var result = cmp.Compare(x.TargetKey, y.TargetKey);
				if(result != 0)
					return result;
				if(x.Data == y.Data)
					return 0;
				if(x.Data == null)
					return y.Data == null ? 0 : -1;
				if(y.Data == null)
					return 1;
				result = x.Data.Length.CompareTo(y.Data.Length);
				if(result != 0)
					return result;
				for(var i = 0; i < x.Data.Length; i++)
				{
					result = cmp.Compare(x.Data[i], y.Data[i]);
					if(result != 0)
						return result;
				}
				return 0;
			}

			public int Compare(object x, object y)
			{
				return Compare(x as OutputMessage, y as OutputMessage);
			}

		}

		#endregion


	}

	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
