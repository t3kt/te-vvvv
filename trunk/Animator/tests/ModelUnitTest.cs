using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;
using Animator.Core.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression

	[TestClass]
	public class ModelUnitTest
	{

		[TestMethod]
		[TestCategory("Model")]
		public void ParametersEqual()
		{
			var x =
				new Dictionary<string, string>
				{
					{"foo", "--foo....."},
					{"baz",String.Empty},
					{"qux",null}
				};
			var y =
				new Dictionary<string, string>
				{
					{"baz",String.Empty},
					{"qux",null},
					{"foo", "--foo....."}
				};
			Assert.IsTrue(ModelUtil.ParametersEqual(x, y));
		}

		[TestMethod]
		[TestCategory("Model")]
		public void ClipReadWriteXElement()
		{
			var clipA = new Clip { ClipType = "FooClipType", Name = "helloclip", TriggerAlignment = 4 };
			var xmlA = clipA.WriteXElement();
			var clipB = new Clip(xmlA);
			var xmlB = clipB.WriteXElement();
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			Assert.AreEqual(clipA, clipB);
		}

		[TestMethod]
		[TestCategory("Model")]
		public void ClipEqualityComparison()
		{
			var clipA = new Clip(Guid.NewGuid())
						{
							Duration = 3.0f,
							ClipType = "misc",
							Name = "fooClip",
							TargetKey = "tgt"
						};
			var clipB = new Clip(clipA.Id)
						{
							Duration = clipA.Duration,
							ClipType = clipA.ClipType,
							Name = clipA.Name,
							TargetKey = clipA.TargetKey
						};
			Assert.AreEqual(clipA, clipB);
			clipB.Duration = -999.33f;
			Assert.AreNotEqual(clipA, clipB);

			var stepClipA = new StepClip(Guid.NewGuid())
							{
								Duration = 9.0f,
								ClipType = "step",
								Name = "stepssss",
								TargetKey = "tttgtt",
								Steps = new ObservableCollection<float> { 3.5f, 12.0f }
							};
			Assert.AreNotEqual(clipA, stepClipA);
			var stepClipB = new StepClip(stepClipA.Id)
							{
								Duration = stepClipA.Duration,
								ClipType = stepClipA.ClipType,
								Name = stepClipA.Name,
								TargetKey = stepClipA.TargetKey,
								Steps = new ObservableCollection<float>(stepClipA.Steps.ToArray())
							};
			Assert.AreEqual(stepClipA, stepClipB);
			stepClipB.Steps.Add(-234.77f);
			Assert.AreNotEqual(stepClipA, stepClipB);
			stepClipB.Steps.RemoveAt(stepClipB.Steps.Count - 1);
			stepClipB.Name = "qqq";
			Assert.AreNotEqual(stepClipA, stepClipB);
			stepClipB.Name = stepClipA.Name;
			Assert.AreEqual(stepClipA, stepClipB);
		}

		[TestMethod]
		[TestCategory("Model")]
		public void TrackReadWriteXElement()
		{
			var docA = new Document();
			var trackA = new Track();
			docA.AddTrack(trackA);
			var clipA = new Clip { ClipType = "FooClipType", Name = "helloclip", TriggerAlignment = 4 };
			trackA.Clips.Add(clipA);
			trackA.Clips.Add(null);
			var clipB = new StepClip
			{
				Name = "foosteps",
				Steps = new ObservableCollection<float> { 5.0f, -2.3f, 3.73e+4f }
			};
			trackA.Clips.Add(clipB);
			var xmlA = trackA.WriteXElement();

			var trackB = new Track(xmlA);
			var xmlB = trackB.WriteXElement();
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			Assert.AreEqual(trackA, trackB);
		}

		[TestMethod]
		[TestCategory("Model")]
		public void NoDuplicateIds()
		{
			var doc = new Document();
			var output = new Output();
			var track = new Track(output.Id);
			TestUtil.AssertThrowsException(() =>
											{
												doc.AddTrack(track);
											}, "Add duplicate id allowed");
		}

		[TestMethod]
		[TestCategory("Model")]
		public void StepClipGetValue()
		{
			var doc = new Document();
			var track = new Track();
			doc.AddTrack(track);
			var clip = new StepClip
			{
				Duration = new Time(4),
				Steps = new ObservableCollection<float> { 0.0f, 1.0f, 2.0f, 3.0f }
			};
			track.Clips.Add(clip);
			Assert.AreEqual(0.0f, clip.GetValue(new Time(0.0f)));
			Assert.AreEqual(1.0f, clip.GetValue(new Time(1.0f)));
			Assert.AreEqual(0.0f, clip.GetValue(new Time(0.4f)));
			Assert.AreEqual(0.0f, clip.GetValue(new Time(0.5f)));
			Assert.AreEqual(1.0f, clip.GetValue(new Time(1.5f)));
			Assert.AreEqual(1.0f, clip.GetValue(new Time(5.5f)));
		}

	}

	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
