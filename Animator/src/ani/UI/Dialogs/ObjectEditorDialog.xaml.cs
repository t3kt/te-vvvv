using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Animator.Common.Diagnostics;
using Animator.Core.Runtime;
using Animator.Core.Runtime.ObjectStates;
using Animator.UI.Controls;
using Animator.UI.Editors;
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

		public static readonly DependencyProperty EditorProperty = DependencyProperty.Register("Editor", typeof(IObjectEditor),
				typeof(ObjectEditorDialog), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnEditorChanged));

		public static readonly DependencyProperty TargetProperty = ObjectEditor.TargetProperty.AddOwner(typeof(ObjectEditorDialog),
				new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty AutoCommitProperty = ObjectEditor.AutoCommitProperty.AddOwner(typeof(ObjectEditorDialog),
				new FrameworkPropertyMetadata(true));

		public static readonly DependencyProperty DirtyProperty = ObjectEditor.DirtyProperty.AddOwner(typeof(ObjectEditorDialog),
				new FrameworkPropertyMetadata(false));

		private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var dlg = (ObjectEditorDialog)d;
			dlg.DetachEditor(e.OldValue as IObjectEditor);
			dlg.AttachEditor(e.NewValue as IObjectEditor);
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

		public static bool ShowForEditor([NotNull]IObjectEditor editor, [CanBeNull]Window owner = null)
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

		internal static bool ShowPropertyGridForTarget(object target, [CanBeNull]Window owner = null, [CanBeNull]string title = null)
		{
			Require.ArgNotNull(target, "target");
			return ShowForEditor(
				new PropertyGridObjectEditor
				{
					Target = target,
					AutoCommit = true
				});
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public IObjectEditor Editor
		{
			get { return (IObjectEditor)this.GetValue(EditorProperty); }
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

		#endregion

		#region Events

		#endregion

		#region Constructors

		public ObjectEditorDialog()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void AttachEditor(IObjectEditor editor)
		{
			this.editorArea.Content = editor;
			if(editor == null)
				return;
			var fEditor = editor as FrameworkElement;
			if(fEditor == null)
				return;
			fEditor.SetBinding(AutoCommitProperty,
				new Binding
				{
					Source = this,
					Path = new PropertyPath("AutoCommit"),
					Mode = BindingMode.TwoWay
				});
			fEditor.SetBinding(DirtyProperty,
				new Binding
				{
					Source = this,
					Path = new PropertyPath("Dirty"),
					Mode = BindingMode.TwoWay
				});
		}

		private void DetachEditor(IObjectEditor editor)
		{
			if(editor == this.editorArea.Content)
				this.editorArea.Content = null;
			if(editor == null)
				return;
			var fEditor = editor as FrameworkElement;
			if(fEditor == null)
				return;
			BindingOperations.ClearBinding(fEditor, AutoCommitProperty);
			BindingOperations.ClearBinding(fEditor, DirtyProperty);
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
