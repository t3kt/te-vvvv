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
	public partial class ValueControl : UserControl
	{

		#region Static / Constant

		#endregion

		#region Fields

		private double _Value;
		private bool _IsInteger;
		private double _Minimum;
		private double _Maximum;
		private bool _MouseDown;
		private Point _MousePoint;
		private Point _MouseDownPoint;

		#endregion

		#region Properties

		private string ValueBoxFormat
		{
			get { return _IsInteger ? "f0" : "f4"; }
		}

		#endregion

		#region Constructors

		public ValueControl()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void ShowValueBox()
		{
			_ValueBox.Text = _Value.ToString(this.ValueBoxFormat);
			_ValueBox.SelectAll();
			_ValueBox.Show();
			_ValueBox.Focus();
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);
			this.ShowValueBox();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if(e.Button == MouseButtons.Right)
			{
				this._MouseDown = true;
				this._MousePoint = this._MouseDownPoint = Cursor.Position;
				Cursor.Hide();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(_MouseDown)
			{
				_MouseDown = false;
				Cursor.Position = _MouseDownPoint;
				Cursor.Show();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(_MouseDown)
			{
				throw new NotImplementedException();
			}
		}

		#endregion

	}
}
