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

namespace Animator.UI.Editors
{

	#region StepListEditor

	/// <summary>
	/// Interaction logic for StepListEditor.xaml
	/// </summary>
	public partial class StepListEditor : UserControl
	{

		#region Static / Constant

		private static readonly List<float> _DebugSteps = new List<float> { 0.5f, 1.0f, 0.0f, -1.0f };

		#endregion

		#region Fields

		#endregion

		#region Properties

		public List<float> DebugSteps
		{
			get { return _DebugSteps; }
		}

		#endregion

		#region Constructors

		public StepListEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
