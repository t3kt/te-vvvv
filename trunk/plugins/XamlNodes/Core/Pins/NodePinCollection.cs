using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace XamlNodes.Core.Pins
{

	#region NodePinCollection

	public sealed class NodePinCollection : Collection<NodePin>, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal void AttachHost(IXamlNodeHost host)
		{
			foreach(var pin in this)
				pin.AttachHost(host);
		}

		protected override void InsertItem(int index, NodePin item)
		{
			if(item == null)
				throw new ArgumentNullException("item");
			base.InsertItem(index, item);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			while(this.Count > 0)
			{
				this[0].Dispose();
				this.RemoveAt(0);
			}
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
