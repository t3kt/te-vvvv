using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Animator.AppCore
{

	#region AniAppCommands

	public static class AniAppCommands
	{

		public static readonly RoutedUICommand Exit = new RoutedUICommand("_Exit", "Exit", typeof(AniAppCommands),
			new InputGestureCollection { new KeyGesture(Key.F4, ModifierKeys.Alt) });

	}

	#endregion

}
