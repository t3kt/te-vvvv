using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Animator.Core.Model.Clips;

namespace Animator.UI.Clips
{
	/// <summary>
	/// Interaction logic for StepDataEditor.xaml
	/// </summary>
	public partial class StepDataEditor
	{

		#region Static / Constant

		public static readonly DependencyProperty StepCountProperty =
			DependencyProperty.Register("StepCount", typeof(int), typeof(StepDataEditor),
				new PropertyMetadata(1, OnStepCountChanged));

		private static void OnStepCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var stepEditor = (StepDataEditor)d;
			var data = stepEditor.DataContext as StepData;
			var count = (int)e.NewValue;
			if(count <= 0 || data == null)
				return;
			data.SetCount(count);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public int StepCount
		{
			get { return (int)this.GetValue(StepCountProperty); }
			set { this.SetValue(StepCountProperty, value); }
		}

		#endregion

		#region Constructors

		public StepDataEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void AddStepButton_Click(object sender, RoutedEventArgs e)
		{
			var data = this.DataContext as StepData;
			if(data != null)
				data.Resize(1);
		}

		private void RemoveStepButton_Click(object sender, RoutedEventArgs e)
		{
			var data = this.DataContext as StepData;
			if(data != null)
				data.Resize(-1);
		}

		#endregion
	}
}
