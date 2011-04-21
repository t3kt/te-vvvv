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

		#region DummyTransport

		internal sealed class DummyTransport : ITransport
		{

			public float BeatsPerMinute { get; set; }

			public bool IsPlaying { get; set; }

			public Time Position { get; set; }

			public void Play()
			{
				this.IsPlaying = true;
			}

			public void Stop()
			{
				this.IsPlaying = false;
				this.Position = 0;
			}

			public void Pause()
			{
				this.IsPlaying = false;
			}

			public DummyTransport()
			{
				this.BeatsPerMinute = Document.DefaultBeatsPerMinute;
			}

		}

		#endregion

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextGetClip()
		{
			var doc = new Document();
			var clipA = new Clip();
			var clipB = new Clip();
			doc.Clips.Add(clipA);
			doc.Clips.Add(clipB);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);

			Assert.IsNull(rctx.GetClip(Guid.NewGuid()));

			Assert.AreSame(clipA, rctx.GetClip(clipA.Id));
			Assert.AreSame(clipB, rctx.GetClip(clipB.Id));

			var clipC = new Clip();
			Assert.IsNull(rctx.GetClip(clipC.Id));
			doc.Clips.Add(clipC);
			Assert.AreSame(clipC, rctx.GetClip(clipC.Id));
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextGetClipState()
		{
			var doc = new Document();
			var clipA = new Clip();
			var clipB = new Clip();
			doc.Clips.Add(clipA);
			doc.Clips.Add(clipB);

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
			doc.Clips.Add(clipC);
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
			doc.Outputs.Add(outputA);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);
			Assert.AreSame(outputA, rctx.GetOutput(outputA.Id));

			var outputB = new Output();
			Assert.IsNull(rctx.GetOutput(outputB.Id));
			doc.Outputs.Add(outputB);
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
			doc.Outputs.Add(outputA);

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
			var clipA = new Clip();
			var clipB = new Clip();
			doc.Clips.Add(clipA);
			doc.Clips.Add(clipB);

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

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextGetStepClipValue()
		{
			var doc = new Document();
			var output = new Output
							{
								Name = "collector_output",
								OutputType = "test.collector"
							};
			doc.Outputs.Add(output);
			var clip = new StepClip
						{
							TargetKey = "myoutput",
							Duration = 4,
							OutputId = output.Id,
							Steps = new ObservableCollection<float> { 1f, 2f, 3f, 4f }
						};
			doc.Clips.Add(clip);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);

			var clipState = rctx.GetClipState(clip.Id);
			Assert.IsNotNull(clipState);

			Assert.IsNull(clipState.GetValue());
			clipState.Start();
			Assert.AreEqual(1f, clipState.GetValue());
			transport.IsPlaying = true;
			transport.Position = 1;
			Assert.AreEqual(2f, clipState.GetValue());
			clipState.Stop();
			Assert.IsNull(clipState.GetValue());
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTContextPostStepClipValue()
		{
			var doc = new Document();
			var output = new Output
			{
				Name = "collector_output",
				OutputType = "test.collector"
			};
			doc.Outputs.Add(output);
			var clip = new StepClip
			{
				TargetKey = "myoutput",
				Duration = 4,
				OutputId = output.Id,
				Steps = new ObservableCollection<float> { 1f, 2f, 3f, 4f }
			};
			doc.Clips.Add(clip);

			var transport = new DummyTransport();
			var rctx = new DocumentRuntimeContext(doc, transport);
			var transmitter = rctx.GetTransmitter(output.Id);
			Assert.IsInstanceOfType(transmitter, typeof(CollectorTransmitter));
			var collector = (CollectorTransmitter)transmitter;
			Assert.IsNotNull(collector);

			var clipState = rctx.GetClipState(clip.Id);
			Assert.IsNotNull(clipState);

			rctx.PostActiveClipOutputs();
			Assert.AreEqual(0, collector.Messages.Count);
			rctx.PostClipOutput(clipState);
			Assert.AreEqual(0, collector.Messages.Count);

			transport.Play();
			rctx.PostActiveClipOutputs();
			Assert.AreEqual(0, collector.Messages.Count);

			clipState.Start();
			rctx.PostActiveClipOutputs();
			Assert.AreEqual(1, collector.Messages.Count);
			CollectionAssert.AreEqual(new[] {new OutputMessage(clip.TargetKey, 1f)}, collector.Messages, IOUnitTest.OutputMessageComparer.Instance);
			collector.Messages.Clear();

			transport.Position = 3;

			rctx.PostActiveClipOutputs();
			Assert.AreEqual(1, collector.Messages.Count);
			CollectionAssert.AreEqual(new[] { new OutputMessage(clip.TargetKey, 4f) }, collector.Messages, IOUnitTest.OutputMessageComparer.Instance);
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
