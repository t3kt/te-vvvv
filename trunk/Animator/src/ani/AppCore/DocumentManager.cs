using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using Animator.Core.Model;
using Animator.Properties;
using Microsoft.Win32;
using TESharedAnnotations;

namespace Animator.AppCore
{

	#region DocumentManager

	public class DocumentManager : DependencyObject
	{

		#region Static / Constant


		public static readonly DependencyProperty ActiveDocumentProperty =
			DependencyProperty.Register("ActiveDocument", typeof(Document), typeof(DocumentManager),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnActiveDocumentChanged));

		private static readonly DependencyPropertyKey HasActiveDocumentPropertyKey =
			DependencyProperty.RegisterReadOnly("HasActiveDocument", typeof(bool), typeof(DocumentManager),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty HasActiveDocumentProperty = HasActiveDocumentPropertyKey.DependencyProperty;

		public static readonly DependencyProperty ActiveDocumentDirtyProperty =
			DependencyProperty.Register("ActiveDocumentDirty", typeof(bool), typeof(DocumentManager),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		private static readonly DependencyPropertyKey ActiveDocumentPathPropertyKey =
			DependencyProperty.RegisterReadOnly("ActiveDocumentPath", typeof(string), typeof(DocumentManager), new PropertyMetadata(null));

		public static readonly DependencyProperty ActiveDocumentPathProperty =
			ActiveDocumentPathPropertyKey.DependencyProperty;

		private static void OnActiveDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.SetValue(HasActiveDocumentPropertyKey, e.NewValue != null);
			var docMgr = d as DocumentManager;
			if(docMgr != null)
				docMgr.OnActiveDocumentChanged((Document)e.OldValue, (Document)e.NewValue);
		}

		#endregion

		#region Fields

		private readonly RecentFileManager _RecentFileManager;

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
			private set { this.SetValue(HasActiveDocumentPropertyKey, value); }
		}

		public bool ActiveDocumentDirty
		{
			get { return (bool)this.GetValue(ActiveDocumentDirtyProperty); }
			set { this.SetValue(ActiveDocumentDirtyProperty, value); }
		}

		public string ActiveDocumentPath
		{
			get { return (string)this.GetValue(ActiveDocumentPathProperty); }
			private set { this.SetValue(ActiveDocumentPathPropertyKey, value); }
		}

		internal RecentFileManager RecentFileManager
		{
			get { return this._RecentFileManager; }
		}

		public ObservableCollection<string> RecentFiles
		{
			get { return this._RecentFileManager.Files; }
		}

		#endregion

		#region Constructors

		public DocumentManager()
		{
			this._RecentFileManager = new RecentFileManager();
		}

		#endregion

		#region Core Methods

		public void CloseFile()
		{
			this.ActiveDocument = null;
			this.ActiveDocumentPath = null;
			this.ActiveDocumentDirty = false;
		}

		public Document NewFile()
		{
			this.CloseFile();
			this.ActiveDocument =
				new Document
				{
					Name = "Untitled"
				};
			this.ActiveDocumentPath = null;
			this.ActiveDocumentDirty = false;
			return this.ActiveDocument;
		}

		public Document OpenFile(string path)
		{
			this.CloseFile();
			if(String.IsNullOrEmpty(path))
				return null;
			try
			{
				var xdoc = XDocument.Load(path);
				Debug.Assert(xdoc.Root != null);
				var doc = new Document(xdoc.Root);
				this._RecentFileManager.AddFile(path);
				this.ActiveDocumentPath = path;
				this.ActiveDocument = doc;
				this.ActiveDocumentDirty = false;
				return doc;
			}
			catch(System.IO.FileNotFoundException ex)
			{
				this._RecentFileManager.RemoveFile(path);
				ShowFileOpenError(path, ex);
			}
			catch(Exception ex)
			{
				ShowFileOpenError(path, ex);
			}
			return null;
		}

