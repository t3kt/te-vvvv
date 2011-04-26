using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Animator.AppCore;
using Animator.Core.Model;
using Animator.Test;
using Animator.UI.Dialogs;
using Microsoft.Win32;

namespace Animator.UI
{

	#region MainWindow

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{

		#region Static / Constant

		public static readonly DependencyProperty ActiveDocumentProperty;
		private static readonly DependencyPropertyKey HasActiveDocumentPropertyKey;
		public static readonly DependencyProperty HasActiveDocumentProperty;
		public static readonly DependencyProperty ActiveDocumentDirtyProperty;

		static MainWindow()
		{
			ActiveDocumentProperty = DependencyProperty.Register("ActiveDocument", typeof(Document), typeof(MainWindow),
																 new PropertyMetadata(null, OnActiveDocumentChanged));
			HasActiveDocumentPropertyKey = DependencyProperty.RegisterReadOnly("HasActiveDocument", typeof(bool), typeof(MainWindow),
																			   new PropertyMetadata(false));
			HasActiveDocumentProperty = HasActiveDocumentPropertyKey.DependencyProperty;
			ActiveDocumentDirtyProperty = DependencyProperty.Register("ActiveDocumentDirty", typeof(bool), typeof(MainWindow),
																	  new PropertyMetadata(false, OnActiveDocumentDirtyChanged));
		}

		private static void OnActiveDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var window = (MainWindow)d;
			window.SetValue(HasActiveDocumentPropertyKey, e.NewValue != null);
			window.OnActiveDocumentChanged((Document)e.OldValue, (Document)e.NewValue);
			var app = Application.Current as AniApplication;
			if(app != null)
				app.ActiveDocument = (Document)e.NewValue;
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

		private string _ActiveDocumentPath;

		#endregion

		#region Events

		#endregion

		#region Properties

		public Document ActiveDocument
		{
			get { return (Document)this.GetValue(ActiveDocumentProperty); }
			set { this.SetValue(ActiveDocumentProperty, value); }
		}

		public bool HasActiveDocument
		{
			get { return (bool)this.GetValue(HasActiveDocumentProperty); }
		}

		public bool ActiveDocumentDirty
		{
			get { return (bool)this.GetValue(ActiveDocumentDirtyProperty); }
			set { this.SetValue(ActiveDocumentDirtyProperty, value); }
		}

		public ObservableCollection<string> RecentFiles
		{
			get { return ((AniApplication)Application.Current).RecentFileManager.Files; }
		}

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void UpdateWindowTitle()
		{
			var sb = new StringBuilder();
			var doc = this.ActiveDocument;
			if(doc != null)
			{
				string name;
				if(!String.IsNullOrEmpty(this._ActiveDocumentPath))
					name = System.IO.Path.GetFileName(this._ActiveDocumentPath);
				else
					name = "[Untitled]";
				sb.AppendFormat("{0}{1} - ", name, this.ActiveDocumentDirty ? "*" : null);
			}
			sb.Append("Animator");
			this.Title = sb.ToString();
		}

		private void OnActiveDocumentChanged(Document oldDocument, Document newDocument)
		{
			this.DetachDocument(oldDocument);
			this.AttachDocument(newDocument);
			this.DataContext = newDocument;
			CommandManager.InvalidateRequerySuggested();
			this.UpdateWindowTitle();
		}

		private void AttachDocument(Document document)
		{
			if(document != null)
				document.PropertyChanged += this.ActiveDocument_PropertyChanged;
		}

		private void DetachDocument(Document document)
		{
			if(document != null)
				document.PropertyChanged -= this.ActiveDocument_PropertyChanged;
		}

		internal void OnFileNew()
		{
			if(!this.PromptSaveForClose())
				return;
			this.ActiveDocument = new Document { Name = "Untitled" };
			this.ActiveDocumentDirty = true;
		}

		internal void OnFileOpen(string path)
		{
			if(!this.PromptSaveForClose())
				return;
			if(!String.IsNullOrEmpty(path))
			{
				this.OpenFile(path);
			}
			else
			{
				var dlg = new OpenFileDialog
						  {
							  AddExtension = true,
							  DefaultExt = Constants.FileExtension,
							  Filter = Constants.FileDialogFilter
						  };
				if(dlg.ShowDialog(this) == true)
				{
					this.OpenFile(dlg.FileName);
				}
			}
		}

		internal void OpenFile(string path)
		{
			if(String.IsNullOrEmpty(path))
				return;
			try
			{
				var xdoc = XDocument.Load(path);
				var doc = new Document(xdoc.Root);
				this._ActiveDocumentPath = path;
				this.ActiveDocument = doc;
				this.ActiveDocumentDirty = false;
				((AniApplication)Application.Current).RecentFileManager.AddFile(path);
			}
			catch(Exception ex)
			{
				ShowFileOpenError(path, ex);
			}
		}

		private void ShowFileOpenError(string path, Exception ex)
		{
			MessageBox.Show(this, String.Format("An error occurred when opening file '{0}':\n\t{1}", path, ex.Message), "An error occurred when opening file", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		internal void OnFileSave(bool asNewFile = false)
		{
			if(!this.HasActiveDocument)
				return;
			if(asNewFile || String.IsNullOrEmpty(this._ActiveDocumentPath))
			{
				var dlg = new SaveFileDialog
							{
								AddExtension = true,
								DefaultExt = Constants.FileExtension,
								Filter = Constants.FileDialogFilter,
								FileName = this._ActiveDocumentPath ?? (this.ActiveDocument.Name + "." + Constants.FileExtension)
							};
				if(dlg.ShowDialog(this) == true)
				{
					SaveFile(dlg.FileName);
				}
			}
			else
			{
				SaveFile(this._ActiveDocumentPath);
			}
		}

		internal void SaveFile(string path)
		{
			if(!this.HasActiveDocument || String.IsNullOrEmpty(path))
				return;
			try
			{
				var doc = new XDocument(this.ActiveDocument.WriteXElement());
				doc.Save(path);
				this.ActiveDocumentDirty = false;
				((AniApplication)Application.Current).RecentFileManager.AddFile(path);
			}
			catch(Exception ex)
			{
				ShowFileSaveError(path, ex);
			}
		}

		private void ShowFileSaveError(string path, Exception ex)
		{
			MessageBox.Show(this, String.Format("An error occurred when saving file '{0}':\n\t{1}", path, ex.Message), "An error occurred when saving file", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private bool PromptSaveForClose()
		{
			if(!this.ActiveDocumentDirty)
				return true;
			var result = MessageBox.Show(this, "Save changes to document before closing?", "Save Changes?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
			switch(result)
			{
			case MessageBoxResult.Yes:
				this.OnFileSave();
				return !this.ActiveDocumentDirty;
			case MessageBoxResult.No:
				return true;
			case MessageBoxResult.Cancel:
				return false;
			default:
				return false;
			}
		}

		private void CloseFile()
		{
			this.ActiveDocument = null;
			this.ActiveDocumentDirty = false;
		}

		internal void OnFileClose()
		{
			if(this.PromptSaveForClose())
				this.CloseFile();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if(!this.PromptSaveForClose())
				e.Cancel = true;
			base.OnClosing(e);
		}

		internal void ShowAddClip()
		{
			if(!this.HasActiveDocument)
				return;
			var clip = AddClipDialog.ShowDialogForNewClip(this);
			if(clip != null)
			{
				//...
				this.ActiveDocument.Clips.Add(clip);
			}
		}

		internal void ShowAddOutput()
		{
			if(!this.HasActiveDocument)
				return;
			var output = AddOutputDialog.ShowDialogForNewOutput(this);
			if(output != null)
			{
				this.ActiveDocument.Outputs.Add(output);
			}
		}

		#endregion

		#region Event Handlers

		private void ActiveDocument_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.ActiveDocumentDirty = true;
		}

		private void Command_CanExecuteWhenActiveDocumentDirty(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.ActiveDocumentDirty;
		}

		private void Command_CanExecuteWhenHasActiveDocument(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.HasActiveDocument;
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
			OnFileNew();
		}

		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OnFileOpen(e.Parameter as string);
		}

		private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OnFileSave();
		}

		private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OnFileSave(true);
		}

		private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OnFileClose();
		}

		private void DebuggerBreakCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			TestCommands.DoDebugBreak();
		}

		private void ShowRecentFilesInfoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			TestCommands.DoShowRecentFilesInfo(this);
		}

		private void AddClipCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.ShowAddClip();
		}

		private void AddOutputCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.ShowAddOutput();
		}

		private void OutputList_DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if(this.ActiveDocument == null)
				return;
			var output = e.Parameter as Output;
			if(output != null)
			{
				this.ActiveDocument.Outputs.Remove(output);
				output.Dispose();
			}
		}

		#endregion

	}

	#endregion

}
