using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Core.Model.Clips;

namespace Animator.UI.Clips
{
	/// <summary>
	/// Interaction logic for StepDataEditor.xaml
	/// </summary>
	public partial class StepDataEditor
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public StepDataEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

		#region IClipPropertyDataEditor Members

		public ClipPropertyData ClipPropertyData
		{
			get { return this.DataContext as ClipPropertyData; }
			set { this.DataContext = value; }
		}

		#endregion

	}
}
