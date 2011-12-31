using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.Utils.Linq;

namespace VVVV.Nodes
{

	#region NodesTestNode

	[PluginInfo(Name = "NodesTest",
		Category = TEShared.Names.Categories.Debug,
		Author = TEShared.Names.Author)]
	public sealed class NodesTestNode : IPluginEvaluate, IPartImportsSatisfiedNotification
	{

		#region Static / Constant

		private static INode2 FindNodeFromInternal(IHDEHost hdeHost, INode internalNode)
		{
			var nodes = hdeHost.RootNode.AsDepthFirstEnumerable();
			return nodes.FirstOrDefault(n => n.InternalCOMInterf == internalNode);
		}

		#endregion

		#region Fields

		private IPluginHost _PluginHost;
		private IHDEHost _HDEHost;
		private INode _InternalNode;
		private INode2 _Node;
		private ILogger _Logger;

		[Input("Bang", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _BangInput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public NodesTestNode(IPluginHost pluginHost, IHDEHost hdeHost, INode internalNode, [Import]ILogger logger)
		{
			_PluginHost = pluginHost;
			_HDEHost = hdeHost;
			_Logger = logger;
			_InternalNode = internalNode;
			Log(".ctor");
		}

		#endregion

		#region Methods

		private void Log(string message, params object[] args)
		{
			if(_Logger != null)
			{
				_Logger.Log(LogType.Debug, "NodesTest: " + String.Format(message, args));
			}
		}

		private void LocateNode()
		{
			_Node = FindNodeFromInternal(_HDEHost, _InternalNode);
			if(_Node != null)
			{
				Log("Found node from internal");
			}
		}

		private void DoStuff()
		{
			Log("do stuff...");
		}

		#endregion

		#region IPartImportsSatisfiedNotification Members

		public void OnImportsSatisfied()
		{
			Log("OnImportsSatisfied");
		}

		#endregion

		#region IPluginEvaluate Members

		public void Evaluate(int spreadMax)
		{
			if(_BangInput.IsChanged || _BangInput[0])
			{
				Log("Evaluate (changed)");
				if(_Node == null)
					LocateNode();
				if(_Node != null)
					DoStuff();
			}
		}

		#endregion

	}

	#endregion

}
