using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VVVV.Lib
{
	public partial class MultiStringEditor : UserControl
	{

		#region Static/Constant

		#endregion

		#region Fields

		private int _Rows = 1;
		private int _Columns = 1;
		private readonly List<TextBox> _TextBoxes = new List<TextBox>();

		#endregion

		#region Properties

		public int Rows
		{
			get { return this._Rows; }
			set
			{
				if(value < 0)
					throw new ArgumentOutOfRangeException();
				if(value != this._Rows)
				{
					this._Rows = value;
					ResizeTable();
				}
			}
		}

		public int Columns
		{
			get { return this._Columns; }
			set
			{
				if(value < 0)
					throw new ArgumentOutOfRangeException();
				if(value != this._Columns)
				{
					this._Columns = value;
					ResizeTable();
				}
			}
		}

		public int TextBoxCount
		{
			get { return this.Rows * this.Columns; }
		}

		#endregion

		#region Constructors

		public MultiStringEditor()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private TextBox CreateTextBox()
		{
			return new TextBox
				   {
					   Dock = DockStyle.Fill
				   };
		}

		private void UpdateColumnStyles()
		{
			var width = 100F / this.Columns;
			while(_Table.ColumnStyles.Count > this.Columns)
				_Table.ColumnStyles.RemoveAt(_Table.ColumnStyles.Count - 1);
			while(_Table.ColumnStyles.Count < this.Columns)
				_Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, width));
			foreach(ColumnStyle style in _Table.ColumnStyles)
				style.Width = width;
		}

		private void UpdateRowStyles()
		{
			var height = 100F / this.Rows;
			while(_Table.RowStyles.Count > this.Rows)
				_Table.RowStyles.RemoveAt(_Table.RowStyles.Count - 1);
			while(_Table.RowStyles.Count < this.Rows)
				_Table.RowStyles.Add(new RowStyle(SizeType.Percent, height));
			foreach(RowStyle style in _Table.RowStyles)
				style.Height = height;
		}

		private void ReduceTextBoxList()
		{
			throw new NotImplementedException();
		}

		private void ResizeTable()
		{
			this.SuspendLayout();
			throw new NotImplementedException();
			this.ResumeLayout();
		}

		#endregion

	}

}
