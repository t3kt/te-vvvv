using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Animator.Core.Model.Sequences;

namespace Animator.UI.Sequencing
{

	#region SequenceTrackClipPanel

	public class SequenceTrackClipPanel : Panel
	{

		#region Static / Constant

		public static readonly DependencyProperty UnitsPerSecondProperty =
			SequencePanel.UnitsPerSecondProperty.AddOwner(typeof(SequenceTrackClipPanel),
				new FrameworkPropertyMetadata(0.0,
					FrameworkPropertyMetadataOptions.AffectsArrange |
					FrameworkPropertyMetadataOptions.AffectsMeasure |
					FrameworkPropertyMetadataOptions.AffectsParentArrange |
					FrameworkPropertyMetadataOptions.AffectsParentMeasure |
					FrameworkPropertyMetadataOptions.Inherits));

		private static Rect CalculateClipBounds(UIElement child, double height, double unitsPerSecond)
		{
			var elem = child as FrameworkElement;
			if(elem != null)
			{
				var clip = elem.DataContext as SequenceClip;
				if(clip != null && clip.Duration > TimeSpan.Zero)
				{
					return new Rect(
					new Point(clip.Start.TotalSeconds * unitsPerSecond, 0),
					new Size(clip.Duration.TotalSeconds * unitsPerSecond, height));
				}
			}
			return Rect.Empty;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public double UnitsPerSecond
		{
			get { return (double)this.GetValue(UnitsPerSecondProperty); }
			set { this.SetValue(UnitsPerSecondProperty, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override Size MeasureOverride(Size availableSize)
		{
			var unitsPerSecond = this.UnitsPerSecond;
			var totalSize = new Size(0, 0);
			if(!Double.IsInfinity(availableSize.Height) && !Double.IsNaN(availableSize.Height))
			{
				totalSize.Height = availableSize.Height;
			}
			foreach(UIElement child in this.InternalChildren)
			{
				var bounds = CalculateClipBounds(child, totalSize.Height, unitsPerSecond);
				if(!bounds.IsEmpty)
				{
					child.Measure(bounds.Size);
					if(bounds.Right > totalSize.Width)
						totalSize.Width = bounds.Right;
				}
				else
				{
					child.Measure(availableSize);
				}
				if(child.DesiredSize.Height > totalSize.Height)
					totalSize.Height = child.DesiredSize.Height;
			}
			//return new Size();
			return totalSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			var unitsPerSecond = this.UnitsPerSecond;
			var totalSize = finalSize;

			foreach(UIElement child in this.InternalChildren)
			{
				var bounds = CalculateClipBounds(child, finalSize.Height, unitsPerSecond);
				child.Arrange(bounds);
				if(!bounds.IsEmpty && bounds.Right > totalSize.Width)
					totalSize.Width = bounds.Right;
			}
			return totalSize;
		}

		#endregion

	}

	#endregion

}
