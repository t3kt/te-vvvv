using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandNodes
{

	#region TriggerStateSet

	internal sealed class TriggerStateSet
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly HashSet<string> _Commands = new HashSet<string>(CommandUtil.CommandIdComparer);
		private bool _IsChanged;

		#endregion

		#region Properties

		public bool this[string id]
		{
			get { return _Commands.Contains(id); }
		}

		public bool IsChanged
		{
			get { return _IsChanged; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

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

	}

	#endregion

}
