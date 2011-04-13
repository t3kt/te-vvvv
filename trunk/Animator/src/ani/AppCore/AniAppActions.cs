using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Animator.AppCore
{

	#region AniAppActions

	internal static class AniAppActions
	{

		public static readonly IAppAction TextBox_TextChange = new DependencyPropertyChangeAppAction(TextBox.TextProperty);

		public static readonly IAppAction ComboBox_TextChange = new DependencyPropertyChangeAppAction(ComboBox.TextProperty);

		public static readonly IAppAction Selector_SelectedValueChange = new DependencyPropertyChangeAppAction(Selector.SelectedValueProperty);

	}

	#endregion

}
