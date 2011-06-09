using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;
using Animator.Core.Model.Sessions;
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
	public class SessionUnitTest
	{

		[TestMethod]
		[TestCategory(CategoryNames.Sessions)]
		public void GetActiveClips()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var doc = new Document(host);

			var clipA = new Clip { Name = "clipA" };
			doc.Clips.Add(clipA);
			var clipB = new StepClip { Name = "clipB", StepCount = 7 };
			doc.Clips.Add(clipB);
			var clipC = new Clip { Name = "clipC" };
			doc.Clips.Add(clipC);

			var ses = new Session(doc) { Name = "sesA" };
			doc.Sessions.Add(ses);

			var trackA = new SessionTrack(doc) { Name = "trackA" };
			ses.Tracks.Add(trackA);

			Assert.Inconclusive();

			//var trackA_clipA_Ref1 = new SessionClipReference(clipA) { Name = "trackA_clipA_Ref1" };
			//trackA.Clips.Add(trackA_clipA_Ref1);
			//var trackA_clipA_Ref2 = new SessionClipReference(clipA) { Name = "trackA_clipA_Ref2" };
			//trackA.Clips.Add(trackA_clipA_Ref2);
			//var trackA_clipB_Ref1 = new SessionClipReference(clipB) { Name = "trackA_clipB_Ref1" };
			//trackA.Clips.Add(trackA_clipB_Ref1);

			//var trackB = new SessionTrack(doc) { Name = "trackB" };
			//ses.Tracks.Add(trackB);
			//var trackB_clipA_Ref1 = new SessionClipReference(clipA) { Name = "trackB_clipA_Ref1" };
			//trackB.Clips.Add(trackB_clipA_Ref1);
			//var trackB_clipC_Ref1 = new SessionClipReference(clipC) { Name = "trackB_clipC_Ref1" };
			//trackB.Clips.Add(trackB_clipC_Ref1);

			//var transport = new DummyTransport();

			//{
			//    var active = trackA.GetActiveClips(transport).ToList();
			//    Assert.AreEqual(0, active.Count);
			//    active = ses.GetActiveClips(transport).ToList();
			//    Assert.AreEqual(0, active.Count);
			//}

			//trackA_clipA_Ref1.State = ClipState.Playing;
			//{
			//    var active = trackA.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(1, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackA_clipA_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//    active = ses.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(1, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackA_clipA_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//}

			//trackA_clipA_Ref1.State = ClipState.Stopped;
			//{
			//    var active = trackA.GetActiveClips(transport).ToList();
			//    Assert.AreEqual(0, active.Count);
			//    active = ses.GetActiveClips(transport).ToList();
			//    Assert.AreEqual(0, active.Count);
			//}

			//trackA_clipA_Ref1.State = ClipState.Playing;
			//trackA_clipB_Ref1.State = ClipState.Playing;
			//{
			//    var active = trackA.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(2, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackA_clipA_Ref1, trackA_clipB_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//    active = ses.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(2, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackA_clipA_Ref1, trackA_clipB_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//}

			//trackB_clipA_Ref1.State = ClipState.Playing;
			//{
			//    var active = trackA.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(2, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackA_clipA_Ref1, trackA_clipB_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//    active = trackB.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(1, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackB_clipA_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//    active = ses.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(3, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackA_clipA_Ref1, trackA_clipB_Ref1, trackB_clipA_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//}
			//Assert.IsTrue(trackA.Clips.Remove(trackA_clipA_Ref1));
			//{
			//    var active = trackA.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(1, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackA_clipB_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//    active = trackB.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(1, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackB_clipA_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//    active = ses.GetActiveClips(transport).OrderBy(c => c.Id).ToList();
			//    Assert.AreEqual(2, active.Count);
			//    CollectionAssert.AreEqual((new[] { trackA_clipB_Ref1, trackB_clipA_Ref1 }).OrderBy(c => c.Id).ToList(), active);
			//}

		}

	}

	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
