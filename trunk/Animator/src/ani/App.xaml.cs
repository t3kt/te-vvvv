using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
	public partial class App : Application
	{

		#region Static / Constant

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

		public App()
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
