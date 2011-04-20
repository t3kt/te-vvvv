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

		internal static int GetDocumentMinRows(this Document document)
		{
			if(document == null)
				return 0;
			if(document.Clips.Count == 0)
				return 0;
			var last = -1;
			foreach(var clip in document.Clips)
			{
				if(clip != null && clip.UIRow != null && clip.UIRow > last)
					last = clip.UIRow.Value;
			}
			return last + 1;
		}

		internal static int GetDocumentMinColumns(this Document document)
		{
			if(document == null)
				return 0;
			if(document.Clips.Count == 0)
				return 0;
			var last = -1;
			foreach(var clip in document.Clips)
			{
				if(clip != null && clip.UIColumn != null && clip.UIColumn > last)
					last = clip.UIColumn.Value;
			}
			return last + 1;
		}

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
