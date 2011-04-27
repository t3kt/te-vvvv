using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.IO;

namespace Animator.Tests.Utils
{

	#region OutputMessageComparer

	internal class OutputMessageComparer : EqualityComparer<OutputMessage>, IComparer<OutputMessage>, IComparer
	{

		public static readonly OutputMessageComparer Instance = new OutputMessageComparer();

		public override bool Equals(OutputMessage x, OutputMessage y)
		{
			if(x == null)
				return y == null;
			if(y == null)
				return true;
			if(x.TargetKey != y.TargetKey)
				return false;
			if(x.Data == y.Data)
				return true;
			if(x.Data == null)
				return y.Data == null;
			if(y.Data == null)
				return false;
			return x.Data.SequenceEqual(y.Data);
		}

		public override int GetHashCode(OutputMessage obj)
		{
			if(obj == null || obj.TargetKey == null)
				return 0;
			return obj.TargetKey.GetHashCode();
		}

		public int Compare(OutputMessage x, OutputMessage y)
		{
			if(x == null)
				return y == null ? 0 : -1;
			if(y == null)
				return 1;
			var cmp = Comparer<object>.Default;
			var result = cmp.Compare(x.TargetKey, y.TargetKey);
			if(result != 0)
				return result;
			if(x.Data == y.Data)
				return 0;
			if(x.Data == null)
				return y.Data == null ? 0 : -1;
			if(y.Data == null)
				return 1;
			result = x.Data.Length.CompareTo(y.Data.Length);
			if(result != 0)
				return result;
			for(var i = 0; i < x.Data.Length; i++)
			{
				result = cmp.Compare(x.Data[i], y.Data[i]);
				if(result != 0)
					return result;
			}
			return 0;
		}

		public int Compare(object x, object y)
		{
			return Compare(x as OutputMessage, y as OutputMessage);
		}

	}

	#endregion

}
