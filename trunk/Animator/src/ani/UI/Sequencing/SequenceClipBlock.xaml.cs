using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Animator.AppCore;

namespace Animator.UI.Sequencing
{
	/// <summary>
	/// Interaction logic for SequenceClipBlock.xaml
	/// </summary>
	public partial class SequenceClipBlock
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public SequenceClipBlock()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			AniAppCommands.EditDetail.Execute(this.DataContext, this);
			base.OnMouseDoubleClick(e);
		}

		#endregion

	}
}
