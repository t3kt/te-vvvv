using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.Model;
using Animator.Test;
using Animator.UI.Dialogs;

namespace Animator.UI
{

	#region MainWindow

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{

		#region Static / Constant

		public static readonly DependencyProperty ActiveDocumentProperty =
			DocumentManager.ActiveDocumentProperty.AddOwner(typeof(MainWindow),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnActiveDocumentChanged));

		public static readonly DependencyProperty HasActiveDocumentProperty =
			//DocumentManager.HasActiveDocumentProperty.AddOwner(typeof(MainWindow));
			DependencyProperty.Register("HasActiveDocument", typeof(bool), typeof(MainWindow));

		public static readonly DependencyProperty ActiveDocumentDirtyProperty =
			DocumentManager.ActiveDocumentDirtyProperty.AddOwner(typeof(MainWindow),
				new PropertyMetadata(false, OnActiveDocumentDirtyChanged));

		public static readonly DependencyProperty SelectedSectionProperty = AniUI.SelectedSectionProperty.AddOwner(typeof(MainWindow));

		private static void OnActiveDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var window = d as MainWindow;
			if(window != null)
				window.OnActiveDocumentChanged((Document)e.OldValue, (Document)e.NewValue);
		}

		private static void OnActiveDocumentDirtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var window = d as MainWindow;
			if(window != null)
				window.UpdateWindowTitle();
			CommandManager.InvalidateRequerySuggested();
		}

		#endregion

		#region Fields

		private readonly DocumentManager _DocumentManager;

		#endregion

		#region Events

		#endregion

		#region Properties

		public Document ActiveDocument
		{
			get { return (Document)this.GetValue(ActiveDocumentProperty); }
			set { this.SetValue(ActiveDocumentProperty, value); }
		}

		public bool ActiveDocumentDirty
		{
			get { return (bool)this.GetValue(ActiveDocumentDirtyProperty); }
		}

		public DocumentSection SelectedSection
		{
			get { return (DocumentSection)this.GetValue(SelectedSectionProperty); }
			set { this.SetValue(SelectedSectionProperty, value); }
		}

		#endregion

		#region Constructors

		public MainWindow()
		{
			this.InitializeComponent();
			this._DocumentManager = AniApplication.GetAppService<DocumentManager>();
			this.InitializeBindings();
		}

		#endregion

		#region Methods

		private void InitializeBindings()
		{
			this.SetBinding(SelectedSectionProperty,
				new Binding
				{
					ElementName = this.sectionsPane.Name,
					Path = new PropertyPath("SelectedSection")
				});

			this.SetBinding(ActiveDocumentProperty,
				new Binding
				{
					Source = this._DocumentManager,
					Path = new PropertyPath(DocumentManager.ActiveDocumentProperty)
				});
			this.SetBinding(HasActiveDocumentProperty,
				new Binding
				{
					Source = this._DocumentManager,
					Path = new PropertyPath(DocumentManager.HasActiveDocumentProperty)
				});
			this.SetBinding(ActiveDocumentDirtyProperty,
				new Binding
				{
					Source = this._DocumentManager,
					Path = new PropertyPath(DocumentManager.ActiveDocumentDirtyProperty)
				});
		}

		private void UpdateWindowTitle()
		{
			var sb = new StringBuilder();
			var doc = this.ActiveDocument;
			if(doc != null)
			{
				string name;
				var path = this._DocumentManager.ActiveDocumentPath;
				if(!String.IsNullOrEmpty(path))
					name = System.IO.Path.GetFileName(path);
				else
					name = "[Untitled]";
				sb.AppendFormat("{0}{1} - ", name, this.ActiveDocumentDirty ? "*" : null);
			}
			sb.Append("Animator");
			this.Title = sb.ToString();
		}

		private void OnActiveDocumentChanged(Document oldDocument, Document newDocument)
		{
			this.DataContext = newDocument;
			this.UpdateWindowTitle();
			this.SelectedSection = newDocument == null ? null : newDocument.Sections.FirstOrDefault();
			CommandManager.InvalidateRequerySuggested();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if(!this._DocumentManager.TryFileClose(this))
				e.Cancel = true;
			base.OnClosing(e);
		}

		internal void ShowAddOutput()
		{
			if(!this._DocumentManager.HasActiveDocument)
				return;
			var output = AddOutputDialog.ShowDialogForNewOutput(this);
			if(output != null)
			{
				this.ActiveDocument.Outputs.Add(output);
			}
		}

		internal void ShowAboutAppDialog()
		{
			var dlg = new AboutDialog { Owner = this };
			dlg.ShowDialog();
		}

		#endregion

		#region Event Handlers

		private void Command_CanExecuteWhenActiveDocumentDirty(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.ActiveDocumentDirty;
		}

		private void Command_CanExecuteWhenHasActiveDocument(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this._DocumentManager.HasActiveDocument;
		}

		private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}

		private void ShowTestWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			TestCommands.DoShowTestWindow(this);
		}

		private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this._DocumentManager.TryFileNew(this);
		}

		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this._DocumentManager.TryFileOpen(e.Parameter as string, this);
		}

		private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this._DocumentManager.TryFileSave(false, this);
		}

		private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this._DocumentManager.TryFileSave(true, this);
		}

		private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this._DocumentManager.TryFileClose(this);
		}

		private void AddOutputCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.ShowAddOutput();
		}

		private void AboutApplicationCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.ShowAboutAppDialog();
		}

		private void EditDetailCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			AniItemTypes.ShowEditDetail(e.Parameter);
		}

		private void EditDetailCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AniItemTypes.CanShowEditDetail(e.Parameter);
		}

		private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			AniItemTypes.TryDeleteNode(e.Parameter as DocumentNode);
		}

		private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AniItemTypes.CanDeleteNode(e.Parameter as DocumentNode);
		}

		#endregion

	}

	#endregion

}
