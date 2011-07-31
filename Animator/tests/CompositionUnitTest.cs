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
			var const_data_a= host.CreateClipPropertyDataByKey(ConstData.Export_Key);
			Assert.IsInstanceOfType(const_data_a, typeof(ConstData));
			var const_data_b = host.CreateClipPropertyDataByKey(ConstData.Export_Key);
			Assert.IsInstanceOfType(const_data_b, typeof(ConstData));
			Assert.AreNotSame(const_data_a, const_data_b);
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
			var const_data_a = host.CreateClipPropertyDataByElementName(ConstData.Export_ElementName);
			Assert.IsInstanceOfType(const_data_a, typeof(ConstData));
			var const_data_b = host.CreateClipPropertyDataByElementName(ConstData.Export_ElementName);
			Assert.IsInstanceOfType(const_data_b, typeof(ConstData));
			Assert.AreNotSame(const_data_a, const_data_b);
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
		public void GetOutputByElementName()
		{
			var host = CreateHost(test: true, core: true, osc: true);
			var output_a = host.CreateOutputByElementName(null);
			Assert.IsInstanceOfType(output_a, typeof(Output));
			var output_b = host.CreateOutputByElementName(null);
			Assert.IsInstanceOfType(output_a, typeof(Output));
			Assert.AreNotSame(output_a, output_b);
			var output_c = host.CreateOutputByElementName(null);
			Assert.IsInstanceOfType(output_c, typeof(Output));
			Assert.AreNotSame(output_a, output_c);

			var osc_output_a = host.CreateOutputByElementName(OscOutput.Export_ElementName);
			Assert.IsInstanceOfType(osc_output_a, typeof(OscOutput));

			//var output_d = host.CreateOutputByElementName(null);
			//Assert.IsInstanceOfType(output_d, typeof(Output));
			//Assert.AreNotSame(output_a, output_d);

			var osc_output_b = host.CreateOutputByElementName(OscOutput.Export_ElementName);
			Assert.IsInstanceOfType(osc_output_b, typeof(OscOutput));
			Assert.AreNotSame(osc_output_a, osc_output_b);
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
