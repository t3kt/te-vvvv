using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Runtime
{

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

		internal Clip Clip
		{
			get { return this._Clip; }
		}

		public bool IsPlaying
		{
			get { return this._IsPlaying && this._Transport.IsPlaying; }
		}

		public Time? Position
		{
			get
			{
				if(!this.IsPlaying)
					return null;
				return (this._Transport.Position - this._StartTime) % this._Clip.Duration;
			}
		}

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
		}

		public void Stop()
		{
			this._IsPlaying = false;
		}

		public object GetValue()
		{
			var position = this.Position;
			return position == null ? null : this._Clip.GetValue(position.Value);
		}

		#endregion

	}

	#endregion

}
