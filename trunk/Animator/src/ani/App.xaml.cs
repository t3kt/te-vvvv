using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Animator.AppCore;
using Animator.Core.Model;

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

		#endregion

		#region Fields

		private readonly AppActionHistoryManager _ActionHistoryManager;

		#endregion

		#region Properties

		public Document ActiveDocument { get; internal set; }

		internal AppActionHistoryManager ActionHistoryManager
		{
			get { return _ActionHistoryManager; }
		}

		#endregion

		#region Constructors

		public AniApplication()
		{
			InitializeComponent();
			_ActionHistoryManager = new AppActionHistoryManager();
		}

		#endregion

		#region Methods

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ApplicationCommands.Close.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Control));
		}

		#endregion

	}
}
