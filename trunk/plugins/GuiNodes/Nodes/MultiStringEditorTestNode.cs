using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{
	[PluginInfo(Name = "MultiStringEditorTest",
		Category = TEShared.Names.Categories.GUI,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		InitialComponentMode = TComponentMode.InABox,
		InitialBoxHeight = 100,
		InitialWindowHeight = 100,
		InitialBoxWidth = 100,
		InitialWindowWidth = 100)]
	public partial class MultiStringEditorTestNode : UserControl, IPluginEvaluate, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Config("Rows", AsInt = true, IsSingle = true, MinValue = 1)]
		private IDiffSpread<int> _Rows;

		[Config("Columns", AsInt = true, IsSingle = true, MinValue = 1)]
		private IDiffSpread<int> _Columns;

		[Config("Entries", AsInt = true, IsSingle = true, MinValue = 0)]
		private IDiffSpread<int> _Entries;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public MultiStringEditorTestNode()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void Rows_Changed(IDiffSpread<int> spread)
		{
			throw new NotImplementedException();
		}

		private void Columns_Changed(IDiffSpread<int> spread)
		{
			throw new NotImplementedException();
		}

		private void Entries_Changed(IDiffSpread<int> spread)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_Rows.Changed += this.Rows_Changed;
			_Columns.Changed += this.Columns_Changed;
			_Entries.Changed += this.Entries_Changed;
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
		}

		#endregion

	}
}
