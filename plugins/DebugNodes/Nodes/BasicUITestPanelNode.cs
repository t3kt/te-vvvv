using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
	[PluginInfo(Name = "BasicUITestPanel",
		Category = TEShared.Names.Categories.Debug,
		Author = TEShared.Names.Author,
		//AutoEvaluate = true,
		InitialBoxHeight = 150,
		InitialWindowHeight = 150, 
		InitialBoxWidth = 150,
		InitialWindowWidth = 150,
		InitialComponentMode = TComponentMode.InABox)]
	public class BasicUITestPanelNode : UserControl, IPluginEvaluate
	{
		public BasicUITestPanelNode()
		{
			InitializeComponent();
		}

		private TextBox nameTextBox;
		private Button sayHelloButton;

		private void sayHelloButton_Click(object sender, EventArgs e)
		{
			MessageBox.Show(this, String.Format("Hello {0}", this.nameTextBox.Text));
		}

		#region Designer stuff


		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.Label label1;
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.sayHelloButton = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			label1.Location = new System.Drawing.Point(56, 20);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(33, 15);
			label1.TabIndex = 0;
			label1.Text = "Hello";
			// 
			// nameTextBox
			// 
			this.nameTextBox.Location = new System.Drawing.Point(33, 97);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(100, 20);
			this.nameTextBox.TabIndex = 1;
			// 
			// sayHelloButton
			// 
			this.sayHelloButton.Location = new System.Drawing.Point(44, 60);
			this.sayHelloButton.Name = "sayHelloButton";
			this.sayHelloButton.Size = new System.Drawing.Size(75, 23);
			this.sayHelloButton.TabIndex = 2;
			this.sayHelloButton.Text = "Say Hello";
			this.sayHelloButton.UseVisualStyleBackColor = true;
			this.sayHelloButton.Click += new System.EventHandler(this.sayHelloButton_Click);
			// 
			// BasicUITestPanelNode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.sayHelloButton);
			this.Controls.Add(this.nameTextBox);
			this.Controls.Add(label1);
			this.DoubleBuffered = true;
			this.Name = "BasicUITestPanelNode";
			this.Size = new System.Drawing.Size(148, 148);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion


		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
		}

		#endregion

	}
}
