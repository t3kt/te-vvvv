using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Animator.AppCore;

namespace Animator.UI.Controls
{
	/// <summary>
	/// Interaction logic for RecentFilesMenuItem.xaml
	/// </summary>
	public partial class RecentFilesMenuItem
	{

		#region Static / Constant

		private static string FormatRecordHeader(string fileName, int index)
		{
			if(index < 10)
				return String.Format("_{0} {1}", index + 1, fileName);
			if(index == 10)
				return String.Format("_0 {0}", fileName);
			return fileName;
		}

		#endregion

		#region Fields

		private ObservableCollection<string> _RecentFiles;

		#endregion

		#region Properties

		public IEnumerable<KeyValuePair<string, string>> RecentFileRecords
		{
			get
			{
				if(this._RecentFiles == null && !this.LoadRecentFilesSource())
					return Enumerable.Empty<KeyValuePair<string, string>>();
				return this._RecentFiles.Reverse().Select((file, i) => new KeyValuePair<string, string>(file, FormatRecordHeader(file, i)));
			}
		}

		#endregion

		#region Constructors

		public RecentFilesMenuItem()
		{
			this.InitializeComponent();
		}

		private bool LoadRecentFilesSource()
		{
			var mgr = AniApplication.GetAppService<RecentFileManager>(required: false);
			if(mgr == null)
				return false;
			this._RecentFiles = mgr.Files;
			this._RecentFiles.CollectionChanged += this.RecentFiles_CollectionChanged;
			return true;
		}

		private void RecentFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var bindExpr = this.GetBindingExpression(DataContextProperty);
			if(bindExpr != null)
				bindExpr.UpdateTarget();
		}

		#endregion

		#region Methods

		#endregion

	}
}
