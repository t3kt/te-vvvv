using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Animator.AppCore;

namespace Animator.UI.Controls
{

	#region PropertyEditor

	public class PropertyEditor : UserControl
	{

		#region Static / Constant

		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(object),
			typeof(PropertyEditor), new FrameworkPropertyMetadata(null, OnTargetChanged));

		public static readonly DependencyProperty AutoCommitProperty = DependencyProperty.Register("AutoCommit", typeof(bool),
			typeof(PropertyEditor), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits, OnAutoCommitChanged));

		private static readonly DependencyPropertyKey DirtyPropertyKey = DependencyProperty.RegisterReadOnly("Dirty", typeof(bool),
			typeof(PropertyEditor), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty DirtyProperty = DirtyPropertyKey.DependencyProperty;

		public static readonly DependencyProperty BasicsVisibilityProperty = DependencyProperty.Register("BasicsVisibility", typeof(Visibility),
			typeof(PropertyEditor), new FrameworkPropertyMetadata(Visibility.Visible));

		public static readonly DependencyProperty DetailsVisibilityProperty = DependencyProperty.Register("DetailsVisibility", typeof(Visibility),
			typeof(PropertyEditor), new FrameworkPropertyMetadata(Visibility.Collapsed));

		public static readonly RoutedEvent TargetPropertyChangedEvent = EventManager.RegisterRoutedEvent("TargetPropertyChanged", RoutingStrategy.Bubble,
			typeof(TargetPropertyChangedEventHandler), typeof(PropertyEditor));

		private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (PropertyEditor)d;
			editor.OnTargetChanged(e);
		}

		private static void OnAutoCommitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var editor = (PropertyEditor)d;
			if((bool)e.NewValue)
			{
				editor.PerformCommit();
				editor.Dirty = false;
			}
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

		public bool AutoCommit
		{
			get { return (bool)this.GetValue(AutoCommitProperty); }
			set { this.SetValue(AutoCommitProperty, value); }
		}

		public bool Dirty
		{
			get { return (bool)this.GetValue(DirtyProperty); }
			private set { this.SetValue(DirtyPropertyKey, value); }
		}

		public Visibility BasicsVisibility
		{
			get { return (Visibility)this.GetValue(BasicsVisibilityProperty); }
			set { this.SetValue(BasicsVisibilityProperty, value); }
		}

		public Visibility DetailsVisibility
		{
			get { return (Visibility)this.GetValue(DetailsVisibilityProperty); }
			set { this.SetValue(DetailsVisibilityProperty, value); }
		}

		#endregion

		#region Events

		public event TargetPropertyChangedEventHandler TargetPropertyChanged
		{
			add { this.AddHandler(TargetPropertyChangedEvent, value); }
			remove { this.RemoveHandler(TargetPropertyChangedEvent, value); }
		}

		protected void OnTargetPropertyChanged(string propertyName)
		{
			OnTargetPropertyChanged(this.Target, propertyName);
		}

		protected virtual void OnTargetPropertyChanged(object target, string propertyName)
		{
			this.RaiseEvent(new TargetPropertyChangedEventArgs(TargetPropertyChangedEvent, this, target, propertyName));
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected virtual void OnTargetChanged(DependencyPropertyChangedEventArgs e)
		{
			this.DetachChangeHandler(e.OldValue);
			this.AttachChangeHandler(e.NewValue);
			this.Dirty = false;
			this.DataContext = e.NewValue;
			this.PerformReset();
		}

		private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnTargetPropertyChanged(sender, e.PropertyName);
		}

		protected void AttachChangeHandler(object target)
		{
			var x = target as INotifyPropertyChanged;
			if(x != null)
				x.PropertyChanged += this.Target_PropertyChanged;
		}

		protected void DetachChangeHandler(object target)
		{
			var x = target as INotifyPropertyChanged;
			if(x != null)
				x.PropertyChanged -= this.Target_PropertyChanged;
		}

		public void Commit()
		{
			if(this.Dirty && !this.AutoCommit)
			{
				this.PerformCommit();
				this.Dirty = false;
			}
		}

		protected virtual void PerformCommit()
		{
		}

		public void Reset()
		{
			if(this.AutoCommit)
				throw new NotSupportedException();
			this.PerformReset();
			this.Dirty = false;
		}

		protected virtual void PerformReset()
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
