using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using Animator.AppCore;
using Animator.Core.Model;
using Animator.Test;
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
		public static readonly DependencyPropertyKey HasActiveDocumentPropertyKey;
		private static readonly DependencyProperty HasActiveDocumentProperty;
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
			((App)Application.Current).ActiveDocument = (Document)e.NewValue;
		}

		private static void OnActiveDocumentDirtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
		}

		#endregion

		#region Fields

		private TestWindow1 _TestWindow;
		private string _ActiveDocumentPath;

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

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void OnActiveDocumentChanged(Document oldDocument, Document newDocument)
		{
			this.DataContext = newDocument;
			CommandManager.InvalidateRequerySuggested();
		}

		internal void OnFileNew()
		{
			if(!this.TryCloseFile())
				return;
			this.ActiveDocument = new Document();
		}

		internal void OnFileOpen()
		{
			if(!this.TryCloseFile())
				return;
			var dlg = new OpenFileDialog
						{
							AddExtension = true,
							DefaultExt = Constants.FileExtension,
							Filter = Constants.FileDialogFilter
						};
			if(dlg.ShowDialog(this) == true)
			{
				OpenFile(dlg.FileName);
			}
		}

		internal void OpenFile(string path)
		{
			if(String.IsNullOrEmpty(path))
				return;
			if(!this.TryCloseFile())
				return;
			try
			{
				var xdoc = XDocument.Load(path);
				var doc = new Document(xdoc.Root);
				this.ActiveDocument = doc;
				this._ActiveDocumentPath = path;
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
								FileName = String.IsNullOrEmpty(this._ActiveDocumentPath) ? null : this._ActiveDocumentPath
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

		private bool TryCloseFile()
		{
			if(!this.ActiveDocumentDirty)
				return true;
			var result = MessageBox.Show(this, "Save changes to document before closing?", "Save Changes?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
			switch(result)
			{
			case MessageBoxResult.Yes:
				OnFileSave();
				return !this.ActiveDocumentDirty;
			case MessageBoxResult.No:
				return true;
			case MessageBoxResult.Cancel:
				return false;
			default:
				return false;
			}
		}

		internal void OnFileClose()
		{
			if(!this.HasActiveDocument)
				return;
			TryCloseFile();
		}

		#endregion

		#region Event Handlers

		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.ActiveDocumentDirty;
		}

		private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.ActiveDocumentDirty;
		}

		private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.HasActiveDocument;
		}

		private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}

		private void ShowTestWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if(_TestWindow == null)
				_TestWindow = new TestWindow1 { Owner = this };
			_TestWindow.Show();
		}

		private void LoadTestDocumentCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var doc = new Document
			{
				Name = "Test Document"
			};
			var outputA = doc.CreateOutput(Guid.NewGuid());
			outputA.Name = "out A";
			doc.AddOutput(outputA);
			var outputB = doc.CreateOutput(Guid.NewGuid());
			outputB.Name = "out B";
			doc.AddOutput(outputB);
			var trackA = doc.CreateTrack(Guid.NewGuid());
			trackA.Name = "track A";
			doc.AddTrack(trackA);
			this.ActiveDocument = doc;
		}

		private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OnFileNew();
		}

		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OnFileOpen();
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

		#endregion

	}

	#endregion

}