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

#pragma warning disable 649
			public Func<OutputMessage, bool> PostMessageCallback;
#pragma warning restore 649

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

		}

		#endregion

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
		public void RTDocumentGetTransmitter()
		{
			var doc = new Document();
			var outputA = new Output
						  {
							  OutputType = "test.mtracker"
						  };
			doc.Outputs.Add(outputA);

			Assert.IsNull(doc.GetTransmitter(Guid.NewGuid()));

			var transmitterA = doc.GetTransmitter(outputA.Id);
			Assert.IsNotNull(transmitterA);
			Assert.IsInstanceOfType(transmitterA, typeof(ModelTrackerTransmitter));
			Assert.AreSame(outputA, ((ModelTrackerTransmitter)transmitterA).OutputModel);
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTDocumentActiveClips()
		{
			var doc = new Document();
			var clipA = new Clip();
			var clipB = new Clip();
			doc.Clips.Add(clipA);
			doc.Clips.Add(clipB);

			var transport = new DummyTransport();

			Assert.IsFalse(doc.ActiveClips.Any());

			Assert.AreSame(clipA, doc.GetClip(clipA.Id));
			Assert.AreEqual(0, doc.ActiveClips.Count());
			clipA.Start(transport);
			CollectionAssert.Contains(doc.ActiveClips.ToList(), clipA);
			clipA.Stop();
			Assert.AreEqual(0, doc.ActiveClips.Count());
			clipA.Start(transport);
			CollectionAssert.Contains(doc.ActiveClips.ToList(), clipA);

			Assert.AreSame(clipB, doc.GetClip(clipB.Id));
			Assert.AreEqual(1, doc.ActiveClips.Count());
			clipB.Start(transport);
			CollectionAssert.AreEqual(new[] { clipA, clipB }, doc.ActiveClips.ToList());
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTGetStepClipValue()
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

			Assert.AreSame(clip, doc.GetClip(clip.Id));

			Assert.IsNull(clip.GetValue(transport));
			clip.Start(transport);
			Assert.AreEqual(1f, clip.GetValue(transport));
			transport.IsPlaying = true;
			transport.Position = 1;
			Assert.AreEqual(2f, clip.GetValue(transport));
			clip.Stop();
			Assert.IsNull(clip.GetValue(transport));
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void RTDocumentPostStepClipValue()
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
			var transmitter = doc.GetTransmitter(output.Id);
			Assert.IsInstanceOfType(transmitter, typeof(CollectorTransmitter));
			var collector = (CollectorTransmitter)transmitter;
			Assert.IsNotNull(collector);

			Assert.AreSame(clip, doc.GetClip(clip.Id));

			doc.PostActiveClipOutputs(transport);
			Assert.AreEqual(0, collector.Messages.Count);
			doc.PostClipOutput(clip, transport);
			Assert.AreEqual(0, collector.Messages.Count);

			transport.Play();
			doc.PostActiveClipOutputs(transport);
			Assert.AreEqual(0, collector.Messages.Count);

			clip.Start(transport);
			doc.PostActiveClipOutputs(transport);
			Assert.AreEqual(1, collector.Messages.Count);
			CollectionAssert.AreEqual(new[] { new OutputMessage(clip.TargetKey, 1f) }, collector.Messages, IOUnitTest.OutputMessageComparer.Instance);
			collector.Messages.Clear();

			transport.Position = 3;

			doc.PostActiveClipOutputs(transport);
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
