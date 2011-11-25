using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OscSeq.Core.Model
{

	#region IDocument

	public interface IDocument
	{
		string Name { get; set; }
		IKeyedCollection<Guid, ITrack> Tracks { get; }
		IKeyedCollection<Guid, IOutput> Outputs { get; }
	}

	#endregion

}
