using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Runtime;
using Animator.UI.Controls;
using TESharedAnnotations;

namespace Animator.UI.Dialogs
{

	#region ObjectEditorDialog

	/// <summary>
	/// Interaction logic for ObjectEditorDialog.xaml
	/// </summary>
	public partial class ObjectEditorDialog : IObjectEditor
	{

		#region Static / Constant

		public static readonly DependencyProperty EditorProperty = DependencyProperty.Register("Editor", typeof(ObjectEditor),
				typeof(ObjectEditorDialog), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnEditorChanged));

		public static readonly DependencyProperty TargetProperty = ObjectEditor.TargetProperty.AddOwner(typeof(ObjectEditorDialog),
				new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty AutoCommitProperty = ObjectEditor.AutoCommitProperty.AddOwner(typeof(ObjectEditorDialog),
				new FrameworkPropertyMetadata(true));

		public static readonly DependencyProperty BasicsVisibilityProperty = ObjectEditor.BasicsVisibilityProperty.AddOwner(typeof(ObjectEditorDialog),
				new FrameworkPropertyMetadata(Visibility.Visible));

		public static readonly DependencyProperty DetailsVisibilityProperty = ObjectEditor.DetailsVisibilityProperty.AddOwner(typeof(ObjectEditorDialog),
				new FrameworkPropertyMetadata(Visibility.Collapsed));

		public static readonly DependencyProperty DirtyProperty = ObjectEditor.DirtyProperty.AddOwner(typeof(ObjectEditorDialog),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty EditorStyleProperty = DependencyProperty.Register("EditorStyle", typeof(Style),
			typeof(ObjectEditorDialog), new FrameworkPropertyMetadata(null));

		public static readonly RoutedEvent TargetPropertyChangedEvent = ObjectEditor.TargetPropertyChangedEvent.AddOwner(typeof(ObjectEditorDialog));

		private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var dlg = (ObjectEditorDialog)d;
			dlg.DetachEditor(e.OldValue as ObjectEditor);
			dlg.AttachEditor(e.NewValue as ObjectEditor);
		}

		public static bool ShowForTarget(object target, [NotNull]ObjectEditor editor, [CanBeNull]Window owner = null, [CanBeNull]string title = null)
		{
			Require.ArgNotNull(target, "target");
			Require.ArgNotNull(editor, "editor");
			var dlg = new ObjectEditorDialog
					  {
						  Owner = owner,
						  Editor = editor,
						  Target = target
					  };
			var result = dlg.ShowDialog();
			return result == true;
		}

		public static bool ShowForEditor([NotNull]ObjectEditor editor, [CanBeNull]Window owner = null)
		{
			Require.ArgNotNull(editor, "editor");
			var dlg = new ObjectEditorDialog
			{
				Owner = owner,
				Editor = editor
			};
			var result = dlg.ShowDialog();
			return result == true;
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public ObjectEditor Editor
		{
			get { return (ObjectEditor)this.GetValue(EditorProperty); }
			set { this.SetValue(EditorProperty, value); }
		}

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
			set { this.SetValue(DirtyProperty, value); }
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

		public Style EditorStyle
		{
			get { return (Style)this.GetValue(EditorStyleProperty); }
			set { this.SetValue(EditorStyleProperty, value); }
		}

		#endregion

		#region Events

		public event TargetPropertyChangedEventHandler TargetPropertyChanged
		{
			add { this.AddHandler(TargetPropertyChangedEvent, value); }
			remove { this.RemoveHandler(TargetPropertyChangedEvent, value); }
		}

		#endregion

		#region Constructors

		public ObjectEditorDialog()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void Editor_TargetPropertyChanged(object sender, TargetPropertyChangedEventArgs e)
		{
			this.RaiseEvent(new TargetPropertyChangedEventArgs(TargetPropertyChangedEvent, this, e.Target, e.PropertyName));
		}

		private void AttachEditor(ObjectEditor editor)
		{
			if(editor == null)
				return;
			editor.TargetPropertyChanged += this.Editor_TargetPropertyChanged;
			editor.SetBinding(AutoCommitProperty,
				new Binding
				{
					Source = this,
					Path = new PropertyPath("AutoCommit"),
					Mode = BindingMode.TwoWay
				});
			editor.SetBinding(DirtyProperty,
				new Binding
				{
					Source = this,
					Path = new PropertyPath("Dirty"),
					Mode = BindingMode.TwoWay
				});
			editor.SetBinding(BasicsVisibilityProperty,
				new Binding
				{
					Source = this,
					Path = new PropertyPath("BasicsVisibility"),
					Mode = BindingMode.TwoWay
				});
			editor.SetBinding(DetailsVisibilityProperty,
				new Binding
				{
					Source = this,
					Path = new PropertyPath("DetailsVisibility"),
					Mode = BindingMode.TwoWay
				});
			editor.SetBinding(StyleProperty,
				new Binding
				{
					Source = this,
					Path = new PropertyPath("EditorStyle"),
					Mode = BindingMode.OneWay
				});
		}

		private void DetachEditor(ObjectEditor editor)
		{
			if(editor == null)
				return;
			BindingOperations.ClearBinding(editor, AutoCommitProperty);
			BindingOperations.ClearBinding(editor, DirtyProperty);
			BindingOperations.ClearBinding(editor, BasicsVisibilityProperty);
			BindingOperations.ClearBinding(editor, DetailsVisibilityProperty);
			BindingOperations.ClearBinding(editor, StyleProperty);
			editor.TargetPropertyChanged -= this.Editor_TargetPropertyChanged;
		}

		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			this.Accept();
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.Cancel();
		}

		public void Accept()
		{
			if(!this.AutoCommit)
				this.Commit();
			this.DialogResult = true;
			this.Close();
		}

		public void Cancel()
		{
			if(this.AutoCommit)
				throw new NotSupportedException();
			this.Reset();
			this.DialogResult = false;
			this.Close();
		}

		public void Commit()
		{
			var editor = this.Editor;
			if(editor != null)
				editor.Commit();
		}

		public void Reset()
		{
			var editor = this.Editor;
			if(editor != null)
				editor.Reset();
		}

		#endregion

	}

	#endregion

}
