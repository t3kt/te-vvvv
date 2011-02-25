using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VVVV.Core.Logging;
using VVVV.Lib;
using VVVV.Nodes;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.UI
{
	[PluginInfo(Name = Names.Nodes.GetTypes,
		Category = Names.Category,
		Version = "test",
		Author = Names.Author,
		AutoEvaluate = true,
		InitialComponentMode = TComponentMode.InABox,
		InitialBoxHeight = 200,
		InitialWindowHeight = 200,
		InitialBoxWidth = 100,
		InitialWindowWidth = 100)]
	public partial class GlobalStructTypesPanel : UserControl, IPluginEvaluate
	{

		#region Nested Types

		#region TypeRecord

		protected sealed class TypeRecord
		{

			#region Static/Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			public string FriendlyTypeName { get; set; }

			public string PartTypesKey { get; set; }

			public Guid Id { get; set; }

			public int Usages { get; set; }

			#endregion

			#region Constructors

			#endregion

			#region Methods

			#endregion

		}

		#endregion

		#endregion

		#region Static / Constant

		protected static IEnumerable<TypeRecord> GetTypeRecords()
		{
			return from type in StructTypeRegistry.RegisteredTypes
				   select new TypeRecord
						  {
							  FriendlyTypeName = type.FriendlyTypeName,
							  Id = type.Id,
							  PartTypesKey = type.PartTypesKey,
							  Usages = StructTypeRegistry.GetTypeUsageCount(type.Id)
						  };
		}

		#endregion

		#region Fields

		[Input("RefreshTypes", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _RefreshTypesInput;

		//[Import]
		//private ILogger _Log;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public GlobalStructTypesPanel(IPluginHost host)
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		public virtual void RefreshTypes()
		{
			//_Log.Log(LogType.Debug, "StructTypeDisplay Refreshing Types");
			var typeRecords = GetTypeRecords().OrderBy(r => r.FriendlyTypeName).ToList();
			_TypesDataGridView.DataSource = typeRecords;
		}

		#endregion


		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_RefreshTypesInput.IsChanged)
			{
				RefreshTypes();
			}
		}

		#endregion
	}
}
