using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.Model.Clips;
using Animator.Core.Model.Sequences;
using Animator.UI.Clips;
using Animator.UI.Dialogs;
using Animator.UI.Editors;

namespace Animator.UI.Sequencing
{
	/// <summary>
	/// Interaction logic for SequenceTrackPane.xaml
	/// </summary>
	public partial class SequenceTrackPane
	{

		#region Static / Constant

		public static readonly DependencyProperty SelectedClipProperty = AniUI.SelectedClipProperty.AddOwner(typeof(SequenceTrackPane));

		#endregion

		#region Fields

		#endregion

		#region Properties

		public ClipBase SelectedClip
		{
			get { return (ClipBase)this.GetValue(SelectedClipProperty); }
			set { this.SetValue(SelectedClipProperty, value); }
		}

		#endregion

		#region Constructors

		public SequenceTrackPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void Clips_EditDetailCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var clip = e.Parameter as SequenceClip;
			if(clip != null)
			{
				ObjectEditorDialog.ShowForEditor(
					new ClipPropertiesEditor
					{
						Target = clip,
						AutoCommit = true
					}, Window.GetWindow(this));
				e.Handled = true;
			}
		}

		private void Clips_DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var track = this.DataContext as SequenceTrack;
			var clip = e.Parameter as SequenceClip;
			if(track != null && clip != null)
			{
				track.Clips.Remove(clip);
				e.Handled = true;
			}
		}

		#endregion

	}
}
