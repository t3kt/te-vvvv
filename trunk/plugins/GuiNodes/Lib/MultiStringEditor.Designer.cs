namespace VVVV.Lib
{
	partial class MultiStringEditor
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
			this._Table = new System.Windows.Forms.TableLayoutPanel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this._Table.SuspendLayout();
			this.SuspendLayout();
			// 
			// _Table
			// 
			this._Table.AutoSize = true;
			this._Table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._Table.ColumnCount = 2;
			this._Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._Table.Controls.Add(this.textBox4, 1, 1);
			this._Table.Controls.Add(this.textBox3, 0, 1);
			this._Table.Controls.Add(this.textBox2, 1, 0);
			this._Table.Controls.Add(this.textBox1, 0, 0);
			this._Table.Dock = System.Windows.Forms.DockStyle.Fill;
			this._Table.Location = new System.Drawing.Point(0, 0);
			this._Table.Name = "_Table";
			this._Table.RowCount = 2;
			this._Table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._Table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._Table.Size = new System.Drawing.Size(342, 52);
			this._Table.TabIndex = 0;
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(3, 3);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(165, 20);
			this.textBox1.TabIndex = 0;
			// 
			// textBox2
			// 
			this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox2.Location = new System.Drawing.Point(174, 3);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(165, 20);
			this.textBox2.TabIndex = 1;
			// 
			// textBox3
			// 
			this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox3.Location = new System.Drawing.Point(3, 29);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(165, 20);
			this.textBox3.TabIndex = 2;
			// 
			// textBox4
			// 
			this.textBox4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox4.Location = new System.Drawing.Point(174, 29);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(165, 20);
			this.textBox4.TabIndex = 3;
			// 
			// MultiStringEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this._Table);
			this.Name = "MultiStringEditor";
			this.Size = new System.Drawing.Size(342, 52);
			this._Table.ResumeLayout(false);
			this._Table.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _Table;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox2;
	}
}
