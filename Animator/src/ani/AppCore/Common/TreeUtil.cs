using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Animator.AppCore.Common
{

	#region TreeUtil

	internal static class TreeUtil
	{

		internal static IEnumerable<DependencyObject> FindVisualDescendants(DependencyObject parent)
		{
			if(parent == null)
				yield break;
			var childCount = VisualTreeHelper.GetChildrenCount(parent);
			for(var i = 0; i < childCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				if(child != null)
				{
					yield return child;
					foreach(var gchild in FindVisualDescendants(child))
						yield return gchild;
				}
			}
		}

	}

	#endregion

}
