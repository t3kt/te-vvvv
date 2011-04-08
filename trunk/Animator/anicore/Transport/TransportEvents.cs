using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Animator.Core.Transport
{

	#region TransportEventHandler

	public delegate void TransportEventHandler(object sender, TransportContextEventArgs e);

	#endregion

	#region TransportContextEventArgs

	public class TransportContextEventArgs : RoutedEventArgs
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly TransportContext _Context;

		#endregion

		#region Properties

		public TransportContext Context
		{
			get { return _Context; }
		}

		#endregion

		#region Constructors

		public TransportContextEventArgs(TransportContext context)
		{
			if(context == null)
				throw new ArgumentNullException("context");
			_Context = context;
		}

		#endregion

		#region Methods

		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			var handler = (TransportEventHandler)genericHandler;
			handler(genericTarget, this);
		}

		#endregion

	}

	#endregion

}
