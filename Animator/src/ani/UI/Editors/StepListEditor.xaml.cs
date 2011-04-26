using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Core.Runtime;

namespace Animator.UI.Editors
{

	#region StepListEditor

	/// <summary>
	/// Interaction logic for StepListEditor.xaml
	/// </summary>
	public partial class StepListEditor : IClipDataEditor
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public StepListEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

		#region IClipDataEditor Members

		public new Core.Model.Clip Clip
		{
			get { return this.DataContext as Core.Model.Clip; }
			set { this.DataContext = value; }
		}

		#endregion

	}

	#endregion

}
