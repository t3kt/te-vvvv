using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Animator.UI;

namespace Animator.Test
{

	#region TestCommands

	public static class TestCommands
	{

		public static readonly RoutedUICommand ShowTestWindow = new RoutedUICommand("Show Test Window", "ShowTestWindow", typeof(TestCommands), new InputGestureCollection { new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift) });

		private static TestWindow1 _TestWindow;

		internal static void DoShowTestWindow(MainWindow mainWindow)
		{
			if(_TestWindow == null)
				_TestWindow = new TestWindow1 { Owner = mainWindow };
			_TestWindow.Show();
		}

	}

	#endregion

}
