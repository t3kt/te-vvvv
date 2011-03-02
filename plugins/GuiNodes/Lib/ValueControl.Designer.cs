namespace VVVV.Lib
{
	partial class ValueControl
	{
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
			this._ValueBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// _ValueBox
			// 
			this._ValueBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._ValueBox.Location = new System.Drawing.Point(0, 0);
			this._ValueBox.Name = "_ValueBox";
			this._ValueBox.Size = new System.Drawing.Size(40, 20);
			this._ValueBox.TabIndex = 1;
			this._ValueBox.Visible = false;
			// 
			// ValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.Controls.Add(this._ValueBox);
			this.DoubleBuffered = true;
			this.Name = "ValueControl";
			this.Size = new System.Drawing.Size(40, 13);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _ValueBox;
	}
}
