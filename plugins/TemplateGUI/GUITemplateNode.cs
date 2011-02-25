using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "zzzzzzTemplate",
	Category = "GUI",
	Help = "Template with some gui elements",
	Tags = "",
	AutoEvaluate = true)]
	#endregion PluginInfo
	public class GUITemplateNode : UserControl, IPluginEvaluate
	{
		#region fields & pins
		
		[Output("Button")]
		ISpread<bool> FButtonOut;
		
		[Output("Check Box")]
		ISpread<bool> FCheckBoxOut;
		
		[Output("Text Box")]
		ISpread<string> FTextBoxOut;
		
		[Output("Slider")]
		ISpread<double> FSliderOut;

		//gui controls
		private bool FButtonClick;
		private CheckBox FCheckBox;
		private TextBox FTextBox;
		private Button button;
		private TrackBar FSlider;
		
		#endregion fields & pins
		
		#region constructor and init
		
		public GUITemplateNode()
		{
			//setup the gui
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			this.button = new System.Windows.Forms.Button();
			this.FCheckBox = new System.Windows.Forms.CheckBox();
			this.FTextBox = new System.Windows.Forms.TextBox();
			this.FSlider = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.FSlider)).BeginInit();
			this.SuspendLayout();
			// 
			// button
			// 
			this.button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button.Location = new System.Drawing.Point(10, 10);
			this.button.Name = "button";
			this.button.Size = new System.Drawing.Size(100, 25);
			this.button.TabIndex = 0;
			this.button.Text = "Bang";
			this.button.Click += new System.EventHandler(this.button_Click);
			// 
			// FCheckBox
			// 
			this.FCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.FCheckBox.Location = new System.Drawing.Point(15, 50);
			this.FCheckBox.Name = "FCheckBox";
			this.FCheckBox.Size = new System.Drawing.Size(100, 25);
			this.FCheckBox.TabIndex = 1;
			this.FCheckBox.Text = "Toggle";
			// 
			// FTextBox
			// 
			this.FTextBox.Location = new System.Drawing.Point(130, 10);
			this.FTextBox.Name = "FTextBox";
			this.FTextBox.Size = new System.Drawing.Size(150, 20);
			this.FTextBox.TabIndex = 2;
			// 
			// FSlider
			// 
			this.FSlider.Location = new System.Drawing.Point(125, 50);
			this.FSlider.Maximum = 1000;
			this.FSlider.Name = "FSlider";
			this.FSlider.Size = new System.Drawing.Size(160, 45);
			this.FSlider.TabIndex = 3;
			this.FSlider.TickFrequency = 20;
			// 
			// GUITemplateNode
			// 
			this.Controls.Add(this.button);
			this.Controls.Add(this.FCheckBox);
			this.Controls.Add(this.FTextBox);
			this.Controls.Add(this.FSlider);
			this.Name = "GUITemplateNode";
			this.Size = new System.Drawing.Size(311, 111);
			((System.ComponentModel.ISupportInitialize)(this.FSlider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		
		#endregion constructor and init
		
		//called when data for any output pin is requested
		public void Evaluate(int spreadMax)
		{
			//set outputs
			FButtonOut[0] = FButtonClick;
			FButtonClick = false;
			
			FCheckBoxOut[0] = FCheckBox.Checked;
			
			FTextBoxOut[0] = FTextBox.Text;
			
			FSliderOut[0] = FSlider.Value / 1000.0;
		}

		private void button_Click(object sender, EventArgs e)
		{
			if(sender is Button)
			{
				//set button click to true to read it in Evaluate()
				FButtonClick = true;
			}
		}
	}
}