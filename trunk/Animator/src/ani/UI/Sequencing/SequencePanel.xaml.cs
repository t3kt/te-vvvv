using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Animator.AppCore;
using Animator.AppCore.Common;
using Animator.Core.Model.Sequences;

namespace Animator.UI.Sequencing
{
	/// <summary>
	/// Interaction logic for SequencePanel.xaml
	/// </summary>
	public partial class SequencePanel
	{

		#region Static / Constant

		public static readonly DependencyProperty UnitsPerSecondProperty =
			DependencyProperty.Register("UnitsPerSecond", typeof(double), typeof(SequencePanel),
				new FrameworkPropertyMetadata(0.0,
					FrameworkPropertyMetadataOptions.AffectsArrange |
					FrameworkPropertyMetadataOptions.AffectsMeasure |
					FrameworkPropertyMetadataOptions.Inherits));

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Sequence Sequence
		{
			get { return this.DataContext as Sequence; }
			set { this.DataContext = value; }
		}

		public double UnitsPerSecond
		{
			get { return (double)this.GetValue(UnitsPerSecondProperty); }
			set { this.SetValue(UnitsPerSecondProperty, value); }
		}

		#endregion

		#region Constructors

		public SequencePanel()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void Tracks_DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var sequence = this.DataContext as Sequence;
			var track = e.Parameter as SequenceTrack;
			if(sequence != null && track != null)
				sequence.Tracks.Remove(track);
			e.Handled = true;
		}

		#endregion

	}
}
