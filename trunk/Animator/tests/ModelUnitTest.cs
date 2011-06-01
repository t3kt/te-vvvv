using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Animator.Common;
using Animator.Core.Model;
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
		public void ClipReadWriteXElement()
		{
			var host = CompositionUnitTest.CreateHost(test: false, core: true);
			var clipA = new Clip { Name = "helloclip", OutputId = Guid.NewGuid(), UIRow = 12 };
			var xmlA = clipA.WriteXElement();
			var clipB = new Clip();
			clipB.ReadXElement(xmlA);
			var xmlB = clipB.WriteXElement();
			var clipC = host.ReadClipXElement(xmlA);
			var xmlC = clipC.WriteXElement();
			ValidateElementSchema(xmlA, Schema.clip);
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			Assert.AreEqual(clipA, clipB);
			Assert.AreEqual(clipA.UIRow, clipB.UIRow);
			Assert.AreEqual(clipA.UIColumn, clipB.UIColumn);
			Assert.AreEqual(xmlA.ToString(), xmlC.ToString());
			Assert.AreEqual(clipA, clipC);
			Assert.AreEqual(clipA.UIRow, clipC.UIRow);
			Assert.AreEqual(clipA.UIColumn, clipC.UIColumn);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void StepClipReadWriteXElement()
		{
			var host = CompositionUnitTest.CreateHost(test: false, core: true);
			var clipA = new StepClip
						{
							Name = "helloclip",
							OutputId = Guid.NewGuid(),
							UIRow = 12
						};
			clipA.Steps.ReplaceContents(new[] { 40.2f, -345.28f, 0.0f, 4444.4f });
			var xmlA = clipA.WriteXElement();
			var clipB = new StepClip(xmlA);
			var xmlB = clipB.WriteXElement();
			var clipC = host.ReadClipXElement(xmlA);
			ValidateElementSchema(xmlA, Schema.stepclip);
			Assert.IsInstanceOfType(clipC, typeof(StepClip));
			var xmlC = clipC.WriteXElement();
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
			Assert.AreEqual(clipA, clipB);
			Assert.AreEqual(clipA.UIRow, clipB.UIRow);
			Assert.AreEqual(clipA.UIColumn, clipB.UIColumn);
			Assert.AreEqual(xmlA.ToString(), xmlC.ToString());
			Assert.AreEqual(clipA, clipC);
			Assert.AreEqual(clipA.UIRow, clipC.UIRow);
			Assert.AreEqual(clipA.UIColumn, clipC.UIColumn);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void DocumentReadWriteXDocument()
		{
			var host = CompositionUnitTest.CreateHost(test: true, core: true);
			var docA = new Document
					   {
						   BeatsPerMinute = 44.3f,
						   Name = "foodoc",
						   UIColumns = 23,
						   UIRows = 14
					   };
			var outputA1 = new Output { Name = "out1" };
			docA.Outputs.Add(outputA1);
			var clipA1 = new Clip { Name = "helloclip", OutputId = outputA1.Id, UIRow = 12 };
			docA.Clips.Add(clipA1);
			var clipA2 = new StepClip
						 {
							 Name = "steppp",
							 Duration = 48.5f,
							 TargetKey = "footarget",
							 UIColumn = 4,
							 UIRow = 2
						 };
			clipA2.Steps.ReplaceContents(new[] { 25.25f, -234.3f, 0.0f });
			docA.Clips.Add(clipA2);
			var xmlA = docA.WriteXDocument();
			ValidateDocumentSchema(xmlA);
			var docB = new Document(xmlA, host);
			var xmlB = docB.WriteXDocument();
			Assert.AreEqual(docA.BeatsPerMinute, docB.BeatsPerMinute);
			Assert.AreEqual(docA.Id, docB.Id);
			Assert.AreEqual(docA.Name, docB.Name);
			Assert.AreEqual(docA.UIColumns, docB.UIColumns);
			Assert.AreEqual(docA.UIRows, docB.UIRows);
			CollectionAssert.Contains(docB.Clips, clipA1);
			CollectionAssert.Contains(docB.Clips, clipA2);
			Assert.IsTrue(docB.Clips.ItemsEqual(docA.Clips));
			CollectionAssert.Contains(docB.Outputs, outputA1);
			Assert.IsTrue(docB.Outputs.ItemsEqual(docA.Outputs));
			Assert.AreEqual(xmlA.ToString(), xmlB.ToString());
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void ClipEqualityComparison()
		{
			var clipA = new Clip(Guid.NewGuid())
						{
							Duration = 3.0f,
							Name = "fooClip",
							TargetKey = "tgt",
							OutputId = Guid.NewGuid()
						};
			var clipB = new Clip(clipA.Id)
						{
							Duration = clipA.Duration,
							Name = clipA.Name,
							TargetKey = clipA.TargetKey,
							OutputId = clipA.OutputId
						};
			Assert.AreEqual(clipA, clipB);
			clipB.Duration = -999.33f;
			Assert.AreNotEqual(clipA, clipB);

			var stepClipA = new StepClip(Guid.NewGuid())
							{
								Duration = 9.0f,
								Name = "stepssss",
								TargetKey = "tttgtt",
								OutputId = Guid.NewGuid()
							};
			stepClipA.Steps.ReplaceContents(new[] { 3.5f, 12.0f });
			Assert.AreNotEqual(clipA, stepClipA);
			var stepClipB = new StepClip(stepClipA.Id)
							{
								Duration = stepClipA.Duration,
								Name = stepClipA.Name,
								TargetKey = stepClipA.TargetKey,
								OutputId = stepClipA.OutputId
							};
			stepClipB.Steps.ReplaceContents(stepClipA.Steps);
			Assert.AreEqual(stepClipA, stepClipB);
			stepClipB.Steps.Add(-234.77f);
			Assert.AreNotEqual(stepClipA, stepClipB);
			stepClipB.Steps.RemoveAt(stepClipB.Steps.Count - 1);
			stepClipB.Name = "qqq";
			Assert.AreNotEqual(stepClipA, stepClipB);
			stepClipB.Name = stepClipA.Name;
			Assert.AreEqual(stepClipA, stepClipB);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		[ExpectedException(typeof(ArgumentException))]
		public void NoDuplicateIds()
		{
			var doc = new Document();
			var output = new Output();
			var clip = new Clip(output.Id);
			doc.Clips.Add(clip);
			Assert.Inconclusive("Write id uniqueness checks");
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void StepClipGetValue()
		{
			var doc = new Document();
			var clip = new StepClip
			{
				Duration = new Time(4)
			};
			clip.Steps.ReplaceContents(new[] { 0.0f, 1.0f, 2.0f, 3.0f });
			doc.Clips.Add(clip);
			Assert.AreEqual(0.0f, clip.GetValue(new Time(0.0f)));
			Assert.AreEqual(1.0f, clip.GetValue(new Time(1.0f)));
			Assert.AreEqual(0.0f, clip.GetValue(new Time(0.4f)));
			Assert.AreEqual(0.0f, clip.GetValue(new Time(0.5f)));
			Assert.AreEqual(1.0f, clip.GetValue(new Time(1.5f)));
			Assert.AreEqual(1.0f, clip.GetValue(new Time(5.5f)));
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void TargetObjectEquality()
		{
			const string propA_Name = "propA";
			var tgt1 = new TargetObject { Name = "tgt", OutputKey = "foo/tgt/" };
			var tgt2 = new TargetObject(tgt1.Id) { Name = tgt1.Name, OutputKey = tgt1.OutputKey };
			Assert.AreEqual(tgt1, tgt2);
			var prop1A = tgt1.Add(propA_Name, TargetPropertyType.Value, 23.1f);
			var prop2A = tgt2.Add(prop1A.Name, prop1A.Type, prop1A.DefaultValue);
			Assert.AreEqual(tgt1, tgt2);
			prop1A.DefaultValue = 999.4f;
			Assert.AreNotEqual(tgt1, tgt2);
			prop2A.DefaultValue = prop1A.DefaultValue;
			Assert.AreEqual(tgt1, tgt2);
		}

		[TestMethod]
		[TestCategory(CategoryNames.Model)]
		public void TargetObjectReadWriteXElement()
		{
			var tgt1 = new TargetObject { Name = "tgt", OutputKey = "foo/tgt/" };
			var prop1A = tgt1.Add("propA", TargetPropertyType.Value, 23.1f);
			var prop1B = tgt1.Add("propB", TargetPropertyType.String, "foo");
			var xml1 = tgt1.WriteXElement();
			var tgt2 = new TargetObject(xml1);
			var xml2 = tgt2.WriteXElement();
			ValidateElementSchema(xml1, Schema.target);
			Assert.AreEqual(xml1.ToString(), xml2.ToString());
			Assert.AreEqual(tgt1, tgt2);
		}

	}

	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
