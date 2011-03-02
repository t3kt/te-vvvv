namespace VVVV.Nodes
{
	partial class BasicValueBoxNode
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._ValueUpDown = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this._ValueUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// _ValueUpDown
			// 
			this._ValueUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this._ValueUpDown.Location = new System.Drawing.Point(0, 0);
			this._ValueUpDown.Name = "_ValueUpDown";
			this._ValueUpDown.Size = new System.Drawing.Size(80, 20);
			this._ValueUpDown.TabIndex = 0;
			// 
			// BasicValueBoxNode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._ValueUpDown);
			this.Name = "BasicValueBoxNode";
			this.Size = new System.Drawing.Size(80, 20);
			((System.ComponentModel.ISupportInitialize)(this._ValueUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NumericUpDown _ValueUpDown;
	}
}
