using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.WpfPropertyGrid;

namespace Animator.UI.Editors
{
	/// <summary>
	/// Interaction logic for PropertyGridObjectEditor.xaml
	/// </summary>
	public partial class PropertyGridObjectEditor
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public PropertyGrid ObjectPropertyGrid
		{
			get { return this.objectPropertyGrid; }
			set { this.objectPropertyGrid = value; }
		}

		#endregion

		#region Events

		#endregion

		#region Constructors

		public PropertyGridObjectEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

	}
}
