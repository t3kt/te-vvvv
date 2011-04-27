using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Animator.AppCore.Common;
using Animator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Animator.Tests.AniApp
{
	// ReSharper disable SuggestUseVarKeywordEvident
	// ReSharper disable SuggestUseVarKeywordEverywhere
	// ReSharper disable ConvertToConstant.Local


	/// <summary>
	///This is a test class for LinkedStackTest and is intended
	///to contain all LinkedStackTest Unit Tests
	///</summary>
	[TestClass]
	public class LinkedStackTest
	{

		private readonly ReadOnlyCollection<int> _TestValues = new ReadOnlyCollection<int>(new List<int> { 10, 20, 30, 40 });

		public TestContext TestContext { get; set; }

		private int[] GetTestValuesForPush()
		{
			int[] values = _TestValues.Reverse().ToArray();
			return values;
		}

		private LinkedStack<int> CreateWithTestValues()
		{
			LinkedStack<int> target = new LinkedStack<int>();
			this.AddTestValues(target);
			Assert.AreEqual(_TestValues.Count, target.Count);
			CollectionAssert.AreEqual(_TestValues, target);
			return target;
		}

		private void AddTestValues(LinkedStack<int> stack)
		{
			Debug.Assert(stack != null);
			IEnumerable<int> values = this.GetTestValuesForPush();
			stack.PushRange(values);
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void LinkedStackConstructorWithValuesTest()
		{
			var stack = CreateWithTestValues();
			CollectionAssert.AreEqual(_TestValues, stack);
		}

		/// <summary>
		///A test for LinkedStack`1 Constructor
		///</summary>
		public LinkedStack<T> CreateEmpty<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>();
			Assert.AreEqual(0, target.Count);
			Assert.AreEqual(0, target.Count());
			Assert.IsFalse(target.Any());
			return target;
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void LinkedStackConstructorTest1()
		{
			this.CreateEmpty<GenericParameterHelper>();
		}

		/// <summary>
		///A test for Clear
		///</summary>
		public void ClearTestHelper<T>()
		{
			LinkedStack<T> target = new LinkedStack<T>();
			target.Push(default(T));
			target.Push(default(T));
			target.Push(default(T));
			target.Clear();
			Assert.AreEqual(0, target.Count);
			Assert.IsFalse(target.Any());
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void ClearTest()
		{
			ClearTestHelper<GenericParameterHelper>();
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PeekTest()
		{
			LinkedStack<int> target = this.CreateEmpty<int>();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = target.Peek();
																	  }, "Peek was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.First();
			int actual = target.Peek();
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(origCount, target.Count);
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PopTest()
		{
			LinkedStack<int> target = this.CreateEmpty<int>();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = target.Pop();
																	  }, "Pop was allowed on empty stack");
			this.AddTestValues(target);
			var origCount = target.Count;
			int expected = this._TestValues.First();
			int actual = target.Pop();
			Assert.AreEqual(expected, actual);
			Assert.AreEqual(origCount - 1, target.Count);
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PushTest()
		{
			LinkedStack<int> target = this.CreateWithTestValues();
			var origCount = target.Count;
			int value = 999;
			target.Push(value);
			int top = target.Peek();
			Assert.AreEqual(value, top);
			Assert.AreEqual(origCount + 1, target.Count);
			CollectionAssert.AreEqual(new[] { value }.Concat(_TestValues).ToArray(), target);
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void PushRangeTest()
		{
			LinkedStack<int> target = this.CreateEmpty<int>();
			var testValuesForPush = this.GetTestValuesForPush();
			target.PushRange(testValuesForPush);
			CollectionAssert.AreEqual(_TestValues, target);
			target.PushRange(testValuesForPush);
			Assert.AreEqual(_TestValues.Count * 2, target.Count);
			CollectionAssert.AreEqual(_TestValues.Concat(_TestValues).ToArray(), target);
		}

		//[TestMethod]
		//[TestCategory("LinkedStack")]
		//[DeploymentItem("ani.exe")]
		//public void GetInternalEnumeratorNonEmptyTest()
		//{
		//    LinkedStack<int> target = this.CreateWithTestValues();
		//    IEnumerator<int> internalEnumerator = target.GetEnumeratorInternal();
		//    var iterator = target.GetEnumeratorIterator();
		//    for(; ; )
		//    {
		//        if(!iterator.MoveNext())
		//            break;
		//        if(!internalEnumerator.MoveNext())
		//            break;
		//        Assert.AreEqual(iterator.Current, internalEnumerator.Current);
		//    }
		//}

		[TestMethod]
		[TestCategory("LinkedStack")]
		[DeploymentItem("ani.exe")]
		public void GetEnumeratorNonEmptyTest()
		{
			LinkedStack<int> target = this.CreateWithTestValues();
			IEnumerator<int> enumerator = target.GetEnumerator();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on non-started enumerator");
			List<int> actualContents = new List<int>();
			while(enumerator.MoveNext())
			{
				actualContents.Add(enumerator.Current);
			}
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on finished enumerator");
			List<int> expectedContents = _TestValues.ToList();
			CollectionAssert.AreEqual(expectedContents, actualContents);
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void GetEnumeratorEmpty()
		{
			LinkedStack<int> target = this.CreateEmpty<int>();
			IEnumerator<int> enumerator = target.GetEnumerator();
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
																	  {
																		  var z = enumerator.Current;
																	  }, "Current allowed on non-started enumerator");
			Assert.IsFalse(enumerator.MoveNext());
			TestUtil.AssertThrowsException<InvalidOperationException>(() =>
			                                                          {
			                                                          	var z = enumerator.Current;
			                                                          }, "Current allowed on empty enumerator");
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void TryPeekTest()
		{
			LinkedStack<int> target = this.CreateEmpty<int>();
			int value;
			Assert.IsFalse(target.TryPeek(out value));
			this.AddTestValues(target);
			var origCount = target.Count;
			Assert.IsTrue(target.TryPeek(out value));
			Assert.AreEqual(target.First(), value);
			Assert.AreEqual(_TestValues.First(), value);
			Assert.AreEqual(origCount, target.Count);
		}

		[TestMethod]
		[TestCategory("LinkedStack")]
		public void TryPopTest()
		{
			LinkedStack<int> target = this.CreateEmpty<int>();
			int value;
			Assert.IsFalse(target.TryPop(out value));
			this.AddTestValues(target);
			var origCount = target.Count;
			Assert.IsTrue(target.TryPop(out value));
			Assert.AreEqual(_TestValues.First(), value);
			Assert.AreEqual(origCount - 1, target.Count);
			Assert.AreEqual(_TestValues[1], target.First());
		}

	}

	// ReSharper restore ConvertToConstant.Local
	// ReSharper restore SuggestUseVarKeywordEverywhere
	// ReSharper restore SuggestUseVarKeywordEvident
}
