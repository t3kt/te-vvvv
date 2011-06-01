using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.AppCore;
using Animator.Core.Composition;
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
	// ReSharper disable RedundantArgumentName

	[TestClass]
	public class ClipDataEditorsUnitTest
	{

		#region DummyClip

		internal class DummyClipReusable : Clip
		{
		}

		internal class DummyClipNotReusable : Clip
		{
		}

		#endregion

		#region DummyClipDataEditor

		[ClipDataEditorType(ClipType = typeof(DummyClipNotReusable))]
		internal class DummyClipDataEditorNotReusable : IClipDataEditor
		{

			public Clip Clip { get; set; }

		}

		[ClipDataEditorType(ClipType = typeof(DummyClipReusable), Reusable = true)]
		internal class DummyClipDataEditorReusable : IClipDataEditor
		{

			public Clip Clip { get; set; }

		}

		#endregion

		[TestMethod]
		[TestCategory(CategoryNames.ClipDataEditors)]
		public void AppGetEditorTest()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true, app: true);
			Assert.IsNull(host.CreateClipDataEditor(null));
			Assert.IsNull(host.CreateClipDataEditor(typeof(Clip)));

			bool stepEditorIsReusable;
			var stepEditor = host.CreateClipDataEditor(typeof(StepClip), out stepEditorIsReusable);
			Assert.IsNotNull(stepEditor);
			Assert.IsInstanceOfType(stepEditor, typeof(UI.Editors.StepListEditor));
			Assert.IsTrue(stepEditorIsReusable);

			bool dummy1Reusable;
			var dummy1Editor = host.CreateClipDataEditor(typeof(DummyClipReusable), out dummy1Reusable);
			Assert.IsInstanceOfType(dummy1Editor, typeof(DummyClipDataEditorReusable));
			Assert.IsTrue(dummy1Reusable);

			bool dummy2Reusable;
			var dummy2Editor = host.CreateClipDataEditor(typeof(DummyClipNotReusable), out dummy2Reusable);
			Assert.IsInstanceOfType(dummy2Editor, typeof(DummyClipDataEditorNotReusable));
			Assert.IsFalse(dummy2Reusable);
		}

	}

	// ReSharper restore RedundantArgumentName
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