		public void SaveFile(string path)
		{
			if(!this.HasActiveDocument || String.IsNullOrEmpty(path))
				return;
			try
			{
				var doc = this.ActiveDocument.WriteXDocument();
				doc.Save(path);
				this._RecentFileManager.AddFile(path);
				this.ActiveDocumentPath = path;
				this.ActiveDocumentDirty = false;
			}
			catch(Exception ex)
			{
				ShowFileSaveError(path, ex);
			}
		}

		public void SaveFile()
		{
			this.SaveFile(this.ActiveDocumentPath);
		}

		#endregion

		#region UI/Application Logic Methods

		private bool PromptSaveForClose([NotNull] Window window)
		{
			if(!this.ActiveDocumentDirty)
				return true;
			var result = MessageBox.Show(window, "Save changes to document before closing?", "Save Changes?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
			switch(result)
			{
			case MessageBoxResult.Yes:
				return this.TryFileSave(false, window);
			case MessageBoxResult.No:
				return true;
			case MessageBoxResult.Cancel:
				return false;
			default:
				return false;
			}
		}

		internal bool TryFileNew(Window window)
		{
			if(!this.PromptSaveForClose(window ?? Application.Current.MainWindow))
				return false;
			this.NewFile();
			return true;
		}

		internal bool TryFileOpen(string path, Window window)
		{
			window = window ?? Application.Current.MainWindow;
			if(!this.PromptSaveForClose(window))
				return false;
			if(!String.IsNullOrEmpty(path))
			{
				this.OpenFile(path);
			}
			else
			{
				var dlg =
					new OpenFileDialog
					{
						AddExtension = true,
						DefaultExt = Settings.Default.DefaultFileExtension,
						Filter = Settings.Default.FileDialogFilter
					};
				if(dlg.ShowDialog(window) != true)
					return false;
				this.OpenFile(dlg.FileName);
			}
			return true;
		}

		internal bool TryFileSave(bool asNewFile, Window window)
		{
			window = window ?? Application.Current.MainWindow;
			if(!this.HasActiveDocument)
				return false;
			if(asNewFile || String.IsNullOrEmpty(this.ActiveDocumentPath))
			{
				var dlg = new SaveFileDialog
				{
					AddExtension = true,
					DefaultExt = Settings.Default.DefaultFileExtension,
					Filter = Settings.Default.FileDialogFilter,
					FileName = this.ActiveDocumentPath ?? (this.ActiveDocument.Name + "." + Settings.Default.DefaultFileExtension),
					//CheckFileExists = asNewFile
				};
				if(dlg.ShowDialog(window) == true)
				{
					SaveFile(dlg.FileName);
					return true;
				}
				return false;
			}
			this.SaveFile(this.ActiveDocumentPath);
			return true;
		}

		internal bool TryFileClose(Window window)
		{
			if(!this.PromptSaveForClose(window ?? Application.Current.MainWindow))
				return false;
			this.CloseFile();
			return true;
		}

		private void ShowFileSaveError(string path, Exception ex)
		{
			MessageBox.Show(Application.Current.MainWindow, String.Format("An error occurred when saving file '{0}':\n\t{1}", path, ex.Message), "An error occurred when saving file", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void ShowFileOpenError(string path, Exception ex)
		{
			MessageBox.Show(Application.Current.MainWindow,
				String.Format("An error occurred when opening file '{0}':\n\t{1}", path, ex.Message), "An error occurred when opening file", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void OnActiveDocumentChanged(Document oldDocument, Document newDocument)
		{
			if(oldDocument != null)
				oldDocument.PropertyChanged -= this.ActiveDocument_PropertyChanged;
			if(newDocument != null)
				newDocument.PropertyChanged += this.ActiveDocument_PropertyChanged;
		}

		private void ActiveDocument_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.ActiveDocumentDirty = true;
		}

		#endregion

	}

	#endregion

}
