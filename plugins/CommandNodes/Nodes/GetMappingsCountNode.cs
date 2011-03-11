using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace CommandNodes.Nodes
{

	#region GetMappingsCountNode

	[PluginInfo(Name = TEShared.Names.Nodes.Count,
		Category = TEShared.Names.Categories.Command,
		Version = TEShared.Names.Versions.Global,
		Author = TEShared.Names.Author)]
	public sealed class GetMappingsCountNode : IPluginBase, IDisposable, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Update", IsBang = true, IsSingle = true)]
		private IDiffSpread<bool> _UpdateInput;

		[Output("Count", IsSingle = true)]
		private ISpread<int> _CountOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		private void UpdateInput_Changed(IDiffSpread<bool> updateInput)
		{
			if(updateInput[0])
				_CountOutput[0] = CommandRegistry.TotalMappingsCount;
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			_UpdateInput.Changed += this.UpdateInput_Changed;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if(_UpdateInput != null)
				_UpdateInput.Changed -= this.UpdateInput_Changed;
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
