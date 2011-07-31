using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.AppCore.Tasks;
using Animator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.AniApp.Tasks
{
	[TestClass]
	public class LockingTaskStackTest
	{
		[TestMethod]
		[TestCategory(CategoryNames.App_Tasks)]
		public void Stack_BasicAddAndGetSnapshot()
		{
			var stack_changeCounter = new EventCounter();
			var stack = new LockingTaskStack(null);
			stack.Changed += stack_changeCounter.Handler;

			var task_a = new TestTask();
			stack.Push(task_a);
			Assert.AreEqual(1, stack.Count);
			Assert.AreEqual(1, stack_changeCounter.Count);

			var snapshot_1 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_a }, snapshot_1);

			Assert.AreEqual(1, stack_changeCounter.Count);

			var task_b = new TestTask();
			stack.Push(task_b);
			Assert.AreEqual(2, stack.Count);
			Assert.AreEqual(2, stack_changeCounter.Count);

			var snapshot_2 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_b, task_a }, snapshot_2);
		}

		[TestMethod]
		[TestCategory(CategoryNames.App_Tasks)]
		public void Stack_ClearAt_Middle()
		{
			var counter = new EventCounter();
			var stack = new LockingTaskStack(null);
			stack.Changed += counter.Handler;

			var task_a = new TestTask();
			var task_b = new TestTask();
			var task_c = new TestTask();
			var task_d = new TestTask();
			var task_e = new TestTask();

			stack.Push(task_a);
			stack.Push(task_b);
			stack.Push(task_c);
			stack.Push(task_d);
			stack.Push(task_e);

			Assert.AreEqual(5, stack.Count);
			TestUtil.AssertCounterEquals(5, counter, reset: true);

			var snapshot_1 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_e, task_d, task_c, task_b, task_a }, snapshot_1);
			TestUtil.AssertCounterEquals(0, counter);

			stack.ClearAt(task_c);
			TestUtil.AssertCounterEquals(1, counter, reset: true);
			var snapshot_2 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_e, task_d }, snapshot_2);
		}

		[TestMethod]
		[TestCategory(CategoryNames.App_Tasks)]
		public void Stack_ClearAt_Top()
		{
			var counter = new EventCounter();
			var stack = new LockingTaskStack(null);
			stack.Changed += counter.Handler;

			var task_a = new TestTask();
			var task_b = new TestTask();
			var task_c = new TestTask();
			var task_d = new TestTask();
			var task_e = new TestTask();

			var task_z = new TestTask();

			stack.Push(task_a);
			stack.Push(task_b);
			stack.Push(task_c);
			stack.Push(task_d);
			stack.Push(task_e);

			Assert.AreEqual(5, stack.Count);
			TestUtil.AssertCounterEquals(5, counter, reset: true);

			var snapshot_1 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_e, task_d, task_c, task_b, task_a }, snapshot_1);
			TestUtil.AssertCounterEquals(0, counter);

			stack.ClearAt(task_e);
			TestUtil.AssertCounterEquals(1, counter, reset: true);
			var snapshot_2 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { }, snapshot_2);
		}

		[TestMethod]
		[TestCategory(CategoryNames.App_Tasks)]
		public void Stack_ClearAt_Bottom()
		{
			var counter = new EventCounter();
			var stack = new LockingTaskStack(null);
			stack.Changed += counter.Handler;

			var task_a = new TestTask();
			var task_b = new TestTask();
			var task_c = new TestTask();
			var task_d = new TestTask();
			var task_e = new TestTask();

			stack.Push(task_a);
			stack.Push(task_b);
			stack.Push(task_c);
			stack.Push(task_d);
			stack.Push(task_e);

			Assert.AreEqual(5, stack.Count);
			TestUtil.AssertCounterEquals(5, counter, reset: true);

			var snapshot_1 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_e, task_d, task_c, task_b, task_a }, snapshot_1);
			TestUtil.AssertCounterEquals(0, counter);

			stack.ClearAt(task_a);
			TestUtil.AssertCounterEquals(1, counter, reset: true);
			var snapshot_2 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_e, task_d, task_c, task_b }, snapshot_2);
		}

		[TestMethod]
		[TestCategory(CategoryNames.App_Tasks)]
		public void Stack_ClearAt_NotInStack()
		{
			var counter = new EventCounter();
			var stack = new LockingTaskStack(null);
			stack.Changed += counter.Handler;

			var task_a = new TestTask();
			var task_b = new TestTask();
			var task_c = new TestTask();
			var task_d = new TestTask();
			var task_e = new TestTask();

			var task_z = new TestTask();

			stack.Push(task_a);
			stack.Push(task_b);
			stack.Push(task_c);
			stack.Push(task_d);
			stack.Push(task_e);

			Assert.AreEqual(5, stack.Count);
			TestUtil.AssertCounterEquals(5, counter, reset: true);

			var snapshot_1 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_e, task_d, task_c, task_b, task_a }, snapshot_1);
			TestUtil.AssertCounterEquals(0, counter);

			stack.ClearAt(task_z);
			TestUtil.AssertCounterEquals(0, counter, reset: true);
			var snapshot_2 = stack.GetSnapshot();
			CollectionAssert.AreEqual(new List<TaskBase> { task_e, task_d, task_c, task_b, task_a }, snapshot_2);
		}

	}
}
