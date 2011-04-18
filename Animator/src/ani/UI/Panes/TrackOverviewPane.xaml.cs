using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.Core.Model;
using Animator.UI.Dialogs;

namespace Animator.UI.Panes
{

	#region TrackOverviewPane

	/// <summary>
	/// Interaction logic for TrackOverviewPane.xaml
	/// </summary>
	public partial class TrackOverviewPane
	{

		#region Static / Constant

		public static readonly DependencyProperty SelectedClipProperty;
		public static readonly RoutedEvent ClipSelectedEvent;

		static TrackOverviewPane()
		{
			SelectedClipProperty = DependencyProperty.Register("SelectedClip", typeof(Clip), typeof(TrackOverviewPane),
				new PropertyMetadata(null, SelectedClip_Changed));
			ClipSelectedEvent = EventManager.RegisterRoutedEvent("ClipSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
				typeof(TrackOverviewPane));
		}

		private static void SelectedClip_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var pane = d as TrackOverviewPane;
			if(pane != null)
			{
				pane.RaiseEvent(new RoutedEventArgs(ClipSelectedEvent, pane));
			}
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Track Track
		{
			get { return (Track)this.DataContext; }
			set { this.DataContext = value; }
		}

		public Clip SelectedClip
		{
			get { return (Clip)this.GetValue(SelectedClipProperty); }
			set { this.SetValue(SelectedClipProperty, value); }
		}

		#endregion

		#region Events

		public event RoutedEventHandler ClipSelected
		{
			add { this.AddHandler(ClipSelectedEvent, value); }
			remove { this.RemoveHandler(ClipSelectedEvent, value); }
		}

		#endregion

		#region Constructors

		public TrackOverviewPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void ShowEditDetailCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
		{
			var window = new TrackPropertiesWindow
						 {
							 DataContext = this.Track
						 };
			window.ShowDialog();
		}

		#endregion

	}

	#endregion

}
