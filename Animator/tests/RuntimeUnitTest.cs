using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Common;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Model.Sequences;
using Animator.Core.Model.Sessions;
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

	[TestClass]
	public class RuntimeUnitTest
	{

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		public void RTDocumentActiveClips()
		{
			Assert.Inconclusive();
			//var doc = new Document();
			//var clipA = new Clip();
			//var clipB = new Clip();
			//doc.Clips.Add(clipA);
			//doc.Clips.Add(clipB);

			//var transport = new DummyTransport();

			//Assert.IsFalse(doc.ActiveClips.Any());

			//Assert.AreSame(clipA, doc.GetClip(clipA.Id));
			//Assert.AreEqual(0, doc.ActiveClips.Count());
			//clipA.Start(transport);
			//CollectionAssert.Contains(doc.ActiveClips.ToList(), clipA);
			//clipA.Stop();
			//Assert.AreEqual(0, doc.ActiveClips.Count());
			//clipA.Start(transport);
			//CollectionAssert.Contains(doc.ActiveClips.ToList(), clipA);

			//Assert.AreSame(clipB, doc.GetClip(clipB.Id));
			//Assert.AreEqual(1, doc.ActiveClips.Count());
			//clipB.Start(transport);
			//CollectionAssert.AreEqual(new[] { clipA, clipB }, doc.ActiveClips.ToList());
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		public void RTDocumentPostStepClipValue()
		{
			Assert.Inconclusive();
			//var host = CompositionUnitTest.CreateHost(test: true, core: true);
			//var doc = new Document(host);
			//var output = host.CreateOutputByKey("test.collector");
			//output.Name = "collector_output";
			//doc.Outputs.Add(output);
			//var clip = new StepClip
			//{
			//    TargetKey = "myoutput",
			//    Duration = 4,
			//    OutputId = output.Id
			//};
			//clip.Steps.ReplaceContents(new[] { 1f, 2f, 3f, 4f });
			//doc.Clips.Add(clip);

			//var transport = new DummyTransport();
			//Assert.IsInstanceOfType(output, typeof(CollectorOutput));
			//var collector = (CollectorOutput)output;

			//Assert.AreSame(clip, doc.GetClip(clip.Id));

			//doc.PostActiveClipOutputs(transport);
			//Assert.AreEqual(0, collector.Messages.Count);
			//doc.PostClipOutput(clip, transport);
			//Assert.AreEqual(0, collector.Messages.Count);

			//transport.Play();
			//doc.PostActiveClipOutputs(transport);
			//Assert.AreEqual(0, collector.Messages.Count);

			//clip.Start(transport);
			//doc.PostActiveClipOutputs(transport);
			//Assert.AreEqual(1, collector.Messages.Count);
			//CollectionAssert.AreEqual(new[] { new OutputMessage(clip.TargetKey, 1f) }, collector.Messages, OutputMessageComparer.Instance);
			//collector.Messages.Clear();

			//transport.Position = 3;

			//doc.PostActiveClipOutputs(transport);
			//Assert.AreEqual(1, collector.Messages.Count);
			//CollectionAssert.AreEqual(new[] { new OutputMessage(clip.TargetKey, 4f) }, collector.Messages, OutputMessageComparer.Instance);
		}

		#region Thing

		internal class Thing
		{

			public string Str { get; set; }

		}

		#endregion

		#region ThingyEditor

		[PropertyEditor(Key = "thing", TargetType = typeof(Thing))]
		internal class ThingEditor : IPropertyEditor
		{

			#region Static/Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			#endregion

			#region IPropertyEditor Members

			public object Target { get; set; }

			public bool AutoCommit { get; set; }

			public bool Dirty { get; set; }

			public System.Windows.Visibility BasicsVisibility { get; set; }

			public System.Windows.Visibility DetailsVisibility { get; set; }

			public event TargetPropertyChangedEventHandler TargetPropertyChanged;

			public void Commit()
			{
			}

			public void Reset()
			{
			}

			#endregion

		}

		#endregion

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		public void TargetObjectGetSetClear()
		{
			var tgtObj = new TargetObject();
			object value;
			const string propA_Name = "propA";
			Assert.IsFalse(tgtObj.GetValue(propA_Name, out value));
			Assert.IsFalse(tgtObj.SetValue(propA_Name, 22.1f));
			Assert.IsFalse(tgtObj.ClearValue(propA_Name));

			var propA = tgtObj.Add(propA_Name, TargetPropertyType.Value, 12.3f);
			Assert.IsTrue(tgtObj.GetValue(propA_Name, out value));
			Assert.AreEqual(propA.DefaultValue, value);
			Assert.IsTrue(tgtObj.ClearValue(propA_Name));
			Assert.IsTrue(tgtObj.GetValue(propA_Name, out value));
			Assert.AreEqual(propA.DefaultValue, value);
			Assert.IsTrue(tgtObj.SetValue(propA_Name, 22.1f));
			Assert.IsTrue(tgtObj.GetValue(propA_Name, out value));
			Assert.AreEqual(22.1f, value);
			Assert.IsTrue(tgtObj.ClearValue(propA_Name));
			Assert.IsTrue(tgtObj.GetValue(propA_Name, out value));
			Assert.AreEqual(propA.DefaultValue, value);

			Assert.IsTrue(tgtObj.Remove(propA_Name));
			Assert.IsFalse(tgtObj.GetValue(propA_Name, out value));
			Assert.IsFalse(tgtObj.SetValue(propA_Name, 22.1f));
			Assert.IsFalse(tgtObj.ClearValue(propA_Name));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		public void ActiveSectionMustBeInDocumentValid()
		{
			var doc = new Document();
			var ses = new Session(doc);
			doc.Sessions.Add(ses);
			doc.ActiveSection = ses;
			var seq = new Sequence(doc);
			doc.Sequences.Add(seq);
			doc.ActiveSection = seq;
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		[ExpectedException(typeof(ModelException))]
		public void ActiveSessionMustBeInDocumentInvalid()
		{
			var doc = new Document();
			var ses = new Session(doc);
			doc.ActiveSection = ses;
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		[ExpectedException(typeof(ModelException))]
		public void ActiveSequenceMustBeInDocumentInvalid()
		{
			var doc = new Document();
			var seq = new Sequence(doc);
			doc.ActiveSection = seq;
		}

	}

	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
