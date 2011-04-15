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

	#region DocumentRuntimeContext

	internal sealed class DocumentRuntimeContext : IRuntimeContext
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Document _Document;
		private readonly ITransport _Transport;

		#endregion

		#region Properties

		public Document Document
		{
			get { return this._Document; }
		}

		public ITransport Transport
		{
			get { return this._Transport; }
		}

		public IEnumerable<Clip> ActiveClips
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region Constructors

		internal DocumentRuntimeContext([NotNull] Document document, [NotNull] ITransport transport)
		{
			Require.ArgNotNull(document, "document");
			Require.ArgNotNull(transport, "transport");
			this._Document = document;
			this._Transport = transport;
		}

		#endregion

		#region Methods

		public Clip GetClip(Guid id)
		{
			throw new NotImplementedException();
		}

		public Track GetTrack(Guid id)
		{
			throw new NotImplementedException();
		}

		public Output GetOutput(Guid id)
		{
			throw new NotImplementedException();
		}

		public IOutputTransmitter GetOutputTransmitter(Guid id)
		{
			throw new NotImplementedException();
		}

		public ClipState GetClipState(Guid id)
		{
			throw new NotImplementedException();
		}

		public ClipState GetTrackActiveClipState(Guid id)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
