using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Animator.Core.Runtime
{

	#region IObjectEditor

	public interface IObjectEditor
	{
		object Target { get; set; }
		bool AutoCommit { get; set; }
		bool Dirty { get; }
		Visibility BasicsVisibility { get; set; }
		Visibility DetailsVisibility { get; set; }
		event TargetPropertyChangedEventHandler TargetPropertyChanged;
		void Commit();
		void Reset();
	}

	#endregion

}
