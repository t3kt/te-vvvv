using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.AppCore;
using Animator.Core.Model;
using Animator.Core.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.AniApp
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression
	// ReSharper disable InvokeAsExtensionMethod
	// ReSharper disable ClassNeverInstantiated.Local
	// ReSharper disable ClassCanBeSealed.Local
	// ReSharper disable FieldCanBeMadeReadOnly.Local
	// ReSharper disable JoinDeclarationAndInitializer

	[TestClass]
	public class ClipDataEditorsUnitTest
	{

		#region DummyClip

		[ClipDataEditor(typeof(DummyClipDataEditor))]
		internal class DummyClipReusable : Clip
		{
		}

		[ClipDataEditor(typeof(DummyClipDataEditor), IsReusable = false)]
		internal class DummyClipNotReusable : Clip
		{
		}

		#endregion

		#region DummyClipDataEditor

		[Export(typeof(IClipDataEditor))]
		internal class DummyClipDataEditor : IClipDataEditor
		{

			public Clip Clip { get; set; }

		}

		#endregion

		[TestMethod]
		[TestCategory("ClipDataEditors")]
		public void AppGetEditorTypeTest()
		{
			Assert.IsNull(ClipDataEditors.GetEditorType(null));
			Assert.IsNull(ClipDataEditors.GetEditorType(typeof(Clip)));

			bool stepEditorIsReusable;
			Type stepEditorType;
			stepEditorType = ClipDataEditors.GetEditorType(typeof(StepClip), out stepEditorIsReusable);
			Assert.AreEqual(typeof(UI.Editors.StepListEditor), stepEditorType);
			Assert.IsTrue(stepEditorIsReusable);
			stepEditorType = ClipDataEditors.GetEditorType(typeof(StepClip), out stepEditorIsReusable);
			Assert.AreEqual(typeof(UI.Editors.StepListEditor), stepEditorType);
			Assert.IsTrue(stepEditorIsReusable);

			bool dummy1Reusable;
			Type dummy1Type = ClipDataEditors.GetEditorType(typeof(DummyClipReusable), out dummy1Reusable);
			Assert.AreEqual(typeof(DummyClipDataEditor), dummy1Type);
			Assert.IsTrue(dummy1Reusable);

			bool dummy2Reusable;
			Type dummy2Type = ClipDataEditors.GetEditorType(typeof(DummyClipNotReusable), out dummy2Reusable);
			Assert.AreEqual(typeof(DummyClipDataEditor), dummy2Type);
			Assert.IsFalse(dummy2Reusable);
		}

		[TestMethod]
		[TestCategory("ClipDataEditors")]
		public void AppGetEditorTest()
		{
			Assert.IsNull(ClipDataEditors.GetEditor(null));
			Assert.IsNull(ClipDataEditors.GetEditor(typeof(Clip)));

			var stepEditor = ClipDataEditors.GetEditor(typeof(StepClip));
			Assert.IsNotNull(stepEditor);
			Assert.IsInstanceOfType(stepEditor, typeof(UI.Editors.StepListEditor));
			Assert.AreSame(stepEditor, ClipDataEditors.GetEditor(typeof(StepClip)));

			var dummyEditor1a = ClipDataEditors.GetEditor(typeof(DummyClipReusable));
			Assert.IsInstanceOfType(dummyEditor1a, typeof(DummyClipDataEditor));
			var dummyEditor1b = ClipDataEditors.GetEditor(typeof(DummyClipReusable));
			Assert.IsInstanceOfType(dummyEditor1b, typeof(DummyClipDataEditor));
			Assert.AreSame(dummyEditor1a, dummyEditor1b);

			var dummyEditor2a = ClipDataEditors.GetEditor(typeof(DummyClipNotReusable));
			Assert.IsInstanceOfType(dummyEditor2a, typeof(DummyClipDataEditor));
			var dummyEditor2b = ClipDataEditors.GetEditor(typeof(DummyClipNotReusable));
			Assert.IsInstanceOfType(dummyEditor2b, typeof(DummyClipDataEditor));
			Assert.AreNotSame(dummyEditor2a, dummyEditor2b);
		}

	}

	// ReSharper restore JoinDeclarationAndInitializer
	// ReSharper restore FieldCanBeMadeReadOnly.Local
	// ReSharper restore ClassCanBeSealed.Local
	// ReSharper restore ClassNeverInstantiated.Local
	// ReSharper restore InvokeAsExtensionMethod
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
