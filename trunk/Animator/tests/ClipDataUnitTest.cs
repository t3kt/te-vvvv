using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Common;
using Animator.Core.Model.Clips;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local
	// ReSharper disable MemberCanBeMadeStatic.Local
	// ReSharper disable ConvertToLambdaExpression
	// ReSharper disable RedundantArgumentName
	// ReSharper disable UseObjectOrCollectionInitializer

	[TestClass]
	public class ClipDataUnitTest
	{

		[TestMethod]
		[TestCategory(CategoryNames.ClipData)]
		public void ConstDataGetValue()
		{
			var data = new ConstData();
			data.Value = 23.4f;
			Assert.AreEqual(data.Value, data.GetValue(0.5f));
			Assert.AreEqual(data.Value, data.GetValue(10.3f));
			Assert.AreEqual(data.Value, data.GetValue(-40.1f));
			data.Value = 505.341f;
			Assert.AreEqual(data.Value, data.GetValue(0.5f));
			Assert.AreEqual(data.Value, data.GetValue(10.3f));
			Assert.AreEqual(data.Value, data.GetValue(-40.1f));
		}

		[TestMethod]
		[TestCategory(CategoryNames.ClipData)]
		public void StepDataGetIndex()
		{
			var data = new StepData();
			data.Steps.AddRange(new[] { 0f, 1f, 2f, 3f });

			Assert.AreEqual(4, data.Steps.Count);

			Assert.AreEqual(0, data.GetIndex(0f));
			Assert.AreEqual(0, data.GetIndex(0.1f));
			Assert.AreEqual(0, data.GetIndex(0.24f));
			Assert.AreEqual(1, data.GetIndex(0.25f));
			Assert.AreEqual(1, data.GetIndex(0.27f));
			Assert.AreEqual(1, data.GetIndex(0.49f));
			Assert.AreEqual(2, data.GetIndex(0.5f));
			Assert.AreEqual(2, data.GetIndex(0.51f));
			Assert.AreEqual(3, data.GetIndex(0.75f));
			Assert.AreEqual(3, data.GetIndex(0.95f));
			Assert.AreEqual(3, data.GetIndex(1.0f));
		}

		[TestMethod]
		[TestCategory(CategoryNames.ClipData)]
		public void StepDataGetValueBasic()
		{
			var data = new StepData();
			data.Steps.AddRange(new[] { 0f, 1f, 2f, 3f });

			Assert.AreEqual(0f, data.GetValue(0f));
			Assert.AreEqual(1f, data.GetValue(0.25f));
			Assert.AreEqual(2f, data.GetValue(0.5f));
			Assert.AreEqual(2f, data.GetValue(0.53f));
			Assert.AreEqual(3f, data.GetValue(0.75f));
			Assert.AreEqual(3f, data.GetValue(0.95f));
			Assert.AreEqual(3f, data.GetValue(1f));
		}

	}

	// ReSharper restore UseObjectOrCollectionInitializer
	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConvertToLambdaExpression
	// ReSharper restore MemberCanBeMadeStatic.Local
	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
