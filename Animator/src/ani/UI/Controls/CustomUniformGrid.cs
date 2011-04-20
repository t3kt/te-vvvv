using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Animator.Common;

namespace Animator.UI.Controls
{

	#region CustomUniformGrid

	internal class CustomUniformGrid : Grid
	{

		#region Static / Constant

		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(CustomUniformGrid), new PropertyMetadata(1, OnColumnsChanged), IsDimensionValid);

		public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(CustomUniformGrid), new PropertyMetadata(1, OnRowsChanged), IsDimensionValid);

		public static readonly DependencyProperty RowHeightProperty = DependencyProperty.Register("RowHeight", typeof(GridLength), typeof(CustomUniformGrid),
			new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, OnRowHeightChanged), IsCellDimensionValid);

		public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(GridLength), typeof(CustomUniformGrid),
			new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, OnColumnWidthChanged), IsCellDimensionValid);

		private static bool IsCellDimensionValid(object value)
		{
			return ((GridLength)value).Value >= 0.0;
		}

		private static bool IsDimensionValid(object value)
		{
			return (int)value >= 1;
		}

		private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var grid = (CustomUniformGrid)d;
			var requested = (int)e.NewValue;
			var dimension = grid.ColumnWidth;
			CommonUtil.ResizeCollection(grid.ColumnDefinitions, requested, x => new ColumnDefinition { Width = dimension });
		}

		private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var grid = (CustomUniformGrid)d;
			var requested = (int)e.NewValue;
			var dimension = grid.RowHeight;
			CommonUtil.ResizeCollection(grid.RowDefinitions, requested, x => new RowDefinition() { Height = dimension });
		}

		private static void OnColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var grid = (CustomUniformGrid)d;
			var dimension = (GridLength)e.NewValue;
			foreach(var column in grid.ColumnDefinitions)
				column.Width = dimension;
		}

		private static void OnRowHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var grid = (CustomUniformGrid)d;
			var dimension = (GridLength)e.NewValue;
			foreach(var row in grid.RowDefinitions)
				row.Height = dimension;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public int Rows
		{
			get { return (int)this.GetValue(RowsProperty); }
			set { this.SetValue(RowsProperty, value); }
		}

		public int Columns
		{
			get { return (int)this.GetValue(ColumnsProperty); }
			set { this.SetValue(ColumnsProperty, value); }
		}

		public GridLength RowHeight
		{
			get { return (GridLength)this.GetValue(RowHeightProperty); }
			set { this.SetValue(RowHeightProperty, value); }
		}

		public GridLength ColumnWidth
		{
			get { return (GridLength)this.GetValue(ColumnWidthProperty); }
			set { this.SetValue(ColumnWidthProperty, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
