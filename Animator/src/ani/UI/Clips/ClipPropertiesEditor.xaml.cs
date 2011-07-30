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

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public IEnumerable<KeyValuePair<string, string>> ClipPropertyDataTypeDescriptions
		{
			get { return AniApplication.CurrentHost.GetClipPropertyDataTypeDescriptionsByKey(); }
		}

		#endregion

		#region Constructors

		public ClipPropertiesEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

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

		private void AddItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var clip = this.DataContext as ClipBase;
			if(clip == null)
				return;
			var typeKey = (string)e.Parameter;
			if(typeKey == null)
				return;
			var prop = AniApplication.CurrentHost.CreateClipPropertyDataByKey(typeKey);
			if(prop == null)
				return;
			clip.Properties.Add(prop);
			e.Handled = true;
		}

		#endregion

	}
}
