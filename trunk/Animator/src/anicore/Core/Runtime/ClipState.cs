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

	#region ClipState

	[Obsolete]
	public sealed class ClipState
	{

		#region Static / Constant

		#endregion

		#region Fields

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
		}

		#endregion

		#region Events

		internal event EventHandler Started;

		internal event EventHandler Stopped;

		#endregion

		#region Constructors

		internal ClipState([NotNull] Clip clip)
		{
			Require.ArgNotNull(clip, "clip");
			this._Clip = clip;
		}

		#endregion

		#region Methods

		public void Start(ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			this._IsPlaying = true;
			this._StartTime = transport.Position;
			var handler = this.Started;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public void Stop()
		{
			this._IsPlaying = false;
			var handler = this.Stopped;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		public Time GetPosition(ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			if(!this._IsPlaying)
				return 0;
			return (transport.Position - this._StartTime) % this._Clip.Duration;
		}

		public object GetValue(ITransport transport)
		{
			if(!this._IsPlaying)
				return null;
			return this._Clip.GetValue(this.GetPosition(transport));
		}

		internal OutputMessage BuildOutputMessage(ITransport transport)
		{
			return new OutputMessage(this._Clip.TargetKey, this.GetValue(transport));
		}

		#endregion

	}

	#endregion

}
