using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.Composition;
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

		internal static AniHost CurrentHost
		{
			get { return ((AniApplication)Current).Host; }
		}

		#endregion

		#region Fields

		private readonly RecentFileManager _RecentFileManager;
		private readonly AniHost _Host;

		#endregion

		#region Properties

		public Document ActiveDocument { get; internal set; }

		internal RecentFileManager RecentFileManager
		{
			get { return _RecentFileManager; }
		}

		public AniHost Host
		{
			get { return this._Host; }
		}

		#endregion

		#region Constructors

		public AniApplication()
		{
			this.InitializeComponent();
			this._RecentFileManager = new RecentFileManager();
			this._Host = new AniHost();
		}

		#endregion

		#region Methods

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ApplicationCommands.Close.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Control));
			this.RegisterLoadedAssemblies();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			this._RecentFileManager.SaveToSettings();
			Settings.Default.Save();
			base.OnExit(e);
		}

		private void RegisterLoadedAssemblies()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
#warning TODO: find a better way of ensuring that the aniosc assembly is loaded...
			assemblies.Add(typeof(OscOutput).Assembly);
			foreach(var assembly in assemblies.Distinct())
				this._Host.LoadAssembly(assembly);
		}

		#endregion

	}
}
