using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Core.Model
{

	#region IOutputContainer

	public interface IOutputContainer : IDocumentItemContainer
	{

		IEnumerable<IOutput> Outputs { get; }

		IOutput GetOutput(Guid id);

		void AddOutput(IOutput output);

		void RemoveOutput(Guid id);

	}

	#endregion

}
