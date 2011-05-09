using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Transport;
using Animator.Osc;
using Animator.Tests.Utils;
using Animator.UI.Editors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
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
	// ReSharper disable RedundantArgumentDefaultValue
	// ReSharper disable RedundantArgumentName
#pragma warning disable 168
	[TestClass]
	public class CompositionUnitTest
	{

		internal static AniHost CreateHost(bool test = true, bool core = false, bool osc = false, bool app = false)
		{
			var host = new AniHost();
			if(test)
				host.LoadAssembly(typeof(CompositionUnitTest).Assembly);
			if(core)
				host.LoadCoreAssembly();
			if(osc)
				host.LoadAssembly(typeof(OscTransmitter).Assembly);
			if(app)
				host.LoadAssembly(typeof(AniApplication).Assembly);
			return host;
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void LoadCoreAssemblyBasic()
		{
			var host = new AniHost();
			host.LoadCoreAssembly();
		}

		#region IDummyContract

		internal interface IDummyContract
		{

			string Name { get; }

		}

		#endregion

		#region DummyImplementation

		[Export(typeof(IDummyContract))]
		internal class DummyImplementation : IDummyContract
		{

			#region Static/Constant

			public const string Name = "Dummy";

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			#endregion

			#region IDummyContract Members

			string IDummyContract.Name
			{
				get { return Name; }
			}

			#endregion
		}

		#endregion

		#region AttributedDummyContainer

		internal class AttributedDummyContainer
		{

			#region Static/Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			[Import]
			public IDummyContract DummyThing { get; set; }

			#endregion

			#region Constructors

			#endregion

			#region Methods

			#endregion

		}

		#endregion

		[TestMethod]
		[TestCategory("Composition")]
		public void LoadTestAssembly()
		{
			CreateHost(test: true, core: false);
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void LoadCoreAssembly()
		{
			CreateHost(test: false, core: true);
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void ComposeAttributedDummyContainer()
		{
			var host = CreateHost(test: true);
			var container = new AttributedDummyContainer();
			host.Container.ComposeParts(container);
			Assert.IsNotNull(container.DummyThing);
			Assert.IsInstanceOfType(container.DummyThing, typeof(DummyImplementation));
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void LoadClipTypeImports()
		{
			var host = CreateHost(test: false, core: true);
			Assert.IsNotNull(host.Clips);
			Assert.IsTrue(host.Clips.Any());
			Assert.IsTrue(host.Clips.Any(x => x.Value is StepClip));
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void GetClipByElementName()
		{
			var host = CreateHost(test: false, core: true);
			var clipA = host.CreateClipByElementName("clip");
			Assert.IsInstanceOfType(clipA, typeof(Clip));
			var clipB = host.CreateClipByElementName(null);
			Assert.IsInstanceOfType(clipB, typeof(Clip));
			var clipC = host.CreateClipByElementName(String.Empty);
			Assert.IsInstanceOfType(clipC, typeof(Clip));
			var clipD = host.CreateClipByElementName("stepclip");
			Assert.IsInstanceOfType(clipD, typeof(StepClip));
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void GetClipByKey()
		{
			var host = CreateHost(test: false, core: true);
			Assert.IsInstanceOfType(host.CreateClipByKey("clip"), typeof(Clip));
			Assert.IsInstanceOfType(host.CreateClipByKey(null), typeof(Clip));
			Assert.IsInstanceOfType(host.CreateClipByKey(String.Empty), typeof(Clip));
			Assert.IsInstanceOfType(host.CreateClipByKey("stepclip"), typeof(StepClip));
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void GetTransportByKey()
		{
			var host = CreateHost(test: true, core: true);
			Assert.IsInstanceOfType(host.CreateTransportByKey("null"), typeof(Transport.NullTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey(null), typeof(Transport.NullTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey(String.Empty), typeof(Transport.NullTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey("manual"), typeof(ManualTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey("media"), typeof(MediaTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey("dummy"), typeof(DummyTransport));
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void GetOutputTransmitterByKey()
		{
			var host = CreateHost(test: true, core: true, osc: true);
			Assert.IsInstanceOfType(host.CreateTransmitterByKey("null"), typeof(OutputTransmitter.NullTransmitter));
			Assert.IsInstanceOfType(host.CreateTransmitterByKey(null), typeof(OutputTransmitter.NullTransmitter));
			Assert.IsInstanceOfType(host.CreateTransmitterByKey(String.Empty), typeof(OutputTransmitter.NullTransmitter));
			Assert.IsInstanceOfType(host.CreateTransmitterByKey("test"), typeof(IOUnitTest.TestTransmitter));
			Assert.IsInstanceOfType(host.CreateTransmitterByKey("test.callback"), typeof(CallbackTransmitter));
			Assert.IsInstanceOfType(host.CreateTransmitterByKey("trace"), typeof(OutputTransmitter.TraceTransmitter));
			Assert.IsInstanceOfType(host.CreateTransmitterByKey("osc"), typeof(OscTransmitter));
		}

		[TestMethod]
		[TestCategory("Composition")]
		public void GetPropertyEditor()
		{
			var host = CreateHost(test: true, core: true, app: true);
			Assert.IsNull(host.CreatePropertyEditorByKey("foo"));
			Assert.IsNull(host.CreatePropertyEditorByKey(null));
			Assert.IsInstanceOfType(host.CreatePropertyEditorByKey("thing"), typeof(RuntimeUnitTest.ThingEditor));
			Assert.IsNull(host.CreatePropertyEditor(typeof(string)));
			Assert.IsInstanceOfType(host.CreatePropertyEditor(typeof(RuntimeUnitTest.Thing)), typeof(RuntimeUnitTest.ThingEditor));
			Assert.IsInstanceOfType(host.CreatePropertyEditor(typeof(Document), "basic"), typeof(DocumentPropertyEditor));
			var a = host.CreatePropertyEditor(typeof(Document), "basic");
			var b = host.CreatePropertyEditor(typeof(Document), "basic");
			Assert.AreNotSame(a, b);
		}

	}
#pragma warning restore 168
	// ReSharper restore RedundantArgumentName
	// ReSharper restore RedundantArgumentDefaultValue
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
