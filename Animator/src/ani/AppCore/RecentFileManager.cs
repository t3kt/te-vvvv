using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Animator.Common;
using Animator.Properties;

namespace Animator.AppCore
{

	#region RecentFileManager

	internal sealed class RecentFileManager
	{

		#region Static / Constant

		#endregion

		#region Fields

		private int _Capacity;
		private readonly ObservableCollection<string> _Files;

		#endregion

		#region Properties

		public int Capacity
		{
			get { return this._Capacity; }
			set
			{
				this._Capacity = Math.Max(0, value);
				CommonUtil.CropList(this._Files, this._Capacity);
			}
		}

		public ObservableCollection<string> Files
		{
			get { return this._Files; }
		}

		#endregion

		#region Constructors

		public RecentFileManager()
		{
			this._Files = new ObservableCollection<string>();
			this.ReloadFromSettings();
		}

		#endregion

		#region Methods

		public void AddFile(string path)
		{
			if(!String.IsNullOrEmpty(path))
			{
				this._Files.Remove(path);
				this._Files.Insert(0, path);
				CommonUtil.CropList(this._Files, this._Capacity);
			}
		}

		public void RemoveFile(string path)
		{
			if(!String.IsNullOrEmpty(path))
			{
				this._Files.Remove(path);
			}
		}

		public void ReloadFromSettings()
		{
			this._Capacity = Math.Max(0, Settings.Default.RecentFileCapacity);
			var files = Settings.Default.RecentFileList;
			this._Files.Clear();
			if(files != null)
			{
				foreach(var file in files.Cast<string>().Take(this._Capacity))
					this._Files.Add(file);
			}
		}

		public void SaveToSettings()
		{
			var files = new StringCollection();
			files.AddRange(this._Files.ToArray());
			var settings = Settings.Default;
			settings.RecentFileList = files;
			settings.RecentFileCapacity = this._Capacity;
		}

		#endregion

	}

	#endregion

}
