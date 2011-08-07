using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Core.Transport;

namespace Animator.UI.Panes
{

	#region TransportPane

	/// <summary>
	/// Interaction logic for TransportPane.xaml
	/// </summary>
	public partial class TransportPane
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public ITransportController TransportController
		{
			get { return AniApplication.GetAppService<ITransportController>(); }
		}

		#endregion

		#region Constructors

		public TransportPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
