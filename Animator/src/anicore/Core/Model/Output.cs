using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Animator.Core.Model
{

	#region Output

	public abstract class Output: DocumentItem, IOutput
	{

		#region Static / Constant

		internal static IOutput ReadOutputXElement(IDocument document, IDocumentItem parent, XElement element)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
