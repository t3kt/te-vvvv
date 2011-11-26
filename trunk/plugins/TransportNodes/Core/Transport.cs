using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEShared;
using TESharedAnnotations;

namespace TransportNodes.Core
{
	public sealed class Transport : IDisposable
	{

		#region Static/Constant

		internal static StringComparer KeyComparer
		{
			get { return StringComparer.OrdinalIgnoreCase; }
		}

		#endregion

		#region Reference Tracking

		private int _RefCount;

		internal int RefCount
		{
			get { return this._RefCount; }
		}

		internal void IncrementRefCount()
		{
			this._RefCount++;
		}

		internal bool DecrementRefCount()
		{
			this._RefCount--;
			return this._RefCount <= 0;
		}

		#endregion

		#region Fields

		private readonly Guid _Id;
		private readonly string _Key;
		private TransportState _State;
		private double _Position;

		private double _StartedTime;
		private double _LastUpdatedTime;

		#endregion

		#region Properties

		public Guid Id
		{
			get { return this._Id; }
		}

		public string Key
		{
			get { return this._Key; }
		}

		public TransportState State
		{
			get { return _State; }
			set
			{
				if(value != _State)
				{
					switch(value)
					{
					case TransportState.Stopped:
						this.Stop();
						break;
					case TransportState.Playing:
						this.Play();
						break;
					case TransportState.Paused:
						this.Pause();
						break;
					}
				}
			}
		}

		public bool IsPlaying
		{
			get { return _State == TransportState.Playing; }
		}

		public double Position
		{
			get { return _Position; }
			set { _Position = value; }
		}

		#endregion

		#region Events

		public event EventHandler StateChanged;

		private void OnStateChanged()
		{
			var handler = StateChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Constructors

		internal Transport([NotNull]string key)
		{
			Require.ArgNotNull(key, "key");
			this._Id = Guid.NewGuid();
			this._Key = key;
		}

		#endregion

		#region Methods

		private void SetStateInternal(TransportState value)
		{
			if(value != _State)
			{
				_State = value;
				this.OnStateChanged();
			}
		}

		private void PlayInternal()
		{
			this.SetStateInternal(TransportState.Playing);
		}

		private void PauseInternal()
		{
			this.SetStateInternal(TransportState.Paused);
		}

		private void UnpauseInternal()
		{
			this.SetStateInternal(TransportState.Playing);
		}

		private void StopInternal()
		{
			this.SetStateInternal(TransportState.Stopped);
			this._Position = 0;
		}

		public void Play()
		{
			switch(this._State)
			{
			case TransportState.Paused:
				this.UnpauseInternal();
				break;
			case TransportState.Stopped:
				this.PlayInternal();
				break;
			}
		}

		public void Pause()
		{
			switch(this._State)
			{
			case TransportState.Playing:
				this.PauseInternal();
				break;
			case TransportState.Paused:
				this.UnpauseInternal();
				break;
			}
		}

		public void Stop()
		{
			switch(this._State)
			{
			case TransportState.Playing:
			case TransportState.Paused:
				this.StopInternal();
				break;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
