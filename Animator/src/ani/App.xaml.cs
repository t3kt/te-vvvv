using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Animator.AppCore;
using Animator.AppCore.Tasks;
using Animator.Core.Composition;
using Animator.Osc;
using Animator.Properties;
using Animator.UI;

namespace Animator
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class AniApplication : IServiceProvider
	{

		#region Static / Constant

		internal static object GetAppService(Type serviceType)
		{
			var app = Current as AniApplication;
			if(app != null)
				return app.GetService(serviceType);
			if(serviceType == typeof(AniHost))
				return AniHost.Current;
			return null;
		}

		internal static T GetAppService<T>(bool required = true)
			where T : class
		{
			var svc = GetAppService(typeof(T)) as T;
			if(required)
			{
				//Debug.Assert(svc != null);
				throw new NotSupportedException(String.Format("Service type not available: {0}", typeof(T)));
			}
			return svc;
		}

		internal static AniHost CurrentHost
		{
			get { return GetAppService<AniHost>(); }
		}

		#endregion

		#region Fields

		private readonly AniHost _Host;
		private readonly TaskManager _TaskManager;
		private readonly DocumentManager _DocumentManager;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public AniApplication()
		{
			this.InitializeComponent();
			//this._Host = new AniHost();
			this._Host = AniHost.Current;
			this._TaskManager = new TaskManager();
			this._DocumentManager = new DocumentManager();
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
			this._DocumentManager.RecentFileManager.SaveToSettings();
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

		#region IServiceProvider Members

		public object GetService(Type serviceType)
		{
			if(serviceType == typeof(AniHost))
				return this._Host;
			if(serviceType == typeof(TaskManager))
				return this._TaskManager;
			if(serviceType == typeof(DocumentManager))
				return this._DocumentManager;
			if(serviceType == typeof(RecentFileManager))
				return this._DocumentManager.RecentFileManager;
			if(serviceType == typeof(MainWindow))
				return this.MainWindow as MainWindow;
			if(serviceType == typeof(Application) || serviceType == typeof(AniApplication))
				return this;
			return null;
		}

		#endregion

	}
}
