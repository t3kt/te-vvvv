using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OscSeq.Core.Model
{

	#region IOutput

	public interface IOutput : IGuidId
	{
		string Name { get; set; }
	}

	#endregion

}
