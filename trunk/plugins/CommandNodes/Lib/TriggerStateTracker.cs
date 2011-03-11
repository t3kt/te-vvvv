using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region TriggerStateTracker

	internal sealed class TriggerStateTracker : IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly TriggerStateSet _States;

		#endregion

		#region Properties

		public TriggerStateSet States
		{
			get { return _States; }
		}

		#endregion

		#region Constructors

		public TriggerStateTracker()
		{
			_States = new TriggerStateSet();
			CommandRegistry.CommandTriggered += this.CommandRegistry_CommandTriggered;
		}

		#endregion

		#region Methods

		private void CommandRegistry_CommandTriggered(object sender, CommandEventArgs e)
		{
			_States.Set(e.CommandId);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			CommandRegistry.CommandTriggered -= this.CommandRegistry_CommandTriggered;
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
