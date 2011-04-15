using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Runtime;
using Animator.Core.Transport;
using Animator.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: RegisteredImplementation(typeof(IOutputTransmitter), "test.callback", typeof(RuntimeUnitTest.CallbackTransmitter))]
[assembly: RegisteredImplementation(typeof(IOutputTransmitter), "test.mtracker", typeof(RuntimeUnitTest.ModelTrackerTransmitter))]
[assembly: RegisteredImplementation(typeof(IOutputTransmitter), "test.collector", typeof(RuntimeUnitTest.CollectorTransmitter))]

namespace Animator.Tests
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression

	[TestClass]
	public class RuntimeUnitTest
	{

		#region CallbackTransmitter

		internal sealed class CallbackTransmitter : OutputTransmitter
		{

			public Func<OutputMessage, bool> PostMessageCallback;

			protected override bool PostMessageInternal(OutputMessage message)
			{
				if(this.PostMessageCallback == null)
					return false;
				return this.PostMessageCallback(message);
			}

		}

		#endregion

		[ClassInitialize]
		public static void RegOutputTypes(TestContext testContext)
		{
			OutputTransmitter.RegisterTypes(typeof(RuntimeUnitTest).Assembly);
		}

		[TestMethod]
		[TestCategory("Runtime_OLD")]
		public void SetRTTrackActiveClip()
		{
			var doc = new Document();
			var output = new Output(Guid.NewGuid()) { OutputType = "test.callback" };
			doc.AddOutput(output);
			var track = new Track(Guid.NewGuid()) { TargetKey = "out1", OutputId = output.Id };
			doc.AddTrack(track);
			var clipA = new StepClip(Guid.NewGuid())
						{
							Steps = new ObservableCollection<float> { 0.0f, 1.0f, 2.0f, 3.0f }
						};
			track.Clips.Add(clipA);


			var transport = new RuntimeTransport();
			var rtdoc = new RuntimeDocument(transport, doc);
			var rtoutput = rtdoc.GetOutput(output.Id);
			Assert.IsInstanceOfType(rtoutput.Transmitter, typeof(CallbackTransmitter));

			var msgCount = 0;
			((CallbackTransmitter)rtoutput.Transmitter).PostMessageCallback = msg =>
																				{
																					msgCount++;
																					return true;
																				};

			rtdoc.PostTrackMessages();
			Assert.AreEqual(0, msgCount);

			var rttrack = rtdoc.GetTrack(track.Id);
			Assert.AreSame(track, rttrack.Track);

			rttrack.ActiveClipId = clipA.Id;
			Assert.IsNotNull(rttrack.ActiveClip);
			Assert.AreSame(clipA, rttrack.ActiveClip.Clip);
		}

		[TestMethod]
		[TestCategory("Runtime_OLD")]
		public void PostTrackOutput()
		{
			var doc = new Document();
			var output = new Output(Guid.NewGuid()) { OutputType = "test.callback" };
			doc.AddOutput(output);
			var track = new Track(Guid.NewGuid()) { TargetKey = "out1", OutputId = output.Id };
			doc.AddTrack(track);
			var clipA = new StepClip(Guid.NewGuid())
						{
							Steps = new ObservableCollection<float> { 0.0f, 1.0f, 2.0f, 3.0f },
							TargetKey = ".subA"
						};
			track.Clips.Add(clipA);

			var transport = new RuntimeTransport();
			var rtdoc = new RuntimeDocument(transport, doc);
			var rtoutput = rtdoc.GetOutput(output.Id);
			Assert.IsInstanceOfType(rtoutput.Transmitter, typeof(CallbackTransmitter));

			var msgCount = 0;
			((CallbackTransmitter)rtoutput.Transmitter).PostMessageCallback = msg =>
			{
				msgCount++;
				return true;
			};

			rtdoc.PostTrackMessages();
			Assert.AreEqual(0, msgCount);

			var rttrack = rtdoc.GetTrack(track.Id);
			Assert.AreSame(track, rttrack.Track);

			rttrack.ActiveClipId = clipA.Id;
			Assert.IsNotNull(rttrack.ActiveClip);
			Assert.AreSame(clipA, rttrack.ActiveClip.Clip);

			rtdoc.PostTrackMessages();
			Assert.AreEqual(1, msgCount);
		}

		#region DummyTransport

		internal sealed class DummyTransport : ITransport
		{

			#region ITransport Members

			public float BeatsPerMinute
			{
				get { return Document.DefaultBeatsPerMinute; }
			}

			public bool IsPlaying
			{
				get { return false; }
			}

			public Time Position
			{
				get { return 0; }
			}

			public void Play()
			{
			}

			public void Stop()
			{
			}

			public void Pause()
			{
			}

			#endregion

		}

		#endregion

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextGetClip()
		{
			var doc = new Document();
			var track = new Track();
			doc.AddTrack(track);
			var clipA = new Clip();
			var clipB = new Clip();
			track.Clips.Add(clipA);
			track.Clips.Add(clipB);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);

			Assert.IsNull(rctx.GetClip(Guid.NewGuid()));

			Assert.AreSame(clipA, rctx.GetClip(clipA.Id));
			Assert.AreSame(clipB, rctx.GetClip(clipB.Id));

			var clipC = new Clip();
			Assert.IsNull(rctx.GetClip(clipC.Id));
			track.Clips.Add(clipC);
			Assert.AreSame(clipC, rctx.GetClip(clipC.Id));
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextGetClipState()
		{
			var doc = new Document();
			var track = new Track();
			doc.AddTrack(track);
			var clipA = new Clip();
			var clipB = new Clip();
			track.Clips.Add(clipA);
			track.Clips.Add(clipB);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);

			Assert.IsNull(rctx.GetClipState(Guid.NewGuid()));

			var stateA = rctx.GetClipState(clipA.Id);
			Assert.IsNotNull(stateA);
			Assert.AreSame(clipA, stateA.Clip);

			var stateB = rctx.GetClipState(clipB.Id);
			Assert.IsNotNull(stateB);
			Assert.AreSame(clipB, stateB.Clip);

			var clipC = new Clip();
			var stateC = rctx.GetClipState(clipC.Id);
			Assert.IsNull(stateC);
			track.Clips.Add(clipC);
			stateC = rctx.GetClipState(clipC.Id);
			Assert.IsNotNull(stateC);
			Assert.AreSame(clipC, stateC.Clip);
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextGetOutput()
		{
			var doc = new Document();
			var outputA = new Output();
			doc.AddOutput(outputA);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);
			Assert.AreSame(outputA, rctx.GetOutput(outputA.Id));

			var outputB = new Output();
			Assert.IsNull(rctx.GetOutput(outputB.Id));
			doc.AddOutput(outputB);
			Assert.AreSame(outputB, rctx.GetOutput(outputB.Id));
		}

		#region ModelTrackerTransmitter

		internal sealed class ModelTrackerTransmitter : OutputTransmitter
		{

			public Output OutputModel;

			public override void Initialize(Output outputModel)
			{
				this.OutputModel = outputModel;
			}

			protected override bool PostMessageInternal(OutputMessage message)
			{
				return false;
			}

		}

		#endregion

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextGetTransmitter()
		{
			var doc = new Document();
			var outputA = new Output
						  {
							  OutputType = "test.mtracker"
						  };
			doc.AddOutput(outputA);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);

			Assert.IsNull(rctx.GetTransmitter(Guid.NewGuid()));

			var transmitterA = rctx.GetTransmitter(outputA.Id);
			Assert.IsNotNull(transmitterA);
			Assert.IsInstanceOfType(transmitterA, typeof(ModelTrackerTransmitter));
			Assert.AreSame(outputA, ((ModelTrackerTransmitter)transmitterA).OutputModel);
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextActiveClips()
		{
			var doc = new Document();
			var track = new Track();
			doc.AddTrack(track);
			var clipA = new Clip();
			var clipB = new Clip();
			track.Clips.Add(clipA);
			track.Clips.Add(clipB);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);

			Assert.IsFalse(rctx.ActiveClips.Any());

			var stateA = rctx.GetClipState(clipA.Id);
			Assert.IsNotNull(stateA);
			Assert.AreEqual(0, rctx.ActiveClips.Count());
			stateA.Start();
			CollectionAssert.Contains(rctx.ActiveClips.ToList(), stateA);
			stateA.Stop();
			Assert.AreEqual(0, rctx.ActiveClips.Count());
			stateA.Start();
			CollectionAssert.Contains(rctx.ActiveClips.ToList(), stateA);

			var stateB = rctx.GetClipState(clipB.Id);
			Assert.IsNotNull(stateB);
			Assert.AreEqual(1, rctx.ActiveClips.Count());
			stateB.Start();
			CollectionAssert.AreEqual(new[] { stateA, stateB }, rctx.ActiveClips.ToList());
		}

		#region CollectorTransmitter

		internal class CollectorTransmitter : OutputTransmitter
		{

			#region Static/Constant

			#endregion

			#region Fields

			public readonly List<OutputMessage> Messages = new List<OutputMessage>();

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			protected override bool PostMessageInternal(OutputMessage message)
			{
				if(message != null)
					this.Messages.Add(message);
				return true;
			}

			#endregion

		}

		#endregion

	}

	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
