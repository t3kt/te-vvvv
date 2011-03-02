namespace VVVV.Nodes
{
	partial class MultiStringEditorTestNode
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
			this._ElementHost = new System.Windows.Forms.Integration.ElementHost();
			this.multiStringEditor1 = new VVVV.UI.MultiStringEditor();
			this.SuspendLayout();
			// 
			// _ElementHost
			// 
			this._ElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this._ElementHost.Location = new System.Drawing.Point(0, 0);
			this._ElementHost.Name = "_ElementHost";
			this._ElementHost.Size = new System.Drawing.Size(150, 150);
			this._ElementHost.TabIndex = 0;
			this._ElementHost.Text = "elementHost1";
			this._ElementHost.Child = this.multiStringEditor1;
			// 
			// MultiStringEditorTestNode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._ElementHost);
			this.Name = "MultiStringEditorTestNode";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Integration.ElementHost _ElementHost;
		private UI.MultiStringEditor multiStringEditor1;
	}
}
