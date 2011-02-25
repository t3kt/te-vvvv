namespace VVVV.UI
{
	partial class GlobalStructTypesPanel
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
			System.Windows.Forms.DataGridViewTextBoxColumn FriendlyTypeNameColumn;
			System.Windows.Forms.DataGridViewTextBoxColumn PartTypes;
			System.Windows.Forms.DataGridViewTextBoxColumn Id;
			System.Windows.Forms.DataGridViewTextBoxColumn UsageCount;
			this._TypesDataGridView = new System.Windows.Forms.DataGridView();
			FriendlyTypeNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			PartTypes = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			UsageCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this._TypesDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// _TypesDataGridView
			// 
			this._TypesDataGridView.AllowUserToAddRows = false;
			this._TypesDataGridView.AllowUserToDeleteRows = false;
			this._TypesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._TypesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            FriendlyTypeNameColumn,
            PartTypes,
            Id,
            UsageCount});
			this._TypesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._TypesDataGridView.Location = new System.Drawing.Point(0, 0);
			this._TypesDataGridView.Name = "_TypesDataGridView";
			this._TypesDataGridView.ReadOnly = true;
			this._TypesDataGridView.RowHeadersVisible = false;
			this._TypesDataGridView.Size = new System.Drawing.Size(377, 256);
			this._TypesDataGridView.TabIndex = 0;
			// 
			// FriendlyTypeNameColumn
			// 
			FriendlyTypeNameColumn.DataPropertyName = "FriendlyTypeName";
			FriendlyTypeNameColumn.HeaderText = "Type";
			FriendlyTypeNameColumn.Name = "FriendlyTypeNameColumn";
			FriendlyTypeNameColumn.ReadOnly = true;
			// 
			// PartTypes
			// 
			PartTypes.DataPropertyName = "PartTypesKey";
			PartTypes.HeaderText = "PartsKey";
			PartTypes.Name = "PartTypes";
			PartTypes.ReadOnly = true;
			PartTypes.Width = 60;
			// 
			// Id
			// 
			Id.DataPropertyName = "Id";
			Id.HeaderText = "Id";
			Id.Name = "Id";
			Id.ReadOnly = true;
			Id.Width = 150;
			// 
			// UsageCount
			// 
			UsageCount.HeaderText = "Usages";
			UsageCount.Name = "UsageCount";
			UsageCount.ReadOnly = true;
			UsageCount.Width = 60;
			// 
			// GlobalStructTypesPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._TypesDataGridView);
			this.Name = "GlobalStructTypesPanel";
			this.Size = new System.Drawing.Size(377, 256);
			((System.ComponentModel.ISupportInitialize)(this._TypesDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView _TypesDataGridView;

	}
}
