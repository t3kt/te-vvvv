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
		[TestCategory(CategoryNames.CommonUtil)]
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
		[TestCategory(CategoryNames.CommonUtil)]
		public void MaxOrZeroSelectedTest()
		{
			Assert.AreEqual(0, CommonUtil.MaxOrZero(Enumerable.Empty<ThingWithInt>(), x => x.val));
			Assert.AreEqual(5, CommonUtil.MaxOrZero(new[] { new ThingWithInt(2), new ThingWithInt(5) }, x => x.val));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new[] { new ThingWithInt(-40) }, x => x.val));
		}

		[TestMethod]
		[TestCategory(CategoryNames.CommonUtil)]
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
		[TestCategory(CategoryNames.CommonUtil)]
		public void NullableMaxOrZeroSelectedTest()
		{
			Assert.AreEqual(0, CommonUtil.MaxOrZero(Enumerable.Empty<int?>()));
			Assert.AreEqual(5, CommonUtil.MaxOrZero(new[] { new ThingWithNullableInt(2), new ThingWithNullableInt(5), new ThingWithNullableInt(null) }, x => x.val));
			Assert.AreEqual(0, CommonUtil.MaxOrZero(new[] { new ThingWithNullableInt(null) }, x => x.val));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new[] { new ThingWithNullableInt(-40) }, x => x.val));
			Assert.AreEqual(-40, CommonUtil.MaxOrZero(new[] { new ThingWithNullableInt(-40), new ThingWithNullableInt(null), new ThingWithNullableInt(-100) }, x => x.val));
		}

		[TestMethod]
		[TestCategory(CategoryNames.CommonUtil)]
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
		[TestCategory(CategoryNames.CommonUtil)]
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

		[TestMethod]
		[TestCategory(CategoryNames.CommonUtil)]
		public void TimeSpanMinMaxClamp()
		{
			var ts_1 = TimeSpan.FromSeconds(1);
			var ts_2 = TimeSpan.FromSeconds(2);
			var ts_3 = TimeSpan.FromSeconds(3);
			var ts_4 = TimeSpan.FromSeconds(4);

			Assert.AreEqual(ts_1, CommonUtil.Min(ts_1, ts_2));
			Assert.AreEqual(ts_1, CommonUtil.Min(ts_2, ts_1));
			Assert.AreEqual(ts_1, CommonUtil.Min(ts_1, ts_1));

			Assert.AreEqual(ts_2, CommonUtil.Max(ts_1, ts_2));
			Assert.AreEqual(ts_2, CommonUtil.Max(ts_2, ts_1));
			Assert.AreEqual(ts_2, CommonUtil.Max(ts_2, ts_2));

			Assert.AreEqual(ts_2, CommonUtil.Clamp(ts_1, ts_2, ts_3));
			Assert.AreEqual(ts_2, CommonUtil.Clamp(ts_1, ts_3, ts_2));
			Assert.AreEqual(ts_2, CommonUtil.Clamp(ts_1, ts_2, ts_2));

			Assert.AreEqual(ts_3, CommonUtil.Clamp(ts_4, ts_2, ts_3));
			Assert.AreEqual(ts_3, CommonUtil.Clamp(ts_4, ts_3, ts_2));
			Assert.AreEqual(ts_2, CommonUtil.Clamp(ts_4, ts_2, ts_2));
		}

		private static void TimeSpanIsOverlapHelper(Span a, Span b, bool expected)
		{
			var actual = CommonUtil.IsOverlap(a.Start, a.End, b.Start, b.End);
			Assert.AreEqual(expected, actual, "IsOverlap {0} :: {1} should be {2}", a, b, expected);
			Assert.AreEqual(actual, CommonUtil.IsOverlap(b.Start, b.End, a.Start, a.End), "IsOverlap {0} :: {1} should be reflexive", a, b);
		}

		struct Span
		{
			public readonly TimeSpan Start, End;
			public Span(TimeSpan s, TimeSpan e) { this.Start = s; this.End = e; }
			public override string ToString()
			{
				return String.Format("[{0} -> {1}]", this.Start, this.End);
			}
		}

		[TestMethod]
		[TestCategory(CategoryNames.CommonUtil)]
		public void TimeSpanIsOverlap()
		{
			var ts_10 = TimeSpan.FromSeconds(10);
			var ts_20 = TimeSpan.FromSeconds(20);
			var ts_30 = TimeSpan.FromSeconds(30);
			var ts_34 = TimeSpan.FromSeconds(34);
			var ts_35 = TimeSpan.FromSeconds(35);
			var ts_40 = TimeSpan.FromSeconds(40);
			var ts_50 = TimeSpan.FromSeconds(50);
			var ts_60 = TimeSpan.FromSeconds(60);

			TimeSpanIsOverlapHelper(new Span(ts_10, ts_20), new Span(ts_30, ts_40), false);
			TimeSpanIsOverlapHelper(new Span(ts_50, ts_60), new Span(ts_30, ts_40), false);
			TimeSpanIsOverlapHelper(new Span(ts_10, ts_35), new Span(ts_30, ts_40), true);
			TimeSpanIsOverlapHelper(new Span(ts_34, ts_35), new Span(ts_30, ts_40), true);
			TimeSpanIsOverlapHelper(new Span(ts_35, ts_50), new Span(ts_30, ts_40), true);
		}

		private static void IntervalIsOverlapHelper(Interval a, Interval b, bool expected)
		{
			var actual = a.Overlaps(b);
			Assert.AreEqual(expected, actual, "IsOverlap {0} :: {1} should be {2}", a, b, expected);
			Assert.AreEqual(actual, b.Overlaps(a), "IsOverlap {0} :: {1} should be reflexive", a, b);
		}

		[TestMethod]
		[TestCategory(CategoryNames.CommonUtil)]
		public void IntervalIsOverlap()
		{
			var ts_10 = TimeSpan.FromSeconds(10);
			var ts_20 = TimeSpan.FromSeconds(20);
			var ts_30 = TimeSpan.FromSeconds(30);
			var ts_34 = TimeSpan.FromSeconds(34);
			var ts_35 = TimeSpan.FromSeconds(35);
			var ts_40 = TimeSpan.FromSeconds(40);
			var ts_50 = TimeSpan.FromSeconds(50);
			var ts_60 = TimeSpan.FromSeconds(60);

			IntervalIsOverlapHelper(Interval.FromBounds(ts_10, ts_20), Interval.FromBounds(ts_30, ts_40), false);
			IntervalIsOverlapHelper(Interval.FromBounds(ts_50, ts_60), Interval.FromBounds(ts_30, ts_40), false);
			IntervalIsOverlapHelper(Interval.FromBounds(ts_10, ts_35), Interval.FromBounds(ts_30, ts_40), true);
			IntervalIsOverlapHelper(Interval.FromBounds(ts_34, ts_35), Interval.FromBounds(ts_30, ts_40), true);
			IntervalIsOverlapHelper(Interval.FromBounds(ts_35, ts_50), Interval.FromBounds(ts_30, ts_40), true);
		}

		private static void IntervalPreventOverlap_NoOverlap(Interval target, Interval other)
		{
			Assert.IsFalse(target.Overlaps(other));
			var result = Interval.PreventOverlap(target, other);
			Assert.AreEqual(target, result);
		}

		private static void IntervalPreventOverlap_Modified(Interval target, Interval other, Interval expected)
		{
			Assert.IsTrue(target.Overlaps(other));
			var result = Interval.PreventOverlap(target, other);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		[TestCategory(CategoryNames.CommonUtil)]
		public void IntervalPreventOverlap()
		{
			var ts_10 = TimeSpan.FromSeconds(10);
			var ts_20 = TimeSpan.FromSeconds(20);
			var ts_30 = TimeSpan.FromSeconds(30);
			var ts_40 = TimeSpan.FromSeconds(40);

			var i_10_20 = Interval.FromBounds(ts_10, ts_20);
			var i_10_40 = Interval.FromBounds(ts_10, ts_40);
			var i_10_30 = Interval.FromBounds(ts_10, ts_30);
			var i_20_30 = Interval.FromBounds(ts_20, ts_30);
			var i_20_40 = Interval.FromBounds(ts_20, ts_40);
			var i_30_40 = Interval.FromBounds(ts_30, ts_40);
			var i_40_40 = Interval.FromBounds(ts_40, ts_40);

			// other BEFORE target
			//               [-target-]
			// [-other-]
			// no change
			IntervalPreventOverlap_NoOverlap(i_30_40, i_10_20);

			// other AFTER target
			// [-target-]
			//               [-other-]
			// no change
			IntervalPreventOverlap_NoOverlap(i_10_20, i_30_40);

			// other AFTER target OVERLAPPING
			// [-----target-----]
			//          [-----other-----]
			// change to:
			// [-target-]
			IntervalPreventOverlap_Modified(i_10_30, i_20_40, i_10_20);

			// other BEFORE target OVERLAPPING
			IntervalPreventOverlap_Modified(i_20_40, i_10_30, i_30_40);

			// other CONTAINS target
			{
				var tgt = i_20_30;
				var other = i_10_40;
				Assert.IsTrue(other.Contains(tgt));
				IntervalPreventOverlap_Modified(tgt, other, i_40_40);
			}

			// target CONTAINS other
			{
				var tgt = i_10_40;
				var other = i_20_30;
				Assert.IsTrue(tgt.Contains(other));
				IntervalPreventOverlap_Modified(tgt, other, i_30_40);
			}
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
