using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Runtime;
using Animator.Resources;
using TESharedAnnotations;

namespace Animator.Core.Model.Sequences
{

	#region SequenceClipCollection

	public sealed class SequenceClipCollection : IList<SequenceClip>, ICollection,
		INotifyCollectionChanged, INotifyPropertyChanged
	{

		#region Static / Constant

		private static IOrderedEnumerable<SequenceClip> OrderByStart([NotNull] IEnumerable<SequenceClip> clips)
		{
			Require.DBG_ArgNotNull(clips, "clips");
			return clips.OrderBy(c => c.Start);
		}

		#endregion

		#region Fields

		private object _SyncRoot;
		private readonly SequenceTrack _Track;
		private readonly List<SequenceClip> _Clips;

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

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string name)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Constructors

		internal SequenceClipCollection([NotNull] SequenceTrack track)
			: this(track, 0) { }

		internal SequenceClipCollection([NotNull] SequenceTrack track, int capacity)
		{
			Require.ArgNotNull(track, "track");
			this._Track = track;
			this._Clips = new List<SequenceClip>(capacity);
		}

		#endregion

		#region Methods

		internal bool CheckOverlap(SequenceClip clip)
		{
			return clip != null && this.CheckIntervalOverlap(clip.Interval, clip);
		}

		internal bool CheckIntervalOverlap(Interval interval, SequenceClip except = null)
		{
			foreach(var c in this._Clips)
			{
				if(except == null || !ReferenceEquals(c, except))
				{
					if(c.Interval.Overlaps(interval))
						return true;
				}
			}
			return false;
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

		private void Attach(SequenceClip clip)
		{
			Require.DBG_ArgNotNull(clip, "clip");
			clip.Parent = this._Track;
			clip.SetIntervalChangeHandler(this.Clip_HandleTryChangeSpan);
		}

		private void Detach(SequenceClip clip)
		{
			Require.DBG_ArgNotNull(clip, "clip");
			clip.Parent = null;
			clip.SetIntervalChangeHandler(null);
		}

		public bool Add([NotNull]SequenceClip clip)
		{
			return this.SetOrAdd(-1, clip, false);
		}

		private bool SetOrAdd(int index, [NotNull]SequenceClip clip, bool overwrite)
		{
			Require.ArgNotNull(clip, "clip");
			if(this.CheckOverlap(clip))
				return false;
			if(this._Clips.Contains(clip))
				throw new ArgumentException(String.Format(CoreStrings.DuplicateClipId, clip.Id), "clip");
			this.Attach(clip);
			if(index == -1)
			{
				this._Clips.Add(clip);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, clip, this._Clips.Count - 1));
				this.OnPropertyChanged("Count");
				this.OnPropertyChanged("Item[]");
			}
			else if(overwrite)
			{
				var oldClip = this._Clips[index];
				this.Detach(oldClip);
				this._Clips[index] = clip;
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, oldClip, clip, index));
				this.OnPropertyChanged("Item[]");
			}
			else
			{
				this._Clips.Insert(index, clip);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, clip, index));
				this.OnPropertyChanged("Count");
				this.OnPropertyChanged("Item[]");
			}
			return true;
		}

		public void Clear()
		{
			foreach(var clip in this._Clips)
				this.Detach(clip);
			this._Clips.Clear();
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("Item[]");
		}

		public bool Contains(SequenceClip clip)
		{
			return clip != null && this._Clips.Contains(clip);
		}

		public bool Remove(SequenceClip clip)
		{
			if(clip == null)
				return false;
			var index = this._Clips.IndexOf(clip);
			if(index == -1)
				return false;
			this._Clips.RemoveAt(index);
			//if(!this._Clips.Remove(clip))
			//	return false;
			this.Detach(clip);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, clip, index));
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("Item[]");
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
			this._Clips.CopyTo(array, arrayIndex);
		}

		bool ICollection<SequenceClip>.IsReadOnly
		{
			get { return false; }
		}

		#endregion

		#region IEnumerable<SequenceClip> Members

		public IEnumerator<SequenceClip> GetEnumerator()
		{
			return this._Clips.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region IList<SequenceClip> Members

		int IList<SequenceClip>.IndexOf(SequenceClip clip)
		{
			if(clip == null)
				return -1;
			return this._Clips.IndexOf(clip);
		}

		void IList<SequenceClip>.Insert(int index, SequenceClip clip)
		{
			this.SetOrAdd(index, clip, false);
		}

		void IList<SequenceClip>.RemoveAt(int index)
		{
			var clip = this[index];
			this.Remove(clip);
		}

		public SequenceClip this[int index]
		{
			get { return this._Clips[index]; }
		}

		SequenceClip IList<SequenceClip>.this[int index]
		{
			get { return this[index]; }
			set { this.SetOrAdd(index, value, true); }
		}

		#endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this._Clips).CopyTo(array, index);
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get
			{
				if(this._SyncRoot == null)
					Interlocked.CompareExchange<object>(ref this._SyncRoot, new object(), null);
				return this._SyncRoot;
			}
		}

		#endregion

	}

	#endregion

}
