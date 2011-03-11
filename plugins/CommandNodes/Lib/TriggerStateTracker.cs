using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.Core.Logging;

namespace CommandNodes
{

	#region TriggerStateTracker

	internal sealed class TriggerStateTracker : IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly HashSet<string> _Commands = new HashSet<string>(CommandUtil.CommandIdComparer);
		private bool _IsChanged;

		#endregion

		#region Properties

		internal ILogger Logger { get; set; }

		public bool this[string id]
		{
			get { return _Commands.Contains(id); }
		}

		public bool IsChanged
		{
			get { return _IsChanged; }
		}

		#endregion

		#region Events

		#endregion

		#region Constructors

		public TriggerStateTracker()
		{
			CommandRegistry.CommandTriggered += this.CommandRegistry_CommandTriggered;
		}

		#endregion

		#region Methods

		private void Log(LogType logType, string message, params object[] args)
		{
			try
			{
				if(this.Logger != null)
					this.Logger.Log(logType, message, args);
			}
			catch { }
		}

		private void CommandRegistry_CommandTriggered(object sender, CommandEventArgs e)
		{
			this.Log(LogType.Debug, "CommandTriggered: '{0}'", e.CommandId);
			Set(e.CommandId);
		}

		public void Set(string id)
		{
			if(_Commands.Add(id))
				_IsChanged = true;
		}

		public void Reset()
		{
			_Commands.Clear();
			_IsChanged = false;
		}

		public string[] GetAllSet()
		{
			return _Commands.ToArray();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Logger = null;
			CommandRegistry.CommandTriggered -= this.CommandRegistry_CommandTriggered;
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
