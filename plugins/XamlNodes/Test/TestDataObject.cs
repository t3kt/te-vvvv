using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using VVVV.PluginInterfaces.V1;
using XamlNodes.Core;

namespace XamlNodes.Test
{

	#region TestDataObject

	public class TestDataObject : FrameworkElement, INotifyPropertyChanged
	{

		#region Static / Constant

		public static readonly DependencyProperty StrProperty =
			DependencyProperty.Register("Str", typeof(string), typeof(TestDataObject), new UIPropertyMetadata(String.Empty, Str_Changed));

		private static void Str_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var o = d as TestDataObject;
			if(o != null)
			{
				var handler = o.PropertyChanged;
				if(handler != null)
					handler(o, new PropertyChangedEventArgs("Str"));
			}
			Debug.WriteLine(String.Format("TestDataObject Str_Changed value: '{0}'", e.NewValue));
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public string Str
		{
			get { return (string)GetValue(StrProperty); }
			set
			{
				if(this.Node != null)
					this.Node.Log(TLogType.Debug, "TestDataObject set Str value: '{0}'", value);
				Debug.WriteLine("TestDataObject set Str value: '{0}'", value);
				SetValue(StrProperty, value);
			}
		}

		internal XamlNodeBase Node { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

	}

	#endregion

}
