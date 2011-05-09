using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
			var host = CompositionUnitTest.CreateHost(test: false, core: true);
			var clipA = new Clip { Name = "helloclip", TriggerAlignment = 4, OutputId = Guid.NewGuid(), UIRow = 12 };
			var xmlA = clipA.WriteXElement();
			var clipB = new Clip();
			clipB.ReadXElement(xmlA);
			var xmlB = clipB.WriteXElement();
			var clipC = host.ReadClipXElement(xmlA);
			var xmlC = clipC.WriteXElement();
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			Assert.AreEqual(clipA, clipB);
			Assert.AreEqual(clipA.UIRow, clipB.UIRow);
			Assert.AreEqual(clipA.UIColumn, clipB.UIColumn);
			Assert.AreEqual(xmlA.ToString(), xmlC.ToString());
			Assert.AreEqual(clipA, clipC);
			Assert.AreEqual(clipA.UIRow, clipC.UIRow);
			Assert.AreEqual(clipA.UIColumn, clipC.UIColumn);
		}

		[TestMethod]
		[TestCategory("Model")]
		public void StepClipReadWriteXElement()
		{
			var host = CompositionUnitTest.CreateHost(test: false, core: true);
			var clipA = new StepClip
						{
							Name = "helloclip",
							TriggerAlignment = 4,
							OutputId = Guid.NewGuid(),
							UIRow = 12,
							Steps = new ObservableCollection<float> { 40.2f, -345.28f, 0.0f, 4444.4f }
						};
			var xmlA = clipA.WriteXElement();
			var clipB = new StepClip(xmlA);
			var xmlB = clipB.WriteXElement();
			var clipC = host.ReadClipXElement(xmlA);
			Assert.IsInstanceOfType(clipC, typeof(StepClip));
			var xmlC = clipC.WriteXElement();
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			Assert.AreEqual(clipA, clipB);
			Assert.AreEqual(clipA.UIRow, clipB.UIRow);
			Assert.AreEqual(clipA.UIColumn, clipB.UIColumn);
			Assert.AreEqual(xmlA.ToString(), xmlC.ToString());
			Assert.AreEqual(clipA, clipC);
			Assert.AreEqual(clipA.UIRow, clipC.UIRow);
			Assert.AreEqual(clipA.UIColumn, clipC.UIColumn);
		}

		[TestMethod]
		[TestCategory("Model")]
		public void DocumentReadWriteXElement()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var docA = new Document
					   {
						   BeatsPerMinute = 44.3f,
						   Duration = 284.345f,
						   Name = "foodoc",
						   UIColumns = 23,
						   UIRows = 14
					   };
			var clipA1 = new Clip { Name = "helloclip", TriggerAlignment = 4, OutputId = Guid.NewGuid(), UIRow = 12 };
			docA.Clips.Add(clipA1);
			var clipA2 = new StepClip
						 {
							 Name = "steppp",
							 Duration = 48.5f,
							 Steps = new ObservableCollection<float> { 25.25f, -234.3f, 0.0f },
							 TargetKey = "footarget",
							 UIColumn = 4,
							 UIRow = 2
						 };
			docA.Clips.Add(clipA2);
			var outputA1 = new Output { Name = "out1", OutputType = "fooo_out" };
			docA.Outputs.Add(outputA1);
			var xmlA = docA.WriteXElement();
			var docB = new Document(xmlA, host);
			var xmlB = docB.WriteXElement();
			Assert.AreEqual(docA.BeatsPerMinute, docB.BeatsPerMinute);
			Assert.AreEqual(docA.Id, docB.Id);
			Assert.AreEqual(docA.Duration, docB.Duration);
			Assert.AreEqual(docA.Name, docB.Name);
			Assert.AreEqual(docA.UIColumns, docB.UIColumns);
			Assert.AreEqual(docA.UIRows, docB.UIRows);
			CollectionAssert.Contains(docB.Clips, clipA1);
			CollectionAssert.Contains(docB.Clips, clipA2);
			Assert.IsTrue(docB.Clips.ItemsEqual(docA.Clips));
			CollectionAssert.Contains(docB.Outputs, outputA1);
			Assert.IsTrue(docB.Outputs.ItemsEqual(docA.Outputs));
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
		}

		[TestMethod]
		[TestCategory("Model")]
		public void ClipEqualityComparison()
		{
			var clipA = new Clip(Guid.NewGuid())
						{
							Duration = 3.0f,
							Name = "fooClip",
							TargetKey = "tgt",
							OutputId = Guid.NewGuid()
						};
			var clipB = new Clip(clipA.Id)
						{
							Duration = clipA.Duration,
							Name = clipA.Name,
							TargetKey = clipA.TargetKey,
							OutputId = clipA.OutputId
						};
			Assert.AreEqual(clipA, clipB);
			clipB.Duration = -999.33f;
			Assert.AreNotEqual(clipA, clipB);

			var stepClipA = new StepClip(Guid.NewGuid())
							{
								Duration = 9.0f,
								Name = "stepssss",
								TargetKey = "tttgtt",
								Steps = new ObservableCollection<float> { 3.5f, 12.0f },
								OutputId = Guid.NewGuid()
							};
			Assert.AreNotEqual(clipA, stepClipA);
			var stepClipB = new StepClip(stepClipA.Id)
							{
								Duration = stepClipA.Duration,
								Name = stepClipA.Name,
								TargetKey = stepClipA.TargetKey,
								Steps = new ObservableCollection<float>(stepClipA.Steps.ToArray()),
								OutputId = stepClipA.OutputId
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
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var docA = new Document();
			var trackA = new Track();
			docA.Tracks.Add(trackA);
			var clipA = new Clip { Name = "helloclip", TriggerAlignment = 4, OutputId = Guid.NewGuid() };
			trackA.Clips.Add(clipA);
			trackA.Clips.Add(null);
			var clipB = new StepClip
			{
				Name = "foosteps",
				Steps = new ObservableCollection<float> { 5.0f, -2.3f, 3.73e+4f }
			};
			trackA.Clips.Add(clipB);
			var xmlA = trackA.WriteXElement();

			var trackB = new Track(xmlA, host);
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
											   doc.Tracks.Add(track);
										   }, "Add duplicate id allowed");
		}

		[TestMethod]
		[TestCategory("Model")]
		public void StepClipGetValue()
		{
			var doc = new Document();
			var track = new Track();
			doc.Tracks.Add(track);
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

	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
