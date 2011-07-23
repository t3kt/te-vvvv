using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Animator.AppCore;

namespace Animator.UI.Clips
{

	#region ClipPropertyDataTemplateSelector

	public class ClipPropertyDataTemplateSelector : DataTemplateSelector
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			// ReSharper disable HeuristicUnreachableCode
			if(item == null)
				return null;
			// ReSharper restore HeuristicUnreachableCode
			// ReSharper restore ConditionIsAlwaysTrueOrFalse
			var dataType = item.GetType();
			var editorType = ClipPropertyDataEditors.GetEditorType(dataType);
			if(editorType == null)
				return null;
			var factory = new FrameworkElementFactory(editorType);
			return new DataTemplate(dataType) { VisualTree = factory };
		}

		#endregion

	}

	#endregion

}
