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

		public static readonly RoutedUICommand DebuggerBreak = new RoutedUICommand("Debugger Break", "DebuggerBreak", typeof(TestCommands));

		public static readonly RoutedUICommand ShowRecentFilesInfo = new RoutedUICommand("Recent Files Info", "ShowRecentFilesInfo", typeof(TestCommands));

		internal static void DoShowRecentFilesInfo(MainWindow mainWindow)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("Config path: '{0}'", ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath).AppendLine();
			sb.AppendFormat("Capacity: {0}", ((AniApplication)Application.Current).RecentFileManager.Capacity).AppendLine();
			sb.AppendLine("Files:");
			var files = mainWindow.RecentFiles;
			foreach(var file in files)
				sb.AppendLine(file);
			MessageBox.Show(mainWindow, sb.ToString(), "Recent Files Info", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		internal static void DoDebugBreak()
		{
			//if(System.Diagnostics.Debugger.IsAttached)
			System.Diagnostics.Debugger.Break();
		}

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
