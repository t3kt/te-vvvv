using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using VVVV.Core;
using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.Nodes
{

	#region PartCatalogDebugNode

	[PluginInfo(Name = "PartCatalog",
		Category = TEShared.Names.Categories.Debug,
		Author = TEShared.Names.Author)]
	public sealed class PartCatalogDebugNode : IPluginEvaluate
	{

		#region Static / Constant

		private static Type _ReflectionPartDefType;
		private static Func<object, Type> _ReflectionPartDefGetPartType;

		private static Type GetReflectionPartDefPartType(ComposablePartDefinition part)
		{
			if(_ReflectionPartDefType == null)
				_ReflectionPartDefType = typeof(ComposablePartDefinition).Assembly.GetType("System.ComponentModel.Composition.ReflectionModel.ReflectionComposablePartDefinition", true);
			if(!_ReflectionPartDefType.IsInstanceOfType(part))
				return null;
			if(_ReflectionPartDefGetPartType == null)
			{
				var method = _ReflectionPartDefType.GetMethod("GetPartType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				_ReflectionPartDefGetPartType = (Func<object, Type>)Delegate.CreateDelegate(typeof(Func<object, Type>), method, true);
			}
			if(_ReflectionPartDefGetPartType == null)
				return null;
			return _ReflectionPartDefGetPartType(part);
		}

		private static IEnumerable<string> GetPartsInfo()
		{
			return Shell.Instance.Container.Catalog.Parts
				.Where(p => p != null)
				.Select(FormatInfo);
		}

		private static string FormatInfo(ComposablePartDefinition part)
		{
			var sb = new StringBuilder("<part> ");
			sb.AppendFormat("DefType: {0} ", part.GetType().FullName);
			if(part is ICompositionElement)
				sb.AppendFormat("DisplayName: {0} ", ((ICompositionElement)part).DisplayName);
			var partType = GetReflectionPartDefPartType(part);
			if(partType != null)
				sb.AppendFormat("PartType: {0} ", partType.FullName);
			//...?
			return sb.ToString();
		}

		#endregion

		#region Fields

		[Input("DumpToLog", IsBang = true, IsSingle = true)]
		private IDiffSpread<bool> _DumpToLogInput;

		[Input("Refresh", IsBang = true, IsSingle = true)]
		private IDiffSpread<bool> _RefreshInput;

		[Output("PartInfo")]
		private ISpread<string> _PartInfoOutput;

		private bool _NeedsUpdate = true;

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
			if(_NeedsUpdate || _RefreshInput.IsChanged || _DumpToLogInput.IsChanged)
			{
				var partsInfo = GetPartsInfo().ToList();
				if(_NeedsUpdate || _RefreshInput.IsChanged)
				{
					_PartInfoOutput.AssignFrom(partsInfo);
				}
				if(_DumpToLogInput.IsChanged)
				{
					foreach(var partInfo in partsInfo)
						Shell.Instance.Logger.Log(LogType.Debug, partInfo);
				}
				_NeedsUpdate = false;
			}
		}

		#endregion

	}

	#endregion

}
