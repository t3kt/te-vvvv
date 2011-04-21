using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Runtime
{

	#region ClipStateEventArgs

	internal sealed class ClipStateEventArgs : EventArgs
	{

		#region Static/Constant

		#endregion

		#region Fields

		private readonly ClipState _ClipState;

		#endregion

		#region Properties

		public ClipState ClipState
		{
			get { return this._ClipState; }
		}

		#endregion

		#region Constructors

		public ClipStateEventArgs(ClipState clipState)
		{
			this._ClipState = clipState;
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region ClipState

	public sealed class ClipState
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ITransport _Transport;
		private readonly Clip _Clip;
		private bool _IsPlaying;
		private Time _StartTime;

		#endregion

		#region Properties

		public Clip Clip
		{
			get { return this._Clip; }
		}

		public bool IsPlaying
		{
			get { return this._IsPlaying; }
			set
			{
				if(value)
					this.Start();
				else
					this.Stop();
			}
		}

		public Time Position
		{
			get
			{
				return (this._Transport.Position - this._StartTime) % this._Clip.Duration;
			}
		}

		#endregion

		#region Events

		internal event EventHandler<ClipStateEventArgs> Started;

		internal event EventHandler<ClipStateEventArgs> Stopped;

		#endregion

		#region Constructors

		internal ClipState([NotNull] ITransport transport, [NotNull] Clip clip)
		{
			Require.ArgNotNull(transport, "transport");
			Require.ArgNotNull(clip, "clip");
			this._Clip = clip;
			this._Transport = transport;
		}

		#endregion

		#region Methods

		public void Start()
		{
			this._IsPlaying = true;
			this._StartTime = this._Transport.Position;
			var handler = this.Started;
			if(handler != null)
				handler(this, new ClipStateEventArgs(this));
		}

		public void Stop()
		{
			this._IsPlaying = false;
			var handler = this.Stopped;
			if(handler != null)
				handler(this, new ClipStateEventArgs(this));
		}

		public object GetValue()
		{
			if(!this._IsPlaying)
				return null;
			return this._Clip.GetValue(this.Position);
		}

		internal OutputMessage BuildOutputMessage()
		{
			return new OutputMessage(this._Clip.TargetKey, this.GetValue());
		}

		#endregion

	}

	#endregion

}
