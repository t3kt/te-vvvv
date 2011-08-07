using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Animator.Core.Runtime;
using Animator.Core.Runtime.ObjectStates;

namespace Animator.UI.Controls
{

	#region ObjectStateEditor

	public class ObjectStateEditor : UserControl, IObjectEditor
	{

		#region Static / Constant

		public static readonly DependencyProperty TargetProperty = ObjectEditor.TargetProperty.AddOwner(typeof(ObjectStateEditor),
			new FrameworkPropertyMetadata(null, OnTargetChanged));

		public static readonly DependencyProperty DirtyProperty = ObjectEditor.DirtyProperty.AddOwner(typeof(ObjectStateEditor));

		private static readonly DependencyPropertyKey TargetStatePropertyKey = DependencyProperty.RegisterReadOnly("TargetState", typeof(ObjectState),
			typeof(ObjectStateEditor), new FrameworkPropertyMetadata(null, OnTargetStateChanged));

		public static readonly DependencyProperty TargetStateProperty = TargetStatePropertyKey.DependencyProperty;

		private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (ObjectStateEditor)d;
			editor.TargetState = e.NewValue == null ? null : ObjectState.CreateState(e.NewValue.GetType(), e.NewValue);
		}

		private static void OnTargetStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (ObjectStateEditor)d;
			editor.OnTargetStateChanged(e);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public bool AutoCommit
		{
			get { return false; }
		}

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

		public bool Dirty
		{
			get { return (bool)this.GetValue(DirtyProperty); }
			set { this.SetValue(DirtyProperty, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected virtual void OnTargetStateChanged(DependencyPropertyChangedEventArgs e)
		{
			this.DetachTargetState(e.OldValue as ObjectState);
			this.DataContext = this.TargetState;
			this.AttachTargetState(this.TargetState);
			this.Dirty = false;
		}

		private void AttachTargetState(ObjectState state)
		{
			if(state != null)
				state.PropertyChanged += this.Target_PropertyChanged;
		}

		private void DetachTargetState(ObjectState state)
		{
			if(state != null)
				state.PropertyChanged -= this.Target_PropertyChanged;
		}

		private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.Dirty = true;
		}

		public void Commit()
		{
			if(this.Dirty)
			{
				this.PerformCommit();
				this.Dirty = false;
			}
		}

		protected virtual void PerformCommit()
		{
			var state = this.TargetState;
			if(state != null)
				state.Save();
		}

		public void Reset()
		{
			if(this.Dirty)
				this.PerformReset();
			this.Dirty = false;
		}

		protected virtual void PerformReset()
		{
			var state = this.TargetState;
			if(state != null)
				state.Load();
		}

		#endregion

	}

	#endregion

}
