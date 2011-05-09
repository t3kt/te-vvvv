using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Animator.Core.Model;
using Animator.Core.Transport;
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
	public class TransportUnitTest
	{

		[TestMethod]
		[TestCategory("Transport")]
		public void MediaTransportBasicStateChangeTest()
		{
			using(var transport = new MediaTransport())
			{
				var mainThreadId = Thread.CurrentThread.ManagedThreadId;
				Action assertInMainThread = () => Assert.AreEqual(mainThreadId, Thread.CurrentThread.ManagedThreadId);

				var stateChangeCounter = new LockingEventCounter { ExtraAction = assertInMainThread };
				//var posChangeCounter = new LockingEventCounter { ExtraAction = assertInMainThread };
				//var tickCounter = new LockingEventCounter { ExtraAction = assertInMainThread };

				transport.StateChanged += stateChangeCounter.Handler;
				//transport.PositionChanged += posChangeCounter.Handler;
				//transport.Tick += tickCounter.Handler;

				Assert.AreEqual(TransportState.Stopped, transport.State);
				Assert.AreEqual(0, stateChangeCounter.Count);

				transport.Play();
				Assert.AreEqual(TransportState.Playing, transport.State);
				Assert.AreEqual(1, stateChangeCounter.Count);

				transport.Pause();
				Assert.AreEqual(TransportState.Stopped, transport.State);
				Assert.AreEqual(2, stateChangeCounter.Count);

				transport.Pause();
				Assert.AreEqual(TransportState.Stopped, transport.State);
				Assert.AreEqual(2, stateChangeCounter.Count);

				transport.Stop();
				Assert.AreEqual(TransportState.Stopped, transport.State);
				Assert.AreEqual(2, stateChangeCounter.Count);

				transport.Play();
				Assert.AreEqual(TransportState.Playing, transport.State);
				Assert.AreEqual(3, stateChangeCounter.Count);

				transport.Stop();
				Assert.AreEqual(TransportState.Stopped, transport.State);
				Assert.AreEqual(4, stateChangeCounter.Count);
			}
		}

		[TestMethod]
		[TestCategory("Transport")]
		public void DocumentTransportTypesTest()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var doc = new Document(host);
			Assert.IsInstanceOfType(doc.Transport, typeof(Transport.NullTransport));
			doc.TransportType = "dummy";
			Assert.IsInstanceOfType(doc.Transport, typeof(DummyTransport));
			doc.TransportType = null;
			Assert.IsInstanceOfType(doc.Transport, typeof(Transport.NullTransport));
			doc.TransportType = "media";
			Assert.IsInstanceOfType(doc.Transport, typeof(MediaTransport));
		}

	}

	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
