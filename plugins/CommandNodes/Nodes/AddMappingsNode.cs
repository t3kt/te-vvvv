using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region AddMappingsNode

	[PluginInfo(Name = CommandNames.Nodes.Mappings,
		Category = CommandNames.Categories.Command,
		Version = TEShared.Names.Versions.Add,
		Author = TEShared.Names.Author,
		AutoEvaluate = true,
		Help = "Add Command Mappings parsed from encoded mapping strings")]
	public sealed class AddMappingsNode : IPluginBase, IDisposable, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Import]
		private ILogger _Logger;

		[Config("Clear On Update", IsSingle = true, DefaultValue = 1)]
		private ISpread<bool> _ClearOnUpdateConfig;

		[Input("Mapping Parts", BinSize = 3)]
		private ISpread<ISpread<string>> _MappingPartsInput;

		[Input("Do Update", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _DoUpdateInput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void DoUpdateInput_Changed(IDiffSpread<bool> doUpdateInput)
		{
			//if(!doUpdateInput[0])
			//    return;
			if(_ClearOnUpdateConfig[0])
				CommandRegistry.ClearMappings();
			foreach(var mappingParts in _MappingPartsInput)
			{
				if(mappingParts.SliceCount == 3)
				{
					var mapping = CommandUtil.ParseMapping(mappingParts[0], mappingParts[1], mappingParts[2], _Logger);
					CommandRegistry.AddMapping(mapping, _Logger);
				}
			}
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_DoUpdateInput.Changed += this.DoUpdateInput_Changed;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(_DoUpdateInput != null)
				_DoUpdateInput.Changed -= this.DoUpdateInput_Changed;
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
