using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Animator.Test
{

	#region TestCommands

	public static class TestCommands
	{

		public static readonly RoutedUICommand ShowTestWindow = new RoutedUICommand("Show Test Window", "ShowTestWindow", typeof(TestCommands), new InputGestureCollection { new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift) });

		public static readonly RoutedUICommand LoadTestDocument = new RoutedUICommand("Load Test Document", "LoadTestDocument", typeof(TestCommands));

	}

	#endregion

}
