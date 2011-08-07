using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Animator.Common;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Model.Clips;
using Animator.Core.Model.Sequences;
using Animator.Core.Transport;
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
	public class ModelUnitTest
	{

		private static XmlSchema _DocumentSchema;
		private static XmlSchemaSet _DocumentSchemaSet;

		[ClassInitialize]
		public static void LoadDocumentSchemas(TestContext ctx)
		{
			using(var stream = typeof(Document).Assembly.GetManifestResourceStream("Animator.Core.Model.AniDocument.xsd"))
			{
				Assert.IsNotNull(stream);
				_DocumentSchema = XmlSchema.Read(stream, null);
				_DocumentSchemaSet = new XmlSchemaSet();
				_DocumentSchemaSet.Add(_DocumentSchema);
			}
		}

		private static void ValidateDocumentSchema(XDocument document)
		{
			Assert.IsNotNull(document);
			document.Validate(_DocumentSchemaSet, null);
		}

		private static XmlQualifiedName ToQualName(XName name)
		{
			return new XmlQualifiedName(name.LocalName, name.NamespaceName);
		}

		private static void ValidateElementSchema(XElement element, XName elementName)
		{
			Assert.IsNotNull(element);
			Assert.IsNotNull(elementName);
			var elementSchema = _DocumentSchema.Elements[ToQualName(elementName)];
			Assert.IsNotNull(elementSchema);
			element.Validate(elementSchema, _DocumentSchemaSet, null);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void ParametersEqual()
		{
			var x =
				new Dictionary<string, string>
				{
					{"foo", "--foo....."},
					{"baz",String.Empty},
					{"qux",null}
				};
			var y =
				new Dictionary<string, string>
				{
					{"baz",String.Empty},
					{"qux",null},
					{"foo", "--foo....."}
				};
			Assert.IsTrue(ModelUtil.ParametersEqual(x, y));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void SeqClipReadWriteXElement()
		{
			var host = CompositionUnitTest.CreateHost(test: false, core: true);
			var clipA = new SequenceClip { Name = "helloclip", Start = TimeSpan.FromSeconds(4.2), Duration = TimeSpan.FromSeconds(49) };
			clipA.Properties.Add(new ConstData { Name = "fooo", Value = -23.421d });
			var stepProp = new StepData { Name = "etc" };
			stepProp.Steps.AddRange(new[] { 9492d, -23.04d, 0.0001d });
			clipA.Properties.Add(stepProp);

			var xmlA = clipA.WriteXElement();
			ValidateElementSchema(xmlA, Schema.seqclip);

			var clipB = new SequenceClip(xmlA, host);
			var xmlB = clipB.WriteXElement();

			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			//Assert.AreEqual(clipA, clipB);
			Assert.AreEqual(clipA.Name, clipB.Name);
			Assert.AreEqual(clipA.Start, clipB.Start);
			Assert.AreEqual(clipA.Duration, clipB.Duration);
			Assert.AreEqual(clipA.Properties.Count, clipB.Properties.Count);
			Assert.AreEqual(clipA.Properties[0].Name, clipB.Properties[0].Name);
			//...
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void DocumentReadWriteXDocument()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var docA = new Document
					   {
						   Name = "foodoc"
					   };
			var outputA1 = new Output { Name = "out1" };
			docA.Outputs.Add(outputA1);
			//var clipA1 = new Clip { Name = "helloclip", OutputId = outputA1.Id, UIRow = 12 };
			//docA.Clips.Add(clipA1);
			//var clipA2 = new StepClip
			//             {
			//                 Name = "steppp",
			//                 Duration = 48.5f,
			//                 TargetKey = "footarget",
			//                 UIColumn = 4,
			//                 UIRow = 2
			//             };
			//clipA2.Steps.ReplaceContents(new[] { 25.25d, -234.3d, 0.0d });
			//docA.Clips.Add(clipA2);
			var xmlA = docA.WriteXDocument();
			ValidateDocumentSchema(xmlA);
			var docB = new Document(xmlA, host);
			var xmlB = docB.WriteXDocument();
			Assert.AreEqual(docA.Id, docB.Id);
			Assert.AreEqual(docA.Name, docB.Name);
			//CollectionAssert.Contains(docB.Clips, clipA1);
			//CollectionAssert.Contains(docB.Clips, clipA2);
			//Assert.IsTrue(docB.Clips.ItemsEqual(docA.Clips));
			CollectionAssert.Contains(docB.Outputs, outputA1);
			Assert.IsTrue(docB.Outputs.ItemsEqual(docA.Outputs));
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			Assert.Inconclusive();
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void TargetObjectEquality()
		{
			const string propA_Name = "propA";
			var tgt1 = new TargetObject { Name = "tgt", OutputKey = "foo/tgt/" };
			var tgt2 = new TargetObject(tgt1.Id) { Name = tgt1.Name, OutputKey = tgt1.OutputKey };
			Assert.AreEqual(tgt1, tgt2);
			var prop1A = tgt1.Add(propA_Name, TargetPropertyType.Value, 23.1d);
			var prop2A = tgt2.Add(prop1A.Name, prop1A.Type, prop1A.DefaultValue);
			Assert.AreEqual(tgt1, tgt2);
			Assert.IsTrue(tgt1.SetDefaultValue(prop1A.Name, 999.4d));
			Assert.AreNotEqual(tgt1, tgt2);
			Assert.IsTrue(tgt2.SetDefaultValue(prop1A.Name, prop1A.DefaultValue));
			Assert.AreEqual(tgt1, tgt2);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void TargetObjectReadWriteXElement()
		{
			var tgt1 = new TargetObject { Name = "tgt", OutputKey = "foo/tgt/" };
			var prop1A = tgt1.Add("propA", TargetPropertyType.Value, 23.1d);
			var prop1B = tgt1.Add("propB", TargetPropertyType.String, "foo");
			var xml1 = tgt1.WriteXElement();
			var tgt2 = new TargetObject(xml1);
			var xml2 = tgt2.WriteXElement();
			ValidateElementSchema(xml1, Schema.target);
			Assert.AreEqual(xml1.ToString(), xml2.ToString());
			Assert.AreEqual(tgt1, tgt2);
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
