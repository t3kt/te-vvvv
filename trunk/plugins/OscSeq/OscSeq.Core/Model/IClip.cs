using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OscSeq.Core.Model
{

	#region IClip

	public interface IClip : IGuidId
	{
		string Name { get; set; }
	}

	#endregion

}
