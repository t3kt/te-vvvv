using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region BasicValueBox2Node

	[PluginInfo(Name = "BasicValueBox2",
		Category = TEShared.Names.Categories.GUI,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		InitialComponentMode = TComponentMode.InABox,
		InitialBoxHeight = 20,
		InitialWindowHeight = 20,
		InitialBoxWidth = 80,
		InitialWindowWidth = 80)]
	public class BasicValueBox2Node : NumericUpDown, IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Input", IsSingle = true)]
		private IDiffSpread<double> _Input;

		[Output("Output", IsSingle = true)]
		private ISpread<double> _Output;

		private bool _Invalidate;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override void OnValueChanged(EventArgs e)
		{
			_Invalidate = true;
			base.OnValueChanged(e);
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_Input.IsChanged)
			{
				this.Value = (decimal)_Input[0];
			}
			if(_Invalidate)
			{
				_Output[0] = (double)this.Value;
				_Invalidate = false;
			}
		}

		#endregion

	}

	#endregion

}
