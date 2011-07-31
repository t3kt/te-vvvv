using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Animator.AppCore.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Animator.Tests.AniApp.Tasks
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
	// ReSharper disable ConditionIsAlwaysTrueOrFalse
	// ReSharper disable RedundantArgumentName
	// ReSharper disable UnusedVariable

	[TestClass]
	public class TaskManagerTest
	{

		[TestMethod]
		[TestCategory(CategoryNames.App_Tasks)]
		public void Manager_PerformButIgnoreNonUndoableCompletedTasks()
		{
			var mgr = new TaskManager();

			var task = new TestTask(capabilities: TaskCapabilities.None) { _PerformResult = TaskResult.Completed };

			var result = mgr.Perform(task);
			Assert.AreEqual(TaskResult.Completed, result);
			Assert.AreEqual(0, mgr.UndoableTaskCount);
			Assert.AreEqual(0, mgr.RedoableTaskCount);
			Assert.AreEqual(TaskState.Performed, task.State);
		}

		[TestMethod]
		[TestCategory(CategoryNames.App_Tasks)]
		public void Manager_PerformButIgnoreNonUndoableFailedOrUnavailableTasks()
		{
			var mgr = new TaskManager();

			var task_a = new TestTask(capabilities: TaskCapabilities.None) { _PerformResult = TaskResult.Failed };

			var result_a = mgr.Perform(task_a);
			Assert.AreEqual(TaskResult.Failed, result_a);
			Assert.AreEqual(0, mgr.UndoableTaskCount);
			Assert.AreEqual(0, mgr.RedoableTaskCount);
			Assert.AreEqual(TaskState.Performed, task_a.State);

			var task_b = new TestTask(capabilities: TaskCapabilities.None) { _PerformResult = TaskResult.NoTask };

			var result_b = mgr.Perform(task_b);
			Assert.AreEqual(TaskResult.NoTask, result_b);
			Assert.AreEqual(0, mgr.UndoableTaskCount);
			Assert.AreEqual(0, mgr.RedoableTaskCount);
			Assert.AreEqual(TaskState.Performed, task_b.State);
		}

	}

	// ReSharper restore UnusedVariable
	// ReSharper restore RedundantArgumentName
	// ReSharper restore ConditionIsAlwaysTrueOrFalse
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
