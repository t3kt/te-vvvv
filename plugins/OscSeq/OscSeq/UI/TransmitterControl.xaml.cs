using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using OscSeq.Core;

namespace OscSeq.UI
{
	public partial class TransmitterControl : UserControl
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Transmitter _Transmitter;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public TransmitterControl()
		{
			_Transmitter = new Transmitter();
			_Transmitter.StatusChanged += this.Transmitter_StatusChanged;
			InitializeComponent();
			this.UpdateStatus();
		}

		#endregion

		#region Methods

		private void UpdateStatus()
		{
			var isConnected = this._Transmitter.IsConnected;
			this.HostTextBox.IsEnabled = this.PortTextBox.IsEnabled = this.ConnectButton.IsEnabled = !isConnected;
			this.DisconnectButton.IsEnabled = isConnected;
			this.StatusTextBox.Text = isConnected ? "Connected" : "Disconnected";
		}

		private void Transmitter_StatusChanged(object sender, EventArgs e)
		{
			this.UpdateStatus();
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_Transmitter.Open(this.HostTextBox.Text, Convert.ToInt32(this.PortTextBox.Text));
			}
			catch(Exception ex)
			{
				MessageBox.Show(String.Format("Connection Error:\n	{0}", ex.Message), "Connection Error", MessageBoxButton.OK);
			}
		}

		private void DisconnectButton_Click(object sender, RoutedEventArgs e)
		{
			_Transmitter.Close();
		}

		#endregion

	}
}
