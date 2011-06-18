using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Animator.AppCore
{

	#region AniAppCommands

	internal static class AniAppCommands
	{

		public static readonly RoutedUICommand Exit = new RoutedUICommand("_Exit", "Exit", typeof(AniAppCommands),
			new InputGestureCollection { new KeyGesture(Key.F4, ModifierKeys.Alt) });

		public static readonly RoutedUICommand EditDetail = new RoutedUICommand("Edit", "EditDetail", typeof(AniAppCommands));

		public static readonly RoutedUICommand AddOutput = new RoutedUICommand("Add _Output", "AddOutput", typeof(AniAppCommands));

		public static readonly RoutedUICommand EditTransport = new RoutedUICommand("Edit _Transport", "EditTransport", typeof(AniAppCommands),
			new InputGestureCollection { new KeyGesture(Key.T, ModifierKeys.Control) });

		public static readonly RoutedUICommand AboutApplication = new RoutedUICommand("_About Application", "AboutApplication", typeof(AniAppCommands));

	}

	#endregion

}
