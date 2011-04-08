using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Core.Model
{

	#region IClipContainer

	public interface IClipContainer : IDocumentItemContainer
	{

		IEnumerable<IClip> Clips { get; }

		IClip GetClip(Guid id);

		void AddClip(IClip clip);

		void RemoveClip(Guid id);

	}

	#endregion

}
