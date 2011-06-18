using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Model;
using Animator.Core.Model.Sequences;
using Animator.Core.Model.Sessions;
using Animator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression
	// ReSharper disable RedundantTypeArgumentsOfMethod
#pragma warning disable 168

	[TestClass]
	public class RuntimeUnitTest
	{

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

		private void TargetObjectNotifyValueChangesHelper<T>(TargetPropertyType prop1_type, T prop1_def, T testVal)
		{
			Assert.AreEqual(typeof(T), TargetProperty.GetValueType(prop1_type));

			var output = new CollectorOutput();
			var tgt = new TargetObject { OutputKey = "fooOutput-" };
			output.Targets.Add(tgt);

			var prop1_name = "prop1";
			var prop1 = tgt.Add(prop1_name, prop1_type, prop1_def);

			var events = new List<TargetPropertyValueChangedEventArgs>();
			tgt.PropertyValueChanged += (sender, e) =>
			{
				Assert.AreSame(tgt, sender);
				events.Add(e);
			};

			Assert.AreEqual(tgt.GetValue(prop1_name), prop1_def);
			Assert.AreEqual(prop1.Value, prop1_def);

			Assert.IsTrue(tgt.SetValue(prop1_name, testVal));
			Assert.AreEqual(tgt.GetValue(prop1_name), testVal);

			Assert.AreEqual(1, events.Count);
			Assert.AreEqual(prop1_name, events[0].Name);
			Assert.AreEqual(tgt.OutputKey, events[0].OutputKey);
			Assert.AreEqual(testVal, events[0].Value);
			events.Clear();

			Assert.AreEqual(1, output.Messages.Count);
			Assert.AreEqual(tgt.OutputKey + TargetPropertyValueChangedEventArgs.KeyNameSeparator + prop1_name, output.Messages[0].TargetKey);
			CollectionAssert.AreEqual(new object[] { testVal }, output.Messages[0].Data);
			output.Messages.Clear();

			Assert.IsTrue(tgt.SetValue(prop1_name, testVal));
			Assert.AreEqual(0, events.Count);
			Assert.AreEqual(0, output.Messages.Count);
			Assert.IsTrue(tgt.SetValue(prop1_name, testVal));
			Assert.AreEqual(0, events.Count);
			Assert.AreEqual(0, output.Messages.Count);

			Assert.IsTrue(tgt.ClearValue(prop1_name));
			Assert.AreEqual(1, events.Count);
			Assert.AreEqual(prop1_name, events[0].Name);
			Assert.AreEqual(tgt.OutputKey, events[0].OutputKey);
			Assert.AreEqual(prop1_def, events[0].Value);
			events.Clear();

			Assert.AreEqual(1, output.Messages.Count);
			Assert.AreEqual(tgt.OutputKey + TargetPropertyValueChangedEventArgs.KeyNameSeparator + prop1_name, output.Messages[0].TargetKey);
			CollectionAssert.AreEqual(new object[] { prop1_def }, output.Messages[0].Data);
			output.Messages.Clear();

			Assert.IsTrue(tgt.SetValue(prop1_name, testVal));
			events.Clear();
			output.Messages.Clear();
			Assert.IsTrue(tgt.SetValue(prop1_name, prop1_def));

			Assert.AreEqual(1, events.Count);
			Assert.AreEqual(prop1_name, events[0].Name);
			Assert.AreEqual(tgt.OutputKey, events[0].OutputKey);
			Assert.AreEqual(prop1_def, events[0].Value);
			events.Clear();

			Assert.AreEqual(1, output.Messages.Count);
			Assert.AreEqual(tgt.OutputKey + TargetPropertyValueChangedEventArgs.KeyNameSeparator + prop1_name, output.Messages[0].TargetKey);
			CollectionAssert.AreEqual(new object[] { prop1_def }, output.Messages[0].Data);
			output.Messages.Clear();

			Assert.IsTrue(tgt.ClearValue(prop1_name));
			Assert.AreEqual(0, events.Count);
			Assert.AreEqual(0, output.Messages.Count);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		public void TargetObjectNotifyValueChanges_ValueProperty()
		{
			this.TargetObjectNotifyValueChangesHelper<float>(TargetPropertyType.Value, 3.2f, 252.3f);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		public void TargetObjectNotifyValueChanges_StringProperty()
		{
			this.TargetObjectNotifyValueChangesHelper<string>(TargetPropertyType.String, "xyz#123", "helloooooooo");
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		public void ActiveSectionMustBeInDocument_Valid()
		{
			var doc = new Document();
			var ses = new Session(doc);
			doc.Sections.Add(ses);
			doc.ActiveSection = ses;
			var seq = new Sequence(doc);
			doc.Sections.Add(seq);
			doc.ActiveSection = seq;
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		[ExpectedException(typeof(ModelException))]
		public void ActiveSessionMustBeInDocument_Invalid()
		{
			var doc = new Document();
			var ses = new Session(doc);
			doc.ActiveSection = ses;
		}

		[TestMethod]
		[TestCategory(CategoryNames.Runtime_Model)]
		[ExpectedException(typeof(ModelException))]
		public void ActiveSequenceMustBeInDocument_Invalid()
		{
			var doc = new Document();
			var seq = new Sequence(doc);
			doc.ActiveSection = seq;
		}

	}

#pragma warning restore 168
	// ReSharper restore RedundantTypeArgumentsOfMethod
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
