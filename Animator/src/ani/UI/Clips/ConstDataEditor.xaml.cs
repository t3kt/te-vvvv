using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Core.Model.Clips;
using Animator.Core.Runtime;

namespace Animator.UI.Clips
{
	/// <summary>
	/// Interaction logic for ConstDataEditor.xaml
	/// </summary>
	public partial class ConstDataEditor : IClipPropertyDataEditor
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ConstDataEditor()
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
