using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Runtime;
using Animator.Resources;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceClipCollection

	public sealed class SequenceClipCollection : ICollection<SequenceClip>, INotifyCollectionChanged
	{

		#region SeqClipOrderComparer

		private sealed class SeqClipOrderComparer : Comparer<SequenceClip>
		{

			#region Static/Constant

			public new static readonly SeqClipOrderComparer Default = new SeqClipOrderComparer();

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			private SeqClipOrderComparer() { }

			#endregion

			#region Methods

			public override int Compare(SequenceClip x, SequenceClip y)
			{
				if(x == y)
					return 0;
				if(x == null)
					return -1;
				if(y == null)
					return 1;
				return x.Start.CompareTo(y.Start);
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		private static IOrderedEnumerable<SequenceClip> OrderByStart([NotNull] IEnumerable<SequenceClip> clips)
		{
			Require.DBG_ArgNotNull(clips, "clips");
			return clips.OrderBy(c => c.Start);
		}

		#endregion

		#region Fields

		private readonly SequenceTrack _Track;
		private readonly SortedList<TimeSpan, SequenceClip> _Clips;
		private readonly EventHandler<TryChangeValueEventArgs<Interval>> _HandleClipTryChangeSpan;
		private readonly EventHandler<ValueChangedEventArgs<Interval>> _HandleClipIntervalChanged; 

		#endregion

		#region Properties

		public int Count
		{
			get { return this._Clips.Count; }
		}

		#endregion

		#region Events

		//internal event EventHandler<TryChangeValueEventArgs<Interval>> ClipChangingInterval;

		//private void OnClipChangingInterval(object sender, TryChangeValueEventArgs<Interval> e)
		//{
		//    var handler = this.ClipChangingInterval;
		//    if(handler != null)
		//        handler(sender, e);
		//}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			var handler = this.CollectionChanged;
			if(handler != null)
				handler(this, e);
		}

		#endregion

		#region Constructors

		internal SequenceClipCollection([NotNull] SequenceTrack track)
		{
			Require.ArgNotNull(track, "track");
			this._Track = track;
			this._Clips = new SortedList<TimeSpan, SequenceClip>();
			this._HandleClipTryChangeSpan = this.Clip_HandleTryChangeSpan;
			this._HandleClipIntervalChanged += this.Clip_HandleIntervalChanged;
		}

		#endregion

		#region Methods

		internal bool CheckOverlap(SequenceClip clip)
		{
			return clip != null && this.CheckIntervalOverlap(clip.Interval, clip);
		}

		internal bool CheckIntervalOverlap(Interval interval, SequenceClip except = null)
		{
			foreach(var c in this._Clips.Values)
			{
				if(except == null || !ReferenceEquals(c, except))
				{
					if(c.Interval.Overlaps(interval))
						return true;
				}
			}
			return false;
		}

		private void Clip_HandleIntervalChanged(object sender, ValueChangedEventArgs<Interval> e)
		{
			var clip = (SequenceClip) sender;
			var index = this._Clips.IndexOfKey(clip.Start);
			if(index == -1)
				return;
			//Debug.Assert(ReferenceEquals(clip, this._Clips));
			throw new NotImplementedException();
		}

		private void Clip_HandleTryChangeSpan(object sender, TryChangeValueEventArgs<Interval> e)
		{
			var clip = (SequenceClip)sender;
			//var overlaps =
			//    OrderByStart(
			//        this._Clips
			//            .Where(c => !ReferenceEquals(c, clip) && c.Overlaps(clip)));
			//.ToArray();
			//if(overlaps.Any())
			if(this.CheckIntervalOverlap(e.NewValue, clip))
			{
				e.Deny();
			}
			//this.OnClipChangingInterval(sender, e.CloneWithoutDecision());
			//if(overlaps.Length > 0)
			//{
			//    e.Deny();
			//    return;
			//    //Debug.Assert(overlaps.Length <= 2);
			//    //SequenceClip before, after;
			//    //if(overlaps.Length == 2)
			//    //{
			//    //    before = overlaps[0];
			//    //    after = overlaps[1];
			//    //}
			//    //else if(overlaps[0].Start < clip.Start)
			//    //{
			//    //    before = overlaps[0];
			//    //    after = null;
			//    //}
			//    //else
			//    //{
			//    //    before = null;
			//    //    after = overlaps[1];
			//    //}
			//    //throw new NotImplementedException();
			//}
		}

		private void Attach(SequenceClip clip, bool applyNow)
		{
			Require.DBG_ArgNotNull(clip, "clip");
			clip.Parent = this._Track;
			clip.SetIntervalChangeHandler(this._HandleClipTryChangeSpan, applyNow);
			clip.IntervalChanged += this._HandleClipIntervalChanged;
		}

		private void Detach(SequenceClip clip)
		{
			Require.DBG_ArgNotNull(clip, "clip");
			clip.Parent = null;
			clip.SetIntervalChangeHandler(null);
			clip.IntervalChanged -= this._HandleClipIntervalChanged;
		}

		public int AddBatch([NotNull] IEnumerable<SequenceClip> items)
		{
			Require.ArgNotNull(items, "items");
			var added = 0;
			foreach(var item in OrderByStart(items.NonNull()))
			{
				if(this.Add(item, true))
					added++;
			}
			return added;
		}

		internal bool Add([NotNull]SequenceClip clip, bool applyNow)
		{
			Require.ArgNotNull(clip, "clip");
			if(this.CheckOverlap(clip))
				return false;
			this._Clips.Add(clip.Start, clip);
			//if(!this._Clips.Add(clip))
			//    throw new ArgumentException(String.Format(CoreStrings.DuplicateClipId, clip.Id), "clip");
			this.Attach(clip, applyNow);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, clip));
			return true;
		}

		public bool Add([NotNull] SequenceClip clip)
		{
			return this.Add(clip, true);
		}

		public void Clear()
		{
			foreach(var clip in this._Clips.Values)
				this.Detach(clip);
			this._Clips.Clear();
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public bool Contains(SequenceClip clip)
		{
			if(clip == null) return false;
			return this._Clips.ContainsKey(clip.Start) || this._Clips.ContainsValue(clip);
		}

		public bool Remove(SequenceClip clip)
		{
			if(clip == null || !this._Clips.Remove(clip.Start))
				return false;
			this.Detach(clip);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, clip));
			return true;
		}

		#endregion

		#region ICollection<SequenceClip> Members

		void ICollection<SequenceClip>.Add([NotNull] SequenceClip clip)
		{
			this.Add(clip);
		}

		void ICollection<SequenceClip>.CopyTo(SequenceClip[] array, int arrayIndex)
		{
			this._Clips.Values.CopyTo(array, arrayIndex);
		}

		bool ICollection<SequenceClip>.IsReadOnly
		{
			get { return false; }
		}

		#endregion

		#region IEnumerable<SequenceClip> Members

		public IEnumerator<SequenceClip> GetEnumerator()
		{
			//return OrderByStart(this).GetEnumerator();
			return this._Clips.Values.GetEnumerator();
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
