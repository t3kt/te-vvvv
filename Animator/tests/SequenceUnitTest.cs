using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Common;
using Animator.Core.Model;
using Animator.Core.Model.Sequences;
using Animator.Core.Runtime;
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
#pragma warning disable 168

	[TestClass]
	public class SequenceUnitTest
	{

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SeqClipContainsPosition()
		{
			// [1 -> 3]
			var clipA_start = TimeSpan.FromSeconds(1);
			var clipA_dur = TimeSpan.FromSeconds(2);
			var clipA_end = clipA_start + clipA_dur;
			var clipA = new SequenceClip
			{
				Start = clipA_start,
				Duration = clipA_dur
			};

			Assert.IsFalse(clipA.ContainsPosition(TimeSpan.Zero));
			Assert.IsTrue(clipA.ContainsPosition(TimeSpan.FromSeconds(1)));
			Assert.IsTrue(clipA.ContainsPosition(TimeSpan.FromSeconds(1.2)));
			Assert.IsTrue(clipA.ContainsPosition(TimeSpan.FromSeconds(2.7)));
			Assert.IsTrue(clipA.ContainsPosition(TimeSpan.FromSeconds(2.8)));
			Assert.IsTrue(clipA.ContainsPosition(TimeSpan.FromSeconds(2.99)));
			Assert.IsFalse(clipA.ContainsPosition(TimeSpan.FromSeconds(3)));
			Assert.IsFalse(clipA.ContainsPosition(TimeSpan.FromSeconds(4)));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SeqTrackGetActiveClip()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var doc = new Document(host);

			var seq = new Sequence();
			doc.Sections.Add(seq);

			var track = new SequenceTrack(doc);
			seq.Tracks.Add(track);

			Assert.IsNull(track.ActiveClip);

			// [1 -> 3]
			var clipA_start = TimeSpan.FromSeconds(1);
			var clipA_dur = TimeSpan.FromSeconds(2);
			var clipA_end = clipA_start + clipA_dur;
			var clipA = new SequenceClip
			{
				Start = clipA_start,
				Duration = clipA_dur
			};

			// [5 -> 7]
			var clipB_start = TimeSpan.FromSeconds(5);
			var clipB_dur = TimeSpan.FromSeconds(2);
			var clipB_end = clipB_start + clipB_dur;
			var clipB = new SequenceClip
			{
				Start = clipB_start,
				Duration = clipB_dur
			};

			track.Clips.Add(clipA);
			track.Clips.Add(clipB);

			Assert.AreEqual(clipA_start, clipA.Start);
			Assert.AreEqual(clipA_dur, clipA.Duration);
			Assert.AreEqual(clipA_end, clipA.End);

			Assert.AreEqual(clipB_start, clipB.Start);
			Assert.AreEqual(clipB_dur, clipB.Duration);
			Assert.AreEqual(clipB_end, clipB.End);

			Assert.IsNull(track.ActiveClip);

			track.UpdatePosition(TimeSpan.Zero);
			Assert.IsNull(track.ActiveClip);

			track.UpdatePosition(clipA_start);
			Assert.AreSame(clipA, track.ActiveClip);

			track.UpdatePosition(clipA_start + TimeSpan.FromSeconds(1));
			Assert.AreSame(clipA, track.ActiveClip);

			track.UpdatePosition(clipA_end);
			Assert.IsNull(track.ActiveClip);

			track.UpdatePosition(clipB_start);
			Assert.AreSame(clipB, track.ActiveClip);

			track.UpdatePosition(clipB_end);
			Assert.IsNull(track.ActiveClip);

			track.UpdatePosition(clipB_end + TimeSpan.FromSeconds(1));
			Assert.IsNull(track.ActiveClip);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SequenceGetActiveClips()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SeqTrackPreventClipOverlap()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var doc = new Document(host);

			var seq = new Sequence();
			doc.Sections.Add(seq);

			var track = new SequenceTrack(doc);
			seq.Tracks.Add(track);

			// [1 -> 3]
			var clipA_start = TimeSpan.FromSeconds(1);
			var clipA_dur = TimeSpan.FromSeconds(2);
			var clipA_end = clipA_start + clipA_dur;
			var clipA = new SequenceClip
						{
							Start = clipA_start,
							Duration = clipA_dur
						};
			Assert.AreEqual(clipA_start, clipA.Start);
			Assert.AreEqual(clipA_dur, clipA.Duration);
			Assert.AreEqual(clipA_end, clipA.End);

			track.Clips.Add(clipA);
			Assert.AreEqual(clipA_start, clipA.Start);
			Assert.AreEqual(clipA_dur, clipA.Duration);
			Assert.AreEqual(clipA_end, clipA.End);

			// [4 -> 6]
			var clipB_start = TimeSpan.FromSeconds(4);
			var clipB_dur = TimeSpan.FromSeconds(2);
			var clipB_end = clipB_start + clipB_dur;
			var clipB = new SequenceClip
						{
							Start = clipB_start,
							Duration = clipB_dur
						};
			Assert.AreEqual(clipB_start, clipB.Start);
			Assert.AreEqual(clipB_dur, clipB.Duration);
			Assert.AreEqual(clipB_end, clipB.End);

			track.Clips.Add(clipB);

			Assert.AreEqual(clipA_start, clipA.Start);
			Assert.AreEqual(clipA_dur, clipA.Duration);
			Assert.AreEqual(clipA_end, clipA.End);

			Assert.AreEqual(clipB_start, clipB.Start);
			Assert.AreEqual(clipB_dur, clipB.Duration);
			Assert.AreEqual(clipB_end, clipB.End);

			// [5 -> 10]
			var clipC_start = TimeSpan.FromSeconds(5);
			var clipC_dur = TimeSpan.FromSeconds(5);
			var clipC_end = clipC_start + clipC_dur;
			var clipC = new SequenceClip
			{
				Start = clipC_start,
				Duration = clipC_dur
			};
			Assert.AreEqual(clipC_start, clipC.Start);
			Assert.AreEqual(clipC_dur, clipC.Duration);
			Assert.AreEqual(clipC_end, clipC.End);

			// [6 -> 10]
			track.Clips.Add(clipC);

			Assert.AreEqual(clipA_start, clipA.Start);
			Assert.AreEqual(clipA_dur, clipA.Duration);
			Assert.AreEqual(clipA_end, clipA.End);

			Assert.AreEqual(clipB_start, clipB.Start);
			Assert.AreEqual(clipB_dur, clipB.Duration);
			Assert.AreEqual(clipB_end, clipB.End);

			Assert.AreEqual(clipB_end, clipC.Start);
			Assert.AreEqual(clipC_end - clipB_end, clipC.Duration);
			Assert.AreEqual(clipC_end, clipC.End);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SeqClipPushTargetValues()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SeqTrackPushTargetValues()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SequencePushTargetValues()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SequencePositionUpdate()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		[TestCategory(CategoryNames.Sequences)]
		public void SeqClipApproveDenyChanges()
		{
			var clip = new SequenceClip();

			//Assert.AreEqual(TimeSpan.Zero, clip.Start);
			//Assert.AreEqual(TimeSpan.Zero, clip.Duration);

			var a_start = TimeSpan.FromSeconds(1);
			var a_dur = TimeSpan.FromSeconds(2);
			var a_int = new Interval(a_start, a_dur);

			clip.ChangeInterval(a_int);
			Assert.AreEqual(a_start, clip.Start);
			Assert.AreEqual(a_dur, clip.Duration);

			clip.SetIntervalChangeHandler(
				(s, e) =>
				{
					e.Approve();
				});

			var b_start = TimeSpan.FromSeconds(3);
			var b_dur = TimeSpan.FromSeconds(4);
			var b_int = new Interval(b_start, b_dur);
			clip.ChangeInterval(b_int);
			Assert.AreEqual(b_start, clip.Start);
			Assert.AreEqual(b_dur, clip.Duration);

			clip.SetIntervalChangeHandler(
				(s, e) =>
				{
					e.Deny();
				});
			var c_start = TimeSpan.FromSeconds(5);
			var c_dur = TimeSpan.FromSeconds(6);
			var c_int = new Interval(c_start, c_dur);
			clip.ChangeInterval(c_int);
			Assert.AreEqual(b_start, clip.Start);
			Assert.AreEqual(b_dur, clip.Duration);

			clip.SetIntervalChangeHandler(
				(s, e) =>
				{
				});
			clip.ChangeInterval(c_int);
			Assert.AreEqual(c_start, clip.Start);
			Assert.AreEqual(c_dur, clip.Duration);

			var d_start = TimeSpan.FromSeconds(7);
			var d_dur = TimeSpan.FromSeconds(8);
			var d_int = new Interval(d_start, d_dur);

			var e_start = TimeSpan.FromSeconds(9);
			var e_dur = TimeSpan.FromSeconds(10);
			var e_int = new Interval(e_start, e_dur);

			clip.SetIntervalChangeHandler(
				(s, e) =>
				{
					e.ApproveModified(e_int);
				});
			clip.ChangeInterval(d_int);
			Assert.AreEqual(e_start, clip.Start);
			Assert.AreEqual(e_dur, clip.Duration);
		}

		private static void AssertIntervalsEqual(Interval expected, Interval actual, string message = null)
		{
			Tracer.Line("Expected interval: {0}", expected);
			Tracer.Line("Actual interval:   {0}", actual);
			Assert.AreEqual(expected, actual, message);
			Tracer.Line("... they're equal!");
		}

		[TestCategory(CategoryNames.Sequences)]
		[TestMethod]
		public void SeqClipCollectionPreventInsertOverlaps()
		{
			var clips = new SequenceClipCollection(new SequenceTrack());

			var a_start = TimeSpan.FromSeconds(1);
			var a_dur = TimeSpan.FromSeconds(2);
			var a_end = a_start + a_dur;// 3
			var a_interval = new Interval(a_start, a_dur);
			var a = new SequenceClip(a_interval);
			Assert.IsTrue(clips.Add(a));
			Assert.AreEqual(a_interval, a.Interval);
			Assert.AreEqual(1, clips.Count);
			CollectionAssert.AreEqual(new[] { a }, clips.ToArray());

			var b_start = TimeSpan.FromSeconds(5);
			var b_dur = TimeSpan.FromSeconds(3);
			var b_end = b_start + b_dur; //	8
			var b_interval = new Interval(b_start, b_dur);
			var b = new SequenceClip(b_interval);
			Assert.AreEqual(b_interval, b.Interval);
			Assert.IsTrue(clips.Add(b));
			Assert.AreEqual(b_interval, b.Interval);
			CollectionAssert.AreEqual(new[] { a, b }, clips.ToArray());

			var c_start = TimeSpan.FromSeconds(6);
			var c_dur = TimeSpan.FromSeconds(1);
			var c_end = c_start + c_dur;// 7
			var c_interval = new Interval(c_start, c_dur);
			var c = new SequenceClip(c_interval);
			Assert.AreEqual(c_interval, c.Interval);
			Assert.IsFalse(clips.Add(c));
			Assert.AreEqual(c_interval, c.Interval);
			CollectionAssert.AreEqual(new[] { a, b }, clips.ToArray());
		}

		private static string DumpSeqClipIntervals(IEnumerable<SequenceClip> clips)
		{
			return String.Join("  ", clips.Select(c => String.Format("[{0} <--{1}--> {2}] ", c.Start, c.Name, c.End)));
		}

		[TestCategory(CategoryNames.Sequences)]
		[TestMethod]
		public void SeqClipCollectionPreventClipPropertyChangeOverlaps()
		{
			var clips = new SequenceClipCollection(new SequenceTrack());

			var a_interval = new Interval(1, 2);
			SequenceClip a;
			using(Tracer.CategorySection("Clip A", "init"))
			{
				Tracer.CategoryLine("Clip A", "initial interval: {0}", a_interval);
				//Debug.WriteLine("");
				//a = new SequenceClip { Start = a_start, Duration = a_dur };
				a = new SequenceClip(a_interval) { Name = "A" };
				Assert.AreEqual(a_interval, a.Interval);
				Tracer.CategoryLine("Clip A", "equal to interval {0}", a_interval);
				Assert.IsTrue(clips.Add(a));
				Assert.AreEqual(a_interval, a.Interval);
				Tracer.CategoryLine("Clip A", "equal to interval {0}", a_interval);
			}

			Trace.WriteLine(DumpSeqClipIntervals(clips), "SeqClip Intervals");

			var a_interval_new = new Interval(a_interval.Start, 3);
			using(Tracer.CategorySection("Clip A", "change duration"))
			{
				Tracer.CategoryLine("Clip A", "current interval: {0}", a.Interval);
				Tracer.CategoryLine("Clip A", "Trying to change duration from {0} to {1}", a_interval.Duration, a_interval_new.Duration);
				a.Duration = a_interval_new.Duration;
				Assert.AreEqual(a_interval_new.Duration, a.Duration, "Clip A duration have successfully been set");
				Assert.AreEqual(a_interval_new, a.Interval);
				Tracer.CategoryLine("Clip A", "equal to new interval {0}", a_interval_new);
				CollectionAssert.AreEqual(new[] { a }, clips.ToArray());
			}

			Trace.WriteLine(DumpSeqClipIntervals(clips), "SeqClip Intervals");

			var b_interval = new Interval(5, 3);
			SequenceClip b;
			using(Tracer.CategorySection("Clip B", "init"))
			{
				Tracer.CategoryLine("Clip B", "Creating clip B");
				Tracer.CategoryLine("Clip B", "initial interval: {0}", b_interval);
				//b = new SequenceClip { Start = b_start, Duration = b_dur };
				b = new SequenceClip(b_interval) { Name = "B" };
				Assert.AreEqual(b_interval, b.Interval);
				Tracer.CategoryLine("Clip B", "equal to interval {0}", b_interval);
				Assert.IsTrue(clips.Add(b));
				Assert.AreEqual(b_interval, b.Interval);
				Tracer.CategoryLine("Clip B", "equal to interval {0}", b_interval);
				CollectionAssert.AreEqual(new[] { a, b }, clips.ToArray());
			}

			Trace.WriteLine(DumpSeqClipIntervals(clips), "SeqClip Intervals");

			using(Tracer.CategorySection("Clip B", "change start (with interval)"))
			{
				var b_interval_new = Interval.FromBounds(TimeSpan.FromSeconds(2), b_interval.End);

				Tracer.CategoryLine("Clip A", "interval: {0}", a.Interval);
				Tracer.CategoryLine("Clip B", "interval: {0}", b.Interval);
				Tracer.CategoryLine("Clip B", "Trying to change clip B start from {0} to {1} (new interval would be: {2})...", b_interval.Start, b_interval_new.Start, b_interval_new);
				var decision = b.TryChangeInterval(b_interval_new);
				Assert.AreEqual(TryChangeValueDecision.Denied, decision);
				//b.Start = b_start_new;
				Tracer.CategoryLine("Clip B", "new actual interval: {0}", b.Interval);
				Assert.AreEqual(b_interval.Start, b.Start);
				Assert.AreNotEqual(b_interval_new.Start, b.Start);
				Assert.AreEqual(b_interval, b.Interval);
				Tracer.CategoryLine("Clip B", "equal to interval {0}", b_interval);
				CollectionAssert.AreEqual(new[] { a, b }, clips.ToArray());
			}

			Trace.WriteLine(DumpSeqClipIntervals(clips), "SeqClip Intervals");

			using(Tracer.CategorySection("Clip B", "change start"))
			{
				var b_interval_new = Interval.FromBounds(TimeSpan.FromSeconds(2), b_interval.End);

				var b_interval_new_actual = Interval.FromBounds(a_interval_new.End, b_interval.End);

				Tracer.CategoryLine("Clip A", "interval: {0}", a.Interval);
				Tracer.CategoryLine("Clip B", "interval: {0}", b.Interval);
				Tracer.CategoryLine("Clip B", "Trying to change clip B start from {0} to {1} (new interval would be: {2})...", b_interval.Start, b_interval_new.Start, b_interval_new);
				var decision = b.TryChangeStart(b_interval_new.Start);
				Assert.AreEqual(TryChangeValueDecision.Denied, decision);
				//b.Start = b_start_new;
				Tracer.CategoryLine("Clip B", "new actual interval: {0}", b.Interval);
				Assert.AreEqual(b_interval.Start, b.Start);
				Assert.AreNotEqual(b_interval_new.Start, b.Start);
				Assert.AreEqual(b_interval, b.Interval);
				Tracer.CategoryLine("Clip B", "equal to interval {0}", b_interval);
				CollectionAssert.AreEqual(new[] { a, b }, clips.ToArray());
			}
		}

	}

#pragma warning restore 168
	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
