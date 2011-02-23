using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;

namespace VVVV.Nodes
{

	#region GetStructTypesNode

	public sealed class GetStructTypesNode : IPlugin, IDisposable
	{

		#region Static / Constant

		private static IPluginInfo _PluginInfo;

		public static IPluginInfo PluginInfo
		{
			get
			{
				if(_PluginInfo == null)
				{
					_PluginInfo = new PluginInfo
					{
						Name = "GetTypes",
						Category = "Struct",
						Author = "te",
						Class = typeof(GetStructTypesNode).Name,
						Namespace = typeof(GetStructTypesNode).Namespace
					};
				}
				return _PluginInfo;
			}
		}

		#endregion

		#region Fields

		private bool _Invalidate = true;
		private IValueIn _RefreshInput;
		private IStringOut _PartTypeKeysOutput;
		private IStringOut _GuidsOutput;
		private IStringOut _FriendlyNamesOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public GetStructTypesNode()
		{
			StructTypeRegistry.TypeRegistered += this.TypeRegistry_Changed;
			StructTypeRegistry.TypeUnregistered += this.TypeRegistry_Changed;
		}

		#endregion

		#region Methods

		private void TypeRegistry_Changed(object sender, StructTypeEventArgs e)
		{
			_Invalidate = true;
		}

		#endregion

		#region IPlugin Members

		public bool AutoEvaluate
		{
			get { return false; }
		}

		public void Configurate(IPluginConfig input)
		{
		}

		public void Evaluate(int spreadMax)
		{
			if(_Invalidate || _RefreshInput.PinIsChanged)
			{
				var types = StructTypeRegistry.RegisteredTypes.OrderBy(t => t.PartTypesKey).ToArray();
				_PartTypeKeysOutput.SliceCount = _GuidsOutput.SliceCount = _FriendlyNamesOutput.SliceCount = types.Length;
				for(var i = 0; i < types.Length; i++)
				{
					_PartTypeKeysOutput.SetString(i, types[i].PartTypesKey);
					_GuidsOutput.SetString(i, types[i].Id.ToString());
					_FriendlyNamesOutput.SetString(i, types[i].FriendlyTypeName);
				}
				_Invalidate = false;
			}
		}

		public void SetPluginHost(IPluginHost host)
		{
			host.CreateValueInput("Refresh", 1, null, TSliceMode.Single, TPinVisibility.True, out _RefreshInput);
			_RefreshInput.SetSubType(0, 1, 1, 0, true, false, false);
			host.CreateStringOutput("PartTypeKeys", TSliceMode.Dynamic, TPinVisibility.True, out _PartTypeKeysOutput);
			_PartTypeKeysOutput.SetSubType(String.Empty, false);
			host.CreateStringOutput("Guids", TSliceMode.Dynamic, TPinVisibility.Hidden, out _GuidsOutput);
			_GuidsOutput.SetSubType(String.Empty, false);
			host.CreateStringOutput("FriendlyNames", TSliceMode.Dynamic, TPinVisibility.Hidden, out _FriendlyNamesOutput);
			_FriendlyNamesOutput.SetSubType(String.Empty, false);
			_Invalidate = true;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			StructTypeRegistry.TypeRegistered -= this.TypeRegistry_Changed;
			StructTypeRegistry.TypeUnregistered -= this.TypeRegistry_Changed;
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
