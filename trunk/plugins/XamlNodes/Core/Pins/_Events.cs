using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace XamlNodes.Core.Pins
{

	#region XamlNodePinValueChangedEventHandler

	public delegate void XamlNodePinValueChangedEventHandler(object sender, XamlNodePinValueChangedEventArgs e);

	#endregion

	#region XamlNodePinValueChangedEventArgs

	public sealed class XamlNodePinValueChangedEventArgs : RoutedEventArgs
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly XamlNodePin _Pin;

		#endregion

		#region Properties

		public XamlNodePin Pin
		{
			get { return this._Pin; }
		}

		#endregion

		#region Constructors

		public XamlNodePinValueChangedEventArgs(RoutedEvent routedEvent, XamlNodePin pin)
			: base(routedEvent)
		{
			this._Pin = pin;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
