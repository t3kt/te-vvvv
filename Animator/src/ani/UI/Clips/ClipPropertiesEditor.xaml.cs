using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Animator.Core.Model.Clips;

namespace Animator.UI.Clips
{
	/// <summary>
	/// Interaction logic for ClipPropertiesEditor.xaml
	/// </summary>
	public partial class ClipPropertiesEditor
	{
		public ClipPropertiesEditor()
		{
			InitializeComponent();
		}

		private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var clip = this.DataContext as ClipBase;
			if(clip != null)
			{
				var prop = e.Parameter as ClipPropertyData;
				if(prop != null && clip.Properties.Remove(prop))
					e.Handled = true;
			}
		}

	}
}
