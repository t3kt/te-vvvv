using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region ParseStringStructNode

	[PluginInfo(Name = TEShared.Names.Nodes.Parse,
		Category = TEShared.Names.Categories.Struct,
		Version = TEShared.Names.Categories.String,
		Author = TEShared.Names.Author)]
	public sealed class ParseStringStructNode : StructNodeBase
	{

		#region Static / Constant

		private static string GetPartTypesKey(int count)
		{
			if(count <= 0)
				return "S";
			return new String('S', count);
		}

		#endregion

		#region Fields

		private readonly IDiffSpread<int> _PartCountConfig;
		private readonly IDiffSpread<string> _SeparatorConfig;

		[Input("Input String")]
		private IDiffSpread<string> _StrInput;

		[Output("Output")]
		private ISpread<StructData> _Output;

		private string[] _Separators;
		private bool _NeedsUpdate;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public ParseStringStructNode(IPluginHost host, [Import] ILogger logger, [Config("PartCount", IsSingle = true, MinValue = 1, DefaultValue = 2)]IDiffSpread<int> partCountConfig, [Config("Separator String")]IDiffSpread<string> separatorConfig)
			: base(host, logger)
		{
			_PartCountConfig = partCountConfig;
			_PartCountConfig.Changed += this.PartCountConfig_Changed;
			_SeparatorConfig = separatorConfig;
			_SeparatorConfig.Changed += this.SeparatorConfig_Changed;
		}

		#endregion

		#region Methods

		private void PartCountConfig_Changed(IDiffSpread<int> partCountConfig)
		{
			CheckDisposed();
			SetType(GetPartTypesKey(partCountConfig[0]));
			_NeedsUpdate = true;
		}

		private void SeparatorConfig_Changed(IDiffSpread<string> separatorConfig)
		{
			_Separators = separatorConfig.ToArray();
			_NeedsUpdate = true;
		}

		public override void Evaluate(int spreadMax)
		{
			if(_NeedsUpdate || _StrInput.IsChanged)
			{
				var count = Math.Min(spreadMax, _StrInput.SliceCount);
				_Output.SliceCount = count;
				for(var i = 0; i < count; i++)
				{
					if(String.IsNullOrEmpty(_StrInput[i]))
					{
						_Output[i] = null;
					}
					else
					{
						var strParts = _StrInput[i].Split(_Separators, StringSplitOptions.None);
						try
						{
							_Output[i] = this.Type.ParseInstance(strParts);
						}
						catch(FormatException ex)
						{
							_Host.Log(TLogType.Error, String.Format("Error parsing string struct: {0}", ex.Message));
							_Output[i] = null;
						}
					}
				}
				_NeedsUpdate = false;
			}
		}

		protected override void Dispose()
		{
			if(_PartCountConfig != null)
				_PartCountConfig.Changed -= this.PartCountConfig_Changed;
			if(_SeparatorConfig != null)
				_SeparatorConfig.Changed -= this.SeparatorConfig_Changed;
			base.Dispose();
		}

		#endregion

	}

	#endregion

}
