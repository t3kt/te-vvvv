using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OscSeq.Core.Model
{

	#region ITrack

	public interface ITrack : IGuidId
	{
		string Name { get; set; }
		IKeyedCollection<Guid, IClip> Clips { get; set; }
	}

	#endregion

}
