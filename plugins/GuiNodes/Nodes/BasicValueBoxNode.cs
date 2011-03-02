using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
	[PluginInfo(Name = "BasicValueBox",
		Category = "GUI",
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		InitialComponentMode = TComponentMode.InABox,
		InitialBoxHeight = 20,
		InitialWindowHeight = 20,
		InitialBoxWidth = 80,
		InitialWindowWidth = 80)]
	public partial class BasicValueBoxNode : UserControl, IPluginEvaluate, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input", IsSingle = true)]
		private IDiffSpread<double> _Input;

		[Output("Output", IsSingle = true)]
		private ISpread<double> _Output;

		private bool _Invalidate = true;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public BasicValueBoxNode()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void Input_Changed(IDiffSpread<double> spread)
		{
			_ValueUpDown.Value = (decimal)spread[0];
			_Invalidate = true;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
					components.Dispose();
				if(_Input != null)
					_Input.Changed -= this.Input_Changed;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_Input.Changed += this.Input_Changed;
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_Invalidate)
			{
				_Output[0] = (double)_ValueUpDown.Value;
				_Invalidate = false;
			}
		}

		#endregion

	}
}
