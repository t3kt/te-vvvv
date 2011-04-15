using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using Animator.Core.Transport;

namespace Animator.Core.Runtime
{

	#region RuntimeClip

	internal sealed class RuntimeClip : RuntimeDocumentItem, IPlayable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Clip _Clip;
		private bool _IsPlaying;
		private Time _StartTime;

		#endregion

		#region Properties

		public override DocumentItem Item
		{
			get { return this._Clip; }
		}

		public Clip Clip
		{
			get { return this._Clip; }
		}

		#endregion

		#region Constructors

		public RuntimeClip(RuntimeDocument runtimeDocument, Clip clip)
			: base(runtimeDocument)
		{
			Require.ArgNotNull(clip, "clip");
			this._Clip = clip;
			this.AttachItem(clip);
		}

		#endregion

		#region Methods

		public Time? GetPosition(ITransport transport)
		{
			if(!transport.IsPlaying || !this._IsPlaying)
				return null;
			var globalPos = transport.Position;
			return (globalPos - this._StartTime) % this._Clip.Duration;
		}

		private object GetValue(Time position)
		{
			return this._Clip.GetValue(position);
		}

		public object GetValue(ITransport transport)
		{
			Require.ArgNotNull(transport, "transport");
			var time = GetPosition(transport);
			if(time == null)
				return null;
			return GetValue(time.Value);
		}

		#endregion

		#region IPlayable Members

		public void Start(ITransport transport)
		{
			this._StartTime = transport.Position;
			this._IsPlaying = true;
		}

		public void Stop(ITransport transport)
		{
			this._IsPlaying = false;
		}

		#endregion

	}

	#endregion

}
