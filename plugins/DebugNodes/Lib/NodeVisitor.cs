using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TESharedAnnotations;
using VVVV.PluginInterfaces.V2.Graph;

namespace VVVV.Lib
{

	#region AnonVisitor

	internal sealed class AnonVisitor : NodeVisitor
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		public Func<INode2, bool> NodeFilter { get; set; }

		public bool IgnorePins { get; set; }

		public bool IgnoreChildren { get; set; }

		public Action<INode2> NodeAction { get; set; }

		public Action<INode2> NodePostAction { get; set; }

		public Action<INode2, IPin2> PinAction { get; set; }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override void VisitNodeInternal(INode2 node)
		{
			if(this.NodeFilter != null && !this.NodeFilter(node))
				return;
			if(this.NodeAction != null)
				this.NodeAction(node);
			if(!this.IgnorePins)
				this.VisitPins(node);
			if(!this.IgnoreChildren)
				this.VisitChildren(node);
			if(this.NodePostAction != null)
				this.NodePostAction(node);
		}

		protected override void VisitPin(INode2 node, IPin2 pin)
		{
			if(this.PinAction != null)
				this.PinAction(node, pin);
		}

		#endregion

	}

	#endregion

	#region NodeVisitor

	internal abstract class NodeVisitor
	{

		#region Static/Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		public void VisitNode([CanBeNull] INode2 node)
		{
			if(node == null)
				return;
			this.VisitNodeInternal(node);
		}

		protected virtual void VisitNodeInternal([NotNull] INode2 node)
		{
			this.VisitPins(node);
			this.VisitChildren(node);
		}

		protected virtual void VisitChildren([NotNull] INode2 node)
		{
			foreach(var child in node)
				this.VisitNode(child);
		}

		protected virtual void VisitPins([NotNull] INode2 node)
		{
			foreach(var pin in node.Pins)
				this.VisitPin(node, pin);
		}

		protected virtual void VisitPin([NotNull] INode2 node, [NotNull] IPin2 pin)
		{
		}

		#endregion

	}

	#endregion
}
