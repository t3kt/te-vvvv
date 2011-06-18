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

		#region Static / Constant

		private static IOrderedEnumerable<SequenceClip> OrderByStart([NotNull] IEnumerable<SequenceClip> clips)
		{
			Require.DBG_ArgNotNull(clips, "clips");
			return clips.OrderBy(c => c.Start);
		}

		#endregion

		#region Fields

		private readonly HashSet<SequenceClip> _Clips;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public SequenceClipCollection()
		{
			this._Clips = new HashSet<SequenceClip>(GuidIdComparer.Default);
		}

		#endregion

		#region Methods

		private void Clip_HandleTryChangeSpan(object sender, TryChangeValueEventArgs<Interval> e)
		{
			var clip = (SequenceClip)sender;
			var overlaps =
				OrderByStart(
					this._Clips
						.Where(c => !ReferenceEquals(c, clip) && c.Overlaps(clip)))
					.ToArray();
			if(overlaps.Length > 0)
			{
				Debug.Assert(overlaps.Length <= 2);
				SequenceClip before, after;
				if(overlaps.Length == 2)
				{
					before = overlaps[0];
					after = overlaps[1];
				}
				else if(overlaps[0].Start < clip.Start)
				{
					before = overlaps[0];
					after = null;
				}
				else
				{
					before = null;
					after = overlaps[1];
				}
				throw new NotImplementedException();
			}
		}

		private void Attach(SequenceClip clip, bool applyNow)
		{
			Require.DBG_ArgNotNull(clip, "clip");
			clip.SetIntervalChangeHandler(this.Clip_HandleTryChangeSpan, applyNow);
		}

		private void Detach(SequenceClip clip)
		{
			Require.DBG_ArgNotNull(clip, "clip");
			clip.SetIntervalChangeHandler(null);
		}

		public void AddBatch([NotNull] IEnumerable<SequenceClip> items)
		{
			Require.ArgNotNull(items, "items");
			foreach(var item in OrderByStart(items))
				this.Add(item, true);
		}

		internal void Add([NotNull]SequenceClip item, bool applyNow)
		{
			Require.ArgNotNull(item, "item");
			if(!this._Clips.Add(item))
				throw new ArgumentException(String.Format(CoreStrings.DuplicateClipId, item.Id), "item");
			this.Attach(item, applyNow);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
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
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public bool Contains(SequenceClip item)
		{
			return item != null && this._Clips.Contains(item);
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
			if(item == null || !this._Clips.Remove(item))
				return false;
			this.Detach(item);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
			return true;
		}

		#endregion

		#region IEnumerable<SequenceClip> Members

		public IEnumerator<SequenceClip> GetEnumerator()
		{
			return OrderByStart(this).GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region INotifyCollectionChanged Members

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			var handler = this.CollectionChanged;
			if(handler != null)
				handler(this, e);
		}

		#endregion

	}

	#endregion

}
