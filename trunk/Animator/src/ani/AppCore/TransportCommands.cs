using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Animator.AppCore
{

	#region TransportCommands

	internal static class TransportCommands
	{

		public static readonly RoutedUICommand Play = new RoutedUICommand("Play", "Play", typeof(TransportCommands));

		public static readonly RoutedUICommand Pause = new RoutedUICommand("Pause", "Pause", typeof(TransportCommands));

		public static readonly RoutedUICommand Stop= new RoutedUICommand("Stop", "Stop", typeof(TransportCommands));

		public static readonly RoutedUICommand Reset= new RoutedUICommand("Reset", "Reset", typeof(TransportCommands));

	}

	#endregion

}
