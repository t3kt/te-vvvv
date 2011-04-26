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
using Animator.AppCore;
using Animator.Core.Model;
using Animator.Core.Runtime;

namespace Animator.UI.Panes
{
	/// <summary>
	/// Interaction logic for ClipEditorPane.xaml
	/// </summary>
	public partial class ClipEditorPane
	{

		#region Static / Constant

		public static readonly DependencyProperty ActiveClipProperty;
		private static readonly DependencyPropertyKey DataEditorPropertyKey;
		public static readonly DependencyProperty DataEditorProperty;

		static ClipEditorPane()
		{
			ActiveClipProperty = DependencyProperty.Register("ActiveClip", typeof(Clip), typeof(ClipEditorPane),
				new FrameworkPropertyMetadata(OnActiveClipChanged));
			DataEditorPropertyKey = DependencyProperty.RegisterReadOnly("DataEditor", typeof(UIElement), typeof(ClipEditorPane),
				new FrameworkPropertyMetadata());
			DataEditorProperty = DataEditorPropertyKey.DependencyProperty;
		}

		private static void OnActiveClipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var pane = (ClipEditorPane)d;
			var clip = (Clip)e.NewValue;
			pane.DataContext = clip;
			if(clip == null)
			{
				pane.DataEditor = null;
			}
			else
			{
				var editorElement = pane.DataEditor;
				var editor = editorElement as IClipDataEditor;
				bool isReusable;
				var requestedEditorType = ClipDataEditors.GetEditorType(clip.GetType(), out isReusable);
				if(isReusable && editor != null && editor.GetType() == requestedEditorType)
				{
					editor.Clip = clip;
				}
				else
				{
					editor = ClipDataEditors.GetEditor(clip.GetType());
					if(editor != null && editor is UIElement)
					{
						pane.DataEditor = (UIElement)editor;
						editor.Clip = clip;
					}
					else
					{
						pane.DataEditor = null;
					}
				}
			}
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Clip ActiveClip
		{
			get { return (Clip)this.GetValue(ActiveClipProperty); }
			set { this.SetValue(ActiveClipProperty, value); }
		}

		public UIElement DataEditor
		{
			get { return (UIElement)this.GetValue(DataEditorProperty); }
			private set { this.SetValue(DataEditorPropertyKey, value); }
		}

		#endregion

		#region Constructors

		public ClipEditorPane()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		#endregion

	}
}
