using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
	[PluginInfo(Name = TEShared.Names.Nodes.GetTypes,
		Category = TEShared.Names.Categories.Struct,
		Version = TEShared.Names.Versions.GUI,
		Author = TEShared.Names.Author,
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

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public GlobalStructTypesPanel(IPluginHost host, ILogger logger)
		{
			InitializeComponent();
			this._TypesDataGridView.DefaultCellStyle.NullValue = null;
			StructTypeRegistry.OfferLogger(logger);
			StructTypeRegistry.TypeRegistered += this.TypeRegistry_Changed;
			StructTypeRegistry.TypeUnregistered += this.TypeRegistry_Changed;
			StructTypeRegistry.TypeUsageCountChanged += this.TypeRegistry_CountChanged;
		}

		#endregion

		#region Methods

		public virtual void RefreshTypes()
		{
			//_Log.Log(LogType.Debug, "StructTypeDisplay Refreshing Types");
			var typeRecords = GetTypeRecords().OrderBy(r => r.FriendlyTypeName).ToList();
			_TypesDataGridView.DataSource = typeRecords;
		}

		private void TypeRegistry_Changed(object sender, StructTypeEventArgs e)
		{
			RefreshTypes();
		}

		private void TypeRegistry_CountChanged(object sender, CountChangedEventArgs<Guid> e)
		{
			RefreshTypes();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
				StructTypeRegistry.RescindLogger();
				StructTypeRegistry.TypeRegistered -= this.TypeRegistry_Changed;
				StructTypeRegistry.TypeUnregistered -= this.TypeRegistry_Changed;
				StructTypeRegistry.TypeUsageCountChanged -= this.TypeRegistry_CountChanged;
			}
			base.Dispose(disposing);
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
