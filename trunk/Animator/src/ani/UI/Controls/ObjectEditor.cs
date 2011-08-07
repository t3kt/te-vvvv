using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Animator.AppCore;
using Animator.Core.Runtime;
using Animator.Core.Runtime.ObjectStates;

namespace Animator.UI.Controls
{

	#region ObjectEditor

	public class ObjectEditor : UserControl, IObjectEditor
	{

		#region Static / Constant

		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(object),
			typeof(ObjectEditor), new FrameworkPropertyMetadata(null, OnTargetChanged));

		private static readonly DependencyPropertyKey TargetStatePropertyKey = DependencyProperty.RegisterReadOnly("TargetState", typeof(ObjectState),
			typeof(ObjectEditor), new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty TargetStateProperty = TargetStatePropertyKey.DependencyProperty;

		public static readonly DependencyProperty AutoCommitProperty = DependencyProperty.Register("AutoCommit", typeof(bool),
			typeof(ObjectEditor), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits, OnAutoCommitChanged));

		public static readonly DependencyProperty DirtyProperty = AniUI.DirtyProperty.AddOwner(typeof(ObjectEditor));

		private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (ObjectEditor)d;
			editor.OnTargetChanged(e);
		}

		private static void OnAutoCommitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (ObjectEditor)d;
			editor.OnAutoCommitChanged(e);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public object Target
		{
			get { return this.GetValue(TargetProperty); }
			set { this.SetValue(TargetProperty, value); }
		}

		public ObjectState TargetState
		{
			get { return (ObjectState)this.GetValue(TargetStateProperty); }
			set { this.SetValue(TargetStatePropertyKey, value); }
		}

		public bool AutoCommit
		{
			get { return (bool)this.GetValue(AutoCommitProperty); }
			set { this.SetValue(AutoCommitProperty, value); }
		}

		public bool Dirty
		{
			get { return (bool)this.GetValue(DirtyProperty); }
			set { this.SetValue(DirtyProperty, value); }
		}

		#endregion

		#region Events

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected virtual void OnTargetChanged(DependencyPropertyChangedEventArgs e)
		{
			this.DataContext = e.NewValue;
			this.DetachTarget(e.OldValue);
			if(this.AutoCommit || e.NewValue == null)
				this.TargetState = null;
			else
				this.DataContext = this.TargetState = ObjectState.CreateState(e.NewValue.GetType(), e.NewValue);
			this.AttachTarget(this.AutoCommit ? e.NewValue : this.TargetState);
			this.Dirty = false;
		}

		protected virtual void OnAutoCommitChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.AutoCommit)
			{
				this.PerformCommit();
				this.Dirty = false;
				this.DetachTarget(this.TargetState);
				this.TargetState = null;
			}
			else
			{
				this.Dirty = false;
			}
			throw new NotImplementedException();
		}

		private void AttachTarget(object target)
		{
			var notifier = target as INotifyPropertyChanged;
			if(notifier != null)
				notifier.PropertyChanged += this.Target_PropertyChanged;
		}

		private void DetachTarget(object target)
		{
			var notifier = target as INotifyPropertyChanged;
			if(notifier != null)
				notifier.PropertyChanged -= this.Target_PropertyChanged;
		}

		private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if(!this.AutoCommit)
				this.Dirty = true;
		}

		public virtual void Commit()
		{
			if(!this.Dirty || this.AutoCommit)
				return;
			this.PerformCommit();
			this.Dirty = false;
		}

		protected virtual void PerformCommit()
		{
			var state = this.TargetState;
			if(state == null)
				return;
			state.Save();
		}

		public void Reset()
		{
			if(this.AutoCommit)
			{
				return;
				//throw new NotSupportedException();
			}
			var state = this.TargetState;
			if(state == null)
				return;
			state.Load();
			this.Dirty = false;
		}

		#endregion

	}

	#endregion

}
