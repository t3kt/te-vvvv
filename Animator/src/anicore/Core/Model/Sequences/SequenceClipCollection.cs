using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Animator.Core.Model.Sequences
{

	#region SequenceClipCollection

	internal sealed class SequenceClipCollection : ICollection<SequenceClip>
	{

		#region PositionComparer

		private sealed class PositionComparer : IComparer<SequenceClip>, IEqualityComparer<SequenceClip>
		{

			#region Static/Constant

			public static readonly PositionComparer Instance = new PositionComparer();

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			private PositionComparer() { }

			#endregion

			#region Methods

			public int Compare(SequenceClip x, SequenceClip y)
			{
				if(x == null)
					return y == null ? 0 : -1;
				if(y == null)
					return 1;
				return x.Start.CompareTo(y.Start);
			}

			#endregion

			#region IEqualityComparer<SequenceClip> Members

			public bool Equals(SequenceClip x, SequenceClip y)
			{
				if(x == null)
					return y == null;
				if(y == null)
					return false;
				return x.Start == y.Start;
			}

			public int GetHashCode(SequenceClip obj)
			{
				return obj == null ? 0 : obj.Start.GetHashCode();
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

		#region ICollection<SequenceClip> Members

		public void Add(SequenceClip item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(SequenceClip item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(SequenceClip[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		bool ICollection<SequenceClip>.IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(SequenceClip item)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<SequenceClip> Members

		public IEnumerator<SequenceClip> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

	}

	#endregion

}
