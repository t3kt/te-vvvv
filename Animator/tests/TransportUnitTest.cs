using System;
using System.Diagnostics;
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
		[TestCategory(CategoryNames.Transport)]
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
		[TestCategory(CategoryNames.Transport)]
		public void DocumentTransportTypesTest()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var doc = new Document(host);
			Assert.IsInstanceOfType(doc.Transport, typeof(NullTransport));
			doc.TransportType = "dummy";
			Assert.IsInstanceOfType(doc.Transport, typeof(DummyTransport));
			doc.TransportType = null;
			Assert.IsInstanceOfType(doc.Transport, typeof(NullTransport));
			doc.TransportType = "media";
			Assert.IsInstanceOfType(doc.Transport, typeof(MediaTransport));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Transport)]
		public void SanfordMultimediaTimerTicks()
		{
			var toleranceMS = 50;
			var toleranceTicks = 0;
			var period = 20;
			var reps = 10;
			var sleep = period * reps;
			using(var timer = new Sanford.Multimedia.Timers.Timer())
			{
				var caps = Sanford.Multimedia.Timers.Timer.Capabilities;
				Trace.WriteLine(String.Format("CAPS periodMin: {0} ms", caps.periodMin));
				Trace.WriteLine(String.Format("CAPS periodMax: {0} ms", caps.periodMax));

				Trace.WriteLine(String.Format("period: {0}ms  reps: {1}  sleep: {2}ms", period, reps, sleep));

				var tickCounter = new EventCounter();

				timer.Tick += tickCounter.Handler;

				Assert.AreEqual(0, tickCounter.Count);

				timer.Period = period;

				//{
				//    var startTime = DateTime.UtcNow;
				//    Trace.WriteLine(String.Format("Start time: {0}", startTime.ToLongTimeString()));
				//    Thread.Sleep(sleep);
				//    var endTime = DateTime.UtcNow;
				//    Trace.WriteLine(String.Format("End time: {0}", endTime.ToLongTimeString()));
				//    var diffTicks = endTime.Ticks - startTime.Ticks;
				//    var diffMs = diffTicks / TimeSpan.TicksPerMillisecond;
				//    Trace.WriteLine(String.Format("Duration: {0}ms", diffMs));

				//    var err = Math.Abs(sleep - diffMs);
				//    Assert.IsTrue(err <= toleranceMS);
				//}

				{
					var startTime = DateTime.UtcNow;
					Trace.WriteLine(String.Format("Start time: {0}", startTime.ToLongTimeString()));
					timer.Start();
					Thread.Sleep(sleep);
					timer.Stop();
					var endTime = DateTime.UtcNow;
					Trace.WriteLine(String.Format("End time: {0}", endTime.ToLongTimeString()));
					var dur = (endTime.Ticks - startTime.Ticks) / TimeSpan.TicksPerMillisecond;
					Trace.WriteLine(String.Format("Duration: {0}ms", dur));
					Assert.IsTrue(Math.Abs(sleep - dur) <= toleranceMS, "Sleep duration {0}ms is not in tolerance of expected duration {1}ms", dur, sleep);

					Trace.WriteLine(String.Format("Tick count: {0}", tickCounter.Count));
					Assert.IsTrue(Math.Abs(reps - tickCounter.Count) <= toleranceTicks, "Tick count {0} is not in tolerance of expected tick count {1}", tickCounter.Count, reps);
				}
			}
		}

		[TestMethod]
		[TestCategory(CategoryNames.Transport)]
		public void MediaTransportTiming()
		{
			var toleranceMS = 50;
			var toleranceTicks = 0;
			var period = 20;
			var reps = 10;
			var sleep = period * reps;
			using(var transport = new MediaTransport())
			{

				var tickCounter = new EventCounter();
				tickCounter.ExtraAction = () => { Trace.WriteLine("..transport tick #" + tickCounter.Count); };
				transport.Tick += (s, e) => tickCounter.Handler(s, e);

				var caps = Sanford.Multimedia.Timers.Timer.Capabilities;
				Trace.WriteLine(String.Format("CAPS periodMin: {0} ms", caps.periodMin));
				Trace.WriteLine(String.Format("CAPS periodMax: {0} ms", caps.periodMax));


				//Assert.AreEqual(MediaTransport_Accessor.DefaultPeriod, transport.Period);
				transport.Period = period;
				Trace.WriteLine(String.Format("Period: {0} ms", transport.Period));

				Assert.AreEqual(0, tickCounter.Count);

				var startTime = DateTime.UtcNow;
				Trace.WriteLine(String.Format("Start time: {0}", startTime.ToLongTimeString()));
				transport.Play();
				Thread.Sleep(sleep);
				transport.Stop();
				var endTime = DateTime.UtcNow;
				Trace.WriteLine(String.Format("End time: {0}", endTime.ToLongTimeString()));
				var dur = (endTime.Ticks - startTime.Ticks) / TimeSpan.TicksPerMillisecond;
				Trace.WriteLine(String.Format("Duration: {0}ms", dur));
				Assert.IsTrue(Math.Abs(sleep - dur) <= toleranceMS, "Sleep duration {0}ms is not in tolerance of expected duration {1}ms", dur, sleep);

				Trace.WriteLine(String.Format("Tick count: {0}", tickCounter.Count));
				Assert.IsTrue(Math.Abs(reps - tickCounter.Count) <= toleranceTicks, "Tick count {0} is not in tolerance of expected tick count {1}", tickCounter.Count, reps);
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
