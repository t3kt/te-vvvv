using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.Common;
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

	[TestClass]
	public class CommonUtilUnitTest
	{

		[TestMethod]
		[TestCategory("CommonUtil")]
		public void MaxOrZeroBasicTest()
		{
			Assert.AreEqual(0, CommonUtil.MaxOrZero(Enumerable.Empty<int>()));
			Assert.AreEqual(5, CommonUtil.MaxOrZero(new[] { 2, 5 }));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new[] { -40 }));
		}

		class ThingWithInt
		{
			public int val;
			public ThingWithInt(int val)
			{
				this.val = val;
			}
		}

		[TestMethod]
		[TestCategory("CommonUtil")]
		public void MaxOrZeroSelectedTest()
		{
			Assert.AreEqual(0, CommonUtil.MaxOrZero(Enumerable.Empty<ThingWithInt>(), x => x.val));
			Assert.AreEqual(5, CommonUtil.MaxOrZero(new[] { new ThingWithInt(2), new ThingWithInt(5) }, x => x.val));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new[] { new ThingWithInt(-40) }, x => x.val));
		}

		[TestMethod]
		[TestCategory("CommonUtil")]
		public void NullableMaxOrZeroBasicTest()
		{
			Assert.AreEqual(0, CommonUtil.MaxOrZero(Enumerable.Empty<int?>()));
			Assert.AreEqual(5, CommonUtil.MaxOrZero(new int?[] { 2, 5, null }));
			Assert.AreEqual(0, CommonUtil.MaxOrZero(new int?[] { null }));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new int?[] { -40 }));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new int?[] { -40, null, -100 }));
		}

		class ThingWithNullableInt
		{
			public int? val;
			public ThingWithNullableInt(int? val)
			{
				this.val = val;
			}
		}

		[TestMethod]
		[TestCategory("CommonUtil")]
		public void NullableMaxOrZeroSelectedTest()
		{
			Assert.AreEqual(0, CommonUtil.MaxOrZero(Enumerable.Empty<int?>()));
			Assert.AreEqual(5, CommonUtil.MaxOrZero(new[] { new ThingWithNullableInt(2), new ThingWithNullableInt(5), new ThingWithNullableInt(null) }, x => x.val));
			Assert.AreEqual(0, CommonUtil.MaxOrZero(new[] { new ThingWithNullableInt(null) }, x => x.val));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new[] { new ThingWithNullableInt(-40) }, x => x.val));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new[] { new ThingWithNullableInt(-40), new ThingWithNullableInt(null), new ThingWithNullableInt(-100) }, x => x.val));
		}

		[TestMethod]
		[TestCategory("CommonUtil")]
		public void ResizeCollectionTest()
		{
			List<int> listA;

			listA = new List<int> { 0, 1, 2 };
			CommonUtil.ResizeCollection(listA, 3, i => i * 10);
			CollectionAssert.AreEqual(new[] { 0, 1, 2 }, listA);

			listA = new List<int> { 0, 1, 2 };
			CommonUtil.ResizeCollection(listA, 5, i => i * 10);
			CollectionAssert.AreEqual(new[] { 0, 1, 2, 30, 40 }, listA);

			listA = new List<int> { 0, 1, 2 };
			CommonUtil.ResizeCollection(listA, 2, i => i * 10);
			CollectionAssert.AreEqual(new[] { 0, 1 }, listA);
		}

		[TestMethod]
		[TestCategory("CommonUtil")]
		public void CropListTest()
		{
			List<int> list;

			list = new List<int> { 0, 1, 2, 3, 4 };
			CommonUtil.CropList(list, 10);
			CollectionAssert.AreEqual(list, new[] { 0, 1, 2, 3, 4 });

			list = new List<int> { 0, 1, 2, 3, 4 };
			CommonUtil.CropList(list, 3);
			CollectionAssert.AreEqual(list, new[] { 0, 1, 2 });

			list = new List<int> { 0, 1, 2, 3, 4 };
			CommonUtil.CropList(list, 0);
			CollectionAssert.AreEqual(list, new int[0]);
		}

	}

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
