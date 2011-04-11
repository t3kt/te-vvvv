using System;
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

namespace Animator.Tests
{
	[TestClass]
	public class RuntimeUnitTest
	{

		#region CallbackTransmitter

		internal class CallbackTransmitter : OutputTransmitter
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
			OutputTransmitter.RegisterTypes(typeof (RuntimeUnitTest).Assembly);
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void BuildRTDocWithTransmitter()
		{
			var doc = new Document();
			var output = doc.CreateOutput(Guid.NewGuid());
			output.OutputType = "test.callback";
			doc.AddOutput(output);
			var track = doc.CreateTrack(Guid.NewGuid());
			track.TargetKey = "out1";
			track.OutputId = output.Id;
			doc.AddTrack(track);
			var clipA = doc.CreateStepClip(track, Guid.NewGuid());
			clipA.SetSteps(0.0f, 1.0f, 2.0f, 3.0f);


			var transport = new RuntimeTransport();
			var rtdoc = new RuntimeDocument(transport, doc);
			var rtoutput = rtdoc.GetOutput(output.Id);
			Assert.IsInstanceOfType(rtoutput.Transmitter, typeof(CallbackTransmitter));
		}

		[TestMethod]
		[TestCategory("Runtime")]
		public void SetRTTrackActiveClip()
		{
			var doc = new Document();
			var output = doc.CreateOutput(Guid.NewGuid());
			output.OutputType = "test.callback";
			doc.AddOutput(output);
			var track = doc.CreateTrack(Guid.NewGuid());
			track.TargetKey = "out1";
			track.OutputId = output.Id;
			doc.AddTrack(track);
			var clipA = doc.CreateStepClip(track, Guid.NewGuid());
			clipA.SetSteps(0.0f, 1.0f, 2.0f, 3.0f);
			track.AddClip(clipA);


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
		[TestCategory("Runtime")]
		public void PostTrackOutput()
		{
			var doc = new Document();
			var output = doc.CreateOutput(Guid.NewGuid());
			output.OutputType = "test.callback";
			doc.AddOutput(output);
			var track = doc.CreateTrack(Guid.NewGuid());
			track.TargetKey = "out1";
			track.OutputId = output.Id;
			doc.AddTrack(track);
			var clipA = doc.CreateStepClip(track, Guid.NewGuid());
			clipA.SetSteps(0.0f, 1.0f, 2.0f, 3.0f);
			clipA.TargetKey = ".subA";
			track.AddClip(clipA);


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

	}
}
