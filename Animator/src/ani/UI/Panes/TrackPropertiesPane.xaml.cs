using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Animator.Core.Model;

namespace Animator.UI.Panes
{

	#region TrackPropertiesPane

	/// <summary>
	/// Interaction logic for TrackPropertiesPane.xaml
	/// </summary>
	public partial class TrackPropertiesPane
	{

		#region Static / Constant

		public static readonly DependencyProperty AvailableOutputsProperty;

		static TrackPropertiesPane()
		{
			AvailableOutputsProperty = DependencyProperty.Register("AvailableOutputs", typeof(ICollection<Output>), typeof(TrackPropertiesPane));
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

		public ICollection<Output> AvailableOutputs
		{
			get { return (ICollection<Output>)this.GetValue(AvailableOutputsProperty); }
			set { this.SetValue(AvailableOutputsProperty, value); }
		}

		#endregion

		#region Constructors

		public TrackPropertiesPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
