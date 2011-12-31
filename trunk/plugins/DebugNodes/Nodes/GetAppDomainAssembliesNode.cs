using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region GetAppDomainAssembliesNode

	[PluginInfo(Name = "Assemblies",
		Category = TEShared.Names.Categories.Debug,
		Author = TEShared.Names.Author)]
	public sealed class GetAppDomainAssembliesNode : IPluginEvaluate
	{

		#region Static / Constant

		#endregion

		#region Fields

		[Input("Refresh", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _RefreshInput;

		[Output("FullNames")]
		private ISpread<string> _FullNameOutput;

		[Output("Locations")]
		private ISpread<string> _LocationOutput;

		[Output("CodeBases")]
		private ISpread<string> _CodeBaseOutput;

		[Output("Assemblies", Visibility = PinVisibility.OnlyInspector)]
		private ISpread<Assembly> _AssemblyOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_RefreshInput.IsChanged)
			{
				var assemblies = AppDomain.CurrentDomain.GetAssemblies()
					.Where(a => !(a.ManifestModule is ModuleBuilder))
					.OrderBy(a => a.FullName, StringComparer.OrdinalIgnoreCase)
					.ToArray();
				_FullNameOutput.SliceCount =
					_LocationOutput.SliceCount =
					_CodeBaseOutput.SliceCount =
					_AssemblyOutput.SliceCount =
					assemblies.Length;
				for(var i = 0; i < assemblies.Length; i++)
				{
					_FullNameOutput[i] = assemblies[i].FullName;
					_LocationOutput[i] = assemblies[i].Location;
					_CodeBaseOutput[i] = assemblies[i].CodeBase;
					_AssemblyOutput[i] = assemblies[i];
				}
			}
		}

		#endregion

	}

	#endregion

}
