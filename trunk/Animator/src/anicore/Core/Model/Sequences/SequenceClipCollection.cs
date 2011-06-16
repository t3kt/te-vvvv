using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Runtime;
using Animator.Resources;
using TESharedAnnotations;

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

		private readonly List<SequenceClip> _Clips;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public SequenceClipCollection()
		{
			this._Clips = new List<SequenceClip>();
		}

		#endregion

		#region Methods

		private void Clip_HandleTryChangeSpan(object sender, TryChangeValueEventArgs<Tuple<TimeSpan, TimeSpan>> e)
		{
			var clip = (SequenceClip)sender;
			foreach(var other in this._Clips)
			{
				if(!ReferenceEquals(clip, other))
				{

				}
			}
			throw new NotImplementedException();
		}

		private void Attach(SequenceClip clip, bool applyNow)
		{
			Require.DBG_ArgNotNull(clip, "clip");
			clip.SetSpanChangeHandler(this.Clip_HandleTryChangeSpan, applyNow);
		}

		private void Detach(SequenceClip clip)
		{
			Require.DBG_ArgNotNull(clip, "clip");
			clip.SetSpanChangeHandler(null);
		}

		public void AddBatch([NotNull] IEnumerable<SequenceClip> items)
		{
			Require.ArgNotNull(items, "items");
			foreach(var item in items.OrderBy(x => x.Start))
				this.Add(item, true);
		}

		internal void Add([NotNull]SequenceClip item, bool applyNow)
		{
			Require.ArgNotNull(item, "item");
			if(this.Contains(item))
				throw new ArgumentException(String.Format(CoreStrings.DuplicateClipId, item.Id), "item");
			this.Attach(item, applyNow);
			this._Clips.Add(item);
		}

		#endregion

		#region ICollection<SequenceClip> Members

		public void Add([NotNull] SequenceClip item)
		{
			this.Add(item, true);
		}

		public void Clear()
		{
			foreach(var clip in this._Clips)
				this.Detach(clip);
			this._Clips.Clear();
		}

		public bool Contains(SequenceClip item)
		{
			return item != null && this._Clips.Contains(item, GuidIdComparer.Default);
		}

		public void CopyTo(SequenceClip[] array, int arrayIndex)
		{
			this.ToArray().CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return this._Clips.Count; }
		}

		bool ICollection<SequenceClip>.IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(SequenceClip item)
		{
			//return this._Clips.Remove(item);
			if(item == null || !this.Contains(item))
				return false;
			//var i = this._Clips.FindIndex(x => GuidIdComparer.Default.Equals(x, item));
			//if(i < 0)
			//    return false;
			this.Detach(item);
			//this._Clips.RemoveAt(i);
			this._Clips.Remove(item);
			return true;
		}

		#endregion

		#region IEnumerable<SequenceClip> Members

		public IEnumerator<SequenceClip> GetEnumerator()
		{
			return this._Clips.OrderBy(x => x.Start).GetEnumerator();
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
