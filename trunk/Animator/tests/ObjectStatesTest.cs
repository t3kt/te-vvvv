using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Runtime.ObjectStates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
{

	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression
	// ReSharper disable RedundantArgumentName
#pragma warning disable 168

	[TestClass]
	public class ObjectStatesTest
	{

		public class TargetThingy
		{
			private static int _NextId = 0;

			private readonly int _Id;

			public TargetThingy()
			{
				this._Id = ++_NextId;
			}

			[StateProperty]
			public int Id { get { return this._Id; } }

			[StateProperty]
			public string Name { get; set; }

			public bool HiddenStuff { get; set; }

			[StateProperty]
			public int Awesomeness { get; set; }

			[StateProperty]
			[ReadOnly(true)]
			public string PseudoReadOnly { get; set; }

		}

		private static PropertyDescriptorCollection _ThingyProps;
		private static PropertyDescriptorCollection _ThingyPropsStateOnly;
		private static PropertyDescriptor _ThingyProp_Id;
		private static PropertyDescriptor _ThingyProp_Name;
		private static PropertyDescriptor _ThingyProp_HiddenStuff;
		private static PropertyDescriptor _ThingyProp_Awesomeness;
		private static PropertyDescriptor _ThingyProp_PseudoReadOnly;

		[ClassInitialize]
		public static void InitThingyProps(TestContext testContext)
		{
			_ThingyProps = TypeDescriptor.GetProperties(typeof(TargetThingy));
			_ThingyProp_Id = _ThingyProps["Id"];
			_ThingyProp_Name = _ThingyProps["Name"];
			_ThingyProp_HiddenStuff = _ThingyProps["HiddenStuff"];
			_ThingyProp_Awesomeness = _ThingyProps["Awesomeness"];
			_ThingyProp_PseudoReadOnly = _ThingyProps["PseudoReadOnly"];
			Assert.IsNotNull(_ThingyProp_Id);
			CollectionAssert.AllItemsAreNotNull(new[] { _ThingyProp_Id, _ThingyProp_Name, _ThingyProp_HiddenStuff, _ThingyProp_Awesomeness, _ThingyProp_PseudoReadOnly });
			CollectionAssert.AreEquivalent(new[] { _ThingyProp_Id, _ThingyProp_Name, _ThingyProp_HiddenStuff, _ThingyProp_Awesomeness, _ThingyProp_PseudoReadOnly }, _ThingyProps);
			_ThingyPropsStateOnly = TypeDescriptor.GetProperties(typeof(TargetThingy), new[] { StatePropertyAttribute.Default });
			CollectionAssert.AreEquivalent(new[] { _ThingyProp_Id, _ThingyProp_Name, _ThingyProp_Awesomeness, _ThingyProp_PseudoReadOnly }, _ThingyPropsStateOnly);
			Assert.IsTrue(_ThingyProp_Id.IsReadOnly);
			Assert.IsTrue(_ThingyProp_PseudoReadOnly.IsReadOnly);
		}

		[TestCategory(CategoryNames.ObjectStates)]
		[TestMethod]
		public void GetObjectStateTypeDescriptor()
		{
			var target = new TargetThingy();
			var state = ObjectState.CreateState(target);
			var provider = TypeDescriptor.GetProvider(typeof(ObjectState));
			//Assert.IsInstanceOfType(provider, typeof(StateType.Provider));
			var descriptor = provider.GetTypeDescriptor(state);
			//Assert.IsInstanceOfType(descriptor, typeof (StateType));
			//Assert.AreSame(state.Type, descriptor);
			Assert.IsNotNull(descriptor);
			var props = descriptor.GetProperties();
			//Assert.AreSame(state.Type.StateProperties, props);
			CollectionAssert.AreEquivalent(state.Type.StateProperties, props);
			//Assert.Inconclusive();
		}

		[TestCategory(CategoryNames.ObjectStates)]
		[TestMethod]
		public void GetStateTypeProperties()
		{
			var target = new TargetThingy();

			var stateType = new StateType(typeof(TargetThingy), target);
			var stateProps = stateType.StateProperties;

			CollectionAssert.AreEquivalent(_ThingyPropsStateOnly.Cast<PropertyDescriptor>().ToArray(), stateProps.Cast<PropertyDescriptor>().ToArray());
		}

		[TestCategory(CategoryNames.ObjectStates)]
		[TestMethod]
		public void StatePropertyGetSet()
		{
			var origName = "foo";
			var origAwesomeness = 1234;
			var target = new TargetThingy { Name = origName, Awesomeness = origAwesomeness };
			var state = ObjectState.CreateState(target);
			var stateType = state.Type;
			var sprop_name = stateType.GetProperty("Name");
			var oprop_name = sprop_name.OriginalProperty;
			var sprop_awesomeness = stateType.GetProperty("Awesomeness");
			var oprop_awesomenesss = sprop_awesomeness.OriginalProperty;
			Assert.AreEqual(_ThingyProp_Name, oprop_name);
			Assert.AreEqual(origName, oprop_name.GetValue(target));
			Assert.AreEqual(_ThingyProp_Awesomeness, oprop_awesomenesss);
			Assert.AreEqual(origAwesomeness, oprop_awesomenesss.GetValue(target));

			var modName = "xyz";
			var modAwesomeness = -34;
			sprop_name.SetValue(state, modName);
			Assert.AreEqual(modName, state.GetProperty("Name"));
			Assert.AreEqual(modName, sprop_name.GetValue(state));
			sprop_awesomeness.SetValue(state, modAwesomeness);
			Assert.AreEqual(modAwesomeness, state.GetProperty("Awesomeness"));
			Assert.AreEqual(modAwesomeness, sprop_awesomeness.GetValue(state));
		}

		[TestCategory(CategoryNames.ObjectStates)]
		[TestMethod]
		public void TargetValuesCopyToState()
		{
			var origName = "foo";
			var origAwesomeness = 1234;
			var target = new TargetThingy { Name = origName, Awesomeness = origAwesomeness };
			var state = ObjectState.CreateState(target);
			var stateType = state.Type;
			var sprop_name = stateType.GetProperty("Name");
			var oprop_name = sprop_name.OriginalProperty;
			var sprop_awesomeness = stateType.GetProperty("Awesomeness");
			var oprop_awesomenesss = sprop_awesomeness.OriginalProperty;
			Assert.AreEqual(_ThingyProp_Name, oprop_name);
			Assert.AreEqual(origName, oprop_name.GetValue(target));
			Assert.AreEqual(_ThingyProp_Awesomeness, oprop_awesomenesss);
			Assert.AreEqual(origAwesomeness, oprop_awesomenesss.GetValue(target));

			Assert.AreEqual(origName, state.GetProperty("Name"));
			Assert.AreEqual(origName, sprop_name.GetValue(state));
			Assert.AreEqual(origAwesomeness, state.GetProperty("Awesomeness"));
			Assert.AreEqual(origAwesomeness, sprop_awesomeness.GetValue(state));

			var modName = "xyz";
			var modAwesomeness = -34;

			target.Name = modName;
			target.Awesomeness = modAwesomeness;

			Assert.AreEqual(origName, state.GetProperty("Name"));
			Assert.AreEqual(origName, sprop_name.GetValue(state));
			Assert.AreEqual(origAwesomeness, state.GetProperty("Awesomeness"));
			Assert.AreEqual(origAwesomeness, sprop_awesomeness.GetValue(state));

			state.Load();

			Assert.AreEqual(modName, sprop_name.GetValue(state));
			Assert.AreEqual(modName, state.GetProperty("Name"));
			Assert.AreEqual(modAwesomeness, sprop_awesomeness.GetValue(state));
			Assert.AreEqual(modAwesomeness, state.GetProperty("Awesomeness"));
		}

	}

#pragma warning restore 168
	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
