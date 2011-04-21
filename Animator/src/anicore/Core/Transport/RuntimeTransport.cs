using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sanford.Multimedia.Timers;

namespace Animator.Core.Transport
{

	#region RuntimeTransport

	internal sealed class RuntimeTransport : ITransport, IDisposable
	{

		#region Nested Types

		#endregion

		#region Static / Constant

		#endregion

		#region Fields

		private readonly object _StateLock = new object();
		private float _BeatsPerMinute = Model.Document.DefaultBeatsPerMinute;
		private uint _StartTime;
		private Time _Position;
		private readonly Timer _Timer;

		#endregion

		#region Properties

		public float BeatsPerMinute
		{
			get { return this._BeatsPerMinute; }
			internal set { this._BeatsPerMinute = value; }
		}

		public bool IsPlaying
		{
			get { return this._Timer.IsRunning; }
		}

		public Time Position
		{
			get
			{
				lock(this._StateLock)
					return this._Position;
			}
		}

		#endregion

		#region Constructors

		public RuntimeTransport()
		{
			this._Timer = new Timer { Mode = TimerMode.Periodic };
			this._Timer.Tick += this.Timer_Tick;
		}

		#endregion

		#region Methods

		private void Timer_Tick(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		public void Play()
		{
			lock(_StateLock)
				PlayNoLock();
		}

		public void Stop()
		{
			lock(_StateLock)
				StopNoLock();
		}

		internal void Reset()
		{
			lock(_StateLock)
				ResetNoLock();
		}

		public void Pause()
		{
			lock(_StateLock)
				PauseNoLock();
		}

		private void PlayNoLock()
		{
			if(this.IsPlaying)
				return;
			this._StartTime = NativeMethods.timeGetTime();
			throw new NotImplementedException();
		}

		private void PauseNoLock()
		{
			if(!this.IsPlaying)
				return;
			throw new NotImplementedException();
		}

		private void StopNoLock()
		{
			PauseNoLock();
			ResetNoLock();
		}

		private void ResetNoLock()
		{
			if(this.IsPlaying)
				throw new InvalidOperationException();
			this._Position = 0;
			throw new NotImplementedException();
		}

		internal void Update()
		{
			lock(this._StateLock)
			{
				this.UpdateNoLock();
			}
		}

		private void UpdateNoLock()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Stop();
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
