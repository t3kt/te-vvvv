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

	internal class RuntimeClip : RuntimeDocumentItem, IPlayable
	{

		#region Static / Constant

		internal static RuntimeClip CreateClip(RuntimeDocument runtimeDocument, Clip clip)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Fields

		private readonly Clip _Clip;

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

		#endregion

		#region IPlayable Members

		public Time Duration
		{
			get { return this._Clip.Duration; }
		}

		public int TriggerAlignment
		{
			get { return this._Clip.TriggerAlignment; }
		}

		public void Start(ITransport transport)
		{
			throw new NotImplementedException();
		}

		public void Stop(ITransport transport)
		{
			throw new NotImplementedException();
		}

		public void EnqueueStart(ITransport transport)
		{
			throw new NotImplementedException();
		}

		public void EnqueueStop(ITransport transport)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
