using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Animator.Core.Model.Sequences;
using Animator.Core.Model.Sessions;

namespace Animator.UI.Common
{

	#region SectionTemplateSelector

	internal sealed class SectionTemplateSelector : DataTemplateSelector
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public DataTemplate SequenceTemplate { get; set; }

		public DataTemplate SessionTemplate { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if(item is Sequence)
				return this.SequenceTemplate;
			if(item is Session)
				return this.SessionTemplate;
			return null;
		}

		#endregion

	}

	#endregion

}
