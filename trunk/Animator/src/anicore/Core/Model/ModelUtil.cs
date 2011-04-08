using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animator.Core.Model
{

	#region ModelUtil

	internal static class ModelUtil
	{

		public static IDocument GetDocument(IDocumentItem item)
		{
			while(item != null)
			{
				if(item is IDocument)
					return (IDocument)item;
				item = item.Parent;
			}
			return null;
		}

	}

	#endregion

}
