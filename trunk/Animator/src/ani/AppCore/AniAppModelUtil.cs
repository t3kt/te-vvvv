using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common;
using Animator.Core.Model;

namespace Animator.AppCore
{

	#region AniAppModelUtil

	internal static class AniAppModelUtil
	{

		internal static int CalculateRows(Document document)
		{
			if(document == null)
				return 0;
			var requestedRows = document.UIRows ?? 0;
			var requiredRows = document.Clips.MaxOrZero(c => c.UIRow);
			return Math.Max(0, Math.Max(requestedRows, requiredRows));
		}

		internal static int CalculateColumns(Document document)
		{
			if(document == null)
				return 0;
			var requestedCols = document.UIColumns ?? 0;
			var requiredCols = document.Clips.MaxOrZero(c => c.UIColumn);
			return Math.Max(0, Math.Max(requestedCols, requiredCols));
		}

	}

	#endregion

}
