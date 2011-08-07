using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Animator.Core.Model;
using Animator.Core.Runtime.ObjectStates;
using Animator.UI.Dialogs;

namespace Animator.Test
{

	#region TestWindow1

	/// <summary>
	/// Interaction logic for TestWindow1.xaml
	/// </summary>
	public partial class TestWindow1 : Window
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public TestWindow1()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void attachObjToPropGridButton_Click(object sender, RoutedEventArgs e)
		{
			var obj = new TestObj {Foo = "fooooo", X = 12};
			//propgrid.SelectedObject = obj;
			var state = ObjectState.CreateState(obj);
			propgrid.SelectedObject = state;
		}

		#endregion

	}

	#endregion

	public class TestObj
	{
		[StateProperty]
		public string Foo { get; set; }
		[StateProperty]
		public int X { get; set; }
	}

}
