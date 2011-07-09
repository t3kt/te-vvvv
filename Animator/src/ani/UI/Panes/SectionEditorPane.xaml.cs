using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Animator.Core.Model.Sequences;
using Animator.Core.Model.Sessions;

namespace Animator.UI.Panes
{
	/// <summary>
	/// Interaction logic for SectionEditorPane.xaml
	/// </summary>
	public partial class SectionEditorPane
	{

		#region Static / Constant

		public static readonly DependencyProperty NoEditorProperty =
			DependencyProperty.Register("NoEditor", typeof(object), typeof(SectionEditorPane),
			new PropertyMetadata(null, OnEditorChanged));

		public static readonly DependencyProperty SequenceEditorProperty =
			DependencyProperty.Register("SequenceEditor", typeof(object), typeof(SectionEditorPane),
			new PropertyMetadata(null, OnEditorChanged));

		public static readonly DependencyProperty SessionEditorProperty =
			DependencyProperty.Register("SessionEditor", typeof(object), typeof(SectionEditorPane),
			new PropertyMetadata(null, OnEditorChanged));

		private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SectionEditorPane)d).UpdateEditors();
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public object NoEditor
		{
			get { return this.GetValue(NoEditorProperty); }
			set { this.SetValue(NoEditorProperty, value); }
		}

		public object SequenceEditor
		{
			get { return this.GetValue(SequenceEditorProperty); }
			set { this.SetValue(SequenceEditorProperty, value); }
		}

		public object SessionEditor
		{
			get { return this.GetValue(SessionEditorProperty); }
			set { this.SetValue(SessionEditorProperty, value); }
		}

		#endregion

		#region Constructors

		public SectionEditorPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void UpdateEditors()
		{
			var type = this.DataContext == null ? null : this.DataContext.GetType();
			if(type == null)
				this.Content = this.NoEditor;
			else if(type == typeof(Sequence))
				this.Content = this.SequenceEditor;
			else if(type == typeof(Session))
				this.Content = this.SessionEditor;
		}

		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.UpdateEditors();
		}

		#endregion

	}

}
