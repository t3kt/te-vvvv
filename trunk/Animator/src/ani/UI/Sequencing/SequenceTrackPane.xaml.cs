using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.AppCore;
using Animator.Core.Model.Clips;

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

		#endregion

	}
}
