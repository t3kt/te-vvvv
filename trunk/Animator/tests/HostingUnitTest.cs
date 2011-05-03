using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Composition;
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
	public class HostingUnitTest
	{

		private static AniHost CreateHost(bool test = true, bool core = false)
		{
			var host = new AniHost();
			if(test)
				host.LoadAssembly(typeof(HostingUnitTest).Assembly);
			if(core)
				host.LoadCoreAssembly();
			return host;
		}

		[TestMethod]
		[TestCategory("Hosting")]
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
		[TestCategory("Hosting")]
		public void LoadTestAssembly()
		{
			CreateHost(test: true, core: false);
		}

		[TestMethod]
		[TestCategory("Hosting")]
		public void LoadCoreAssembly()
		{
			CreateHost(test: false, core: true);
		}

		[TestMethod]
		[TestCategory("Hosting")]
		public void ComposeAttributedDummyContainer()
		{
			var host = CreateHost(test: true);
			var container = new AttributedDummyContainer();
			host.Container.ComposeParts(container);
			Assert.IsNotNull(container.DummyThing);
			Assert.IsInstanceOfType(container.DummyThing, typeof(DummyImplementation));
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
