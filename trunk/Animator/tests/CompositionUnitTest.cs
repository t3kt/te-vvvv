using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model.Clips;
using Animator.Core.Runtime;
using Animator.Core.Transport;
using Animator.Osc;
using Animator.Tests.Utils;
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
				host.LoadAssembly(typeof(OscOutput).Assembly);
			if(app)
				host.LoadAssembly(typeof(AniApplication).Assembly);
			return host;
		}

		[TestMethod]
		[TestCategory(CategoryNames.Composition)]
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
		[TestCategory(CategoryNames.Composition)]
		public void LoadTestAssembly()
		{
			CreateHost(test: true, core: false);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Composition)]
		public void LoadCoreAssembly()
		{
			CreateHost(test: false, core: true);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Composition)]
		public void ComposeAttributedDummyContainer()
		{
			var host = CreateHost(test: true);
			var container = new AttributedDummyContainer();
			host.Container.ComposeParts(container);
			Assert.IsNotNull(container.DummyThing);
			Assert.IsInstanceOfType(container.DummyThing, typeof(DummyImplementation));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Composition)]
		public void GetClipPropertyDataByKey()
		{
			var host = CreateHost(test: false, core: true);
			Assert.IsNull(host.CreateClipPropertyDataByKey(null));
			Assert.IsNull(host.CreateClipPropertyDataByKey(String.Empty));
			Assert.IsNull(host.CreateClipPropertyDataByKey("foo"));
			Assert.IsInstanceOfType(host.CreateClipPropertyDataByKey(ConstData.Export_Key), typeof(ConstData));
			Assert.IsInstanceOfType(host.CreateClipPropertyDataByKey(StepData.Export_Key), typeof(StepData));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Composition)]
		public void GetClipPropertyDataByElementName()
		{
			var host = CreateHost(test: false, core: true);
			Assert.IsNull(host.CreateClipPropertyDataByElementName(null));
			Assert.IsNull(host.CreateClipPropertyDataByElementName(String.Empty));
			Assert.IsNull(host.CreateClipPropertyDataByElementName("foo"));
			Assert.IsInstanceOfType(host.CreateClipPropertyDataByElementName(ConstData.Export_ElementName), typeof(ConstData));
			Assert.IsInstanceOfType(host.CreateClipPropertyDataByElementName(StepData.Export_ElementName), typeof(StepData));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Composition)]
		public void GetTransportByKey()
		{
			var host = CreateHost(test: true, core: true);
			Assert.IsInstanceOfType(host.CreateTransportByKey(NullTransport.Export_Key), typeof(NullTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey(null), typeof(NullTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey(String.Empty), typeof(NullTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey("manual"), typeof(ManualTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey("media"), typeof(MediaTransport));
			Assert.IsInstanceOfType(host.CreateTransportByKey("dummy"), typeof(DummyTransport));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Composition)]
		public void GetOutputByKey()
		{
			var host = CreateHost(test: true, core: true, osc: true);
			Assert.IsInstanceOfType(host.CreateOutputByKey(null), typeof(Output));
			Assert.IsInstanceOfType(host.CreateOutputByKey(String.Empty), typeof(Output));
			Assert.IsInstanceOfType(host.CreateOutputByKey(Output.Export_Key), typeof(Output));
			Assert.IsInstanceOfType(host.CreateOutputByKey(IOUnitTest.TestOutput.Export_Key), typeof(IOUnitTest.TestOutput));
			Assert.IsInstanceOfType(host.CreateOutputByKey(CallbackOutput.Export_Key), typeof(CallbackOutput));
			Assert.IsInstanceOfType(host.CreateOutputByKey(TraceOutput.Export_Key), typeof(TraceOutput));
			Assert.IsInstanceOfType(host.CreateOutputByKey(OscOutput.Export_Key), typeof(OscOutput));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Composition)]
		public void GetObjectEditor()
		{
			var host = CreateHost(test: true, core: true, app: true);
			Assert.IsNull(host.CreateObjectEditorByKey("foo"));
			Assert.IsNull(host.CreateObjectEditorByKey(null));
			Assert.IsInstanceOfType(host.CreateObjectEditorByKey(ThingEditor.Export_Key), typeof(ThingEditor));
			Assert.IsNull(host.CreateObjectEditor(typeof(string)));
			Assert.IsInstanceOfType(host.CreateObjectEditor(typeof(Thing)), typeof(ThingEditor));
		}

		#region Thing

		internal class Thing
		{

			public string Str { get; set; }

		}

		#endregion

		#region ThingyEditor

		[ObjectEditor(Key = Export_Key, TargetType = typeof(Thing))]
		internal class ThingEditor : IObjectEditor
		{

			#region Static/Constant

			internal const string Export_Key = "thing";
			internal static readonly Type Export_TargetType = typeof(Thing);

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			#endregion

			#region IObjectEditor Members

			public object Target { get; set; }

			public bool AutoCommit { get; set; }

			public bool Dirty { get; set; }

			public System.Windows.Visibility BasicsVisibility { get; set; }

			public System.Windows.Visibility DetailsVisibility { get; set; }

#pragma warning disable 67
			public event TargetPropertyChangedEventHandler TargetPropertyChanged;
#pragma warning restore 67

			public void Commit()
			{
			}

			public void Reset()
			{
			}

			#endregion

		}

		#endregion

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
