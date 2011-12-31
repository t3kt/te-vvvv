using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;
using VVVV.Lib;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V2.Graph;

namespace VVVV.Nodes
{

	#region NodeTreeLoggerNode

	[PluginInfo(Name = "NodeTreeLogger",
		Category = TEShared.Names.Categories.Debug,
		Author = TEShared.Names.Author)]
	public sealed class NodeTreeLoggerNode : IPluginBase, IPartImportsSatisfiedNotification, IDisposable
	{

		#region LoggerVisitor

		internal class LoggerVisitor : NodeVisitor
		{

			#region Static/Constant

			private const char IndentChar = '\t';

			#endregion

			#region Fields

			private readonly StringBuilder _Output;
			private int _Indent;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			public LoggerVisitor()
			{
				_Output = new StringBuilder();
			}

			#endregion

			#region Methods

			private void Indent()
			{
				_Indent++;
			}

			private void Unindent()
			{
				if(_Indent > 0)
					_Indent--;
			}

			private void WriteIndent()
			{
				if(_Indent > 0)
					_Output.Append(IndentChar, _Indent);
			}

			private void WriteNode(INode2 node)
			{
				this.WriteIndent();
				this.WriteNodeBasics(node);
				this.WriteNodeInfo(node);
				_Output.AppendLine();
			}

			private void WriteNodeBasics(INode2 node)
			{
				_Output.AppendFormat("[N| {0} #{1} ", node.Name, node.ID);
				if(node.HasCode || node.HasGUI || node.HasPatch)
				{
					_Output.Append("<");
					if(node.HasCode)
						_Output.Append("c");
					if(node.HasGUI)
						_Output.Append("g");
					if(node.HasPatch)
						_Output.Append("p");
					_Output.Append(">");
				}

				_Output.Append("]");
			}

			private void WriteNodeInfo(INode2 node)
			{
				var info = node.NodeInfo;
				_Output.Append("[I| ");
				_Output.AppendFormat("t:{0} n:{1}", info.Type, info.Name);
				if(!String.IsNullOrEmpty(info.Category))
					_Output.AppendFormat(" c:{0}", info.Category);
				if(!String.IsNullOrEmpty(info.Version))
					_Output.AppendFormat(" v:{0}", info.Version);
				if(!String.IsNullOrEmpty(info.Arguments))
					_Output.AppendFormat(" args:{0}", info.Arguments);
				if(info.AutoEvaluate)
					_Output.Append(" <auto>");
				if(info.Ignore)
					_Output.Append(" <ignore>");
				if(info.UserData != null)
					_Output.AppendFormat(" udata:{0}", info.UserData);
				_Output.Append("]");
			}

			protected override void VisitNodeInternal(INode2 node)
			{
				this.WriteNode(node);
				base.VisitNodeInternal(node);
			}

			protected override void VisitChildren(INode2 node)
			{
				this.Indent();
				base.VisitChildren(node);
				this.Unindent();
			}

			public override string ToString()
			{
				return _Output.ToString();
			}

			#endregion

		}

		#endregion

		#region Static/Constant

		#endregion

		#region Fields

		private readonly IHDEHost _HDEHost;

		[Import]
		private ILogger _Logger;

		[Input("Do Dump", IsSingle = true, IsBang = true)]
		private IDiffSpread<bool> _DoDumpInput;

		[Output("Dump Output", IsSingle = true)]
		private ISpread<string> _DumpOutput;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		[ImportingConstructor]
		public NodeTreeLoggerNode(IHDEHost hdeHost)
		{
			this._HDEHost = hdeHost;
		}

		#endregion

		#region Methods

		private void PerformDump()
		{
			var visitor = new LoggerVisitor();
			visitor.VisitNode(_HDEHost.RootNode);
			var dump = visitor.ToString();
			_Logger.Log(LogType.Debug, dump);
			_DumpOutput[0] = dump;
		}

		public void OnImportsSatisfied()
		{
			this._DoDumpInput.Changed += this.DoDumpInput_Changed;
		}

		private void DoDumpInput_Changed(IDiffSpread<bool> spread)
		{
			this.PerformDump();
		}

		public void Dispose()
		{
			if(this._DoDumpInput != null)
				this._DoDumpInput.Changed -= this.DoDumpInput_Changed;
		}

		#endregion

	}

	#endregion
}
