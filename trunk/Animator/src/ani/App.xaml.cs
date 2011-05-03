﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Transport;
using Animator.Osc;
using Animator.Properties;

namespace Animator
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class AniApplication
	{

		#region Static / Constant

		internal static Document CurrentActiveDocument
		{
			get
			{
				var app = Application.Current as AniApplication;
				return app == null ? null : app.ActiveDocument;
			}
		}

		public static void RegisterLoadedAssemblies()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
#warning TODO: find a better way of ensuring that the aniosc assembly is loaded...
			assemblies.Add(typeof(OscTransmitter).Assembly);
			foreach(var assembly in assemblies)
			{
				Clip.TypeRegistry.RegisterTypes(assembly);
				OutputTransmitter.TypeRegistry.RegisterTypes(assembly);
				Transport.TypeRegistry.RegisterTypes(assembly);
			}
		}

		#endregion

		#region Fields

		private readonly AppActionHistoryManager _ActionHistoryManager;
		private readonly RecentFileManager _RecentFileManager;

		#endregion

		#region Properties

		public Document ActiveDocument { get; internal set; }

		internal AppActionHistoryManager ActionHistoryManager
		{
			get { return this._ActionHistoryManager; }
		}

		internal RecentFileManager RecentFileManager
		{
			get { return _RecentFileManager; }
		}

		#endregion

		#region Constructors

		public AniApplication()
		{
			this.InitializeComponent();
			this._ActionHistoryManager = new AppActionHistoryManager();
			this._RecentFileManager = new RecentFileManager();
		}

		#endregion

		#region Methods

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ApplicationCommands.Close.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Control));
			RegisterLoadedAssemblies();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			this._RecentFileManager.SaveToSettings();
			Settings.Default.Save();
			base.OnExit(e);
		}

		#endregion

	}
}
