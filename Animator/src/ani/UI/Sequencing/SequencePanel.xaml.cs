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
using Animator.Core.Model.Sequences;

namespace Animator.UI.Sequencing
{
	/// <summary>
	/// Interaction logic for SequencePanel.xaml
	/// </summary>
	public partial class SequencePanel
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Sequence Sequence
		{
			get { return this.DataContext as Sequence; }
			set { this.DataContext = value; }
		}

		#endregion

		#region Constructors

		public SequencePanel()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

	}
}
