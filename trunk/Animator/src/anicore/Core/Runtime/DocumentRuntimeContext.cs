using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Runtime
{

	#region DocumentRuntimeContext

	[Obsolete]
	internal sealed class DocumentRuntimeContext : IRuntimeContext
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Document _Document;
		private readonly ITransport _Transport;
		private readonly Dictionary<Guid, IOutputTransmitter> _Transmitters;

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
			get
			{
				return from state in this._Document.Clips
					   where state.IsPlaying
					   select state;
			}
		}

		#endregion

		#region Constructors

		internal DocumentRuntimeContext([NotNull] Document document, [NotNull] ITransport transport)
		{
			Require.ArgNotNull(document, "document");
			Require.ArgNotNull(transport, "transport");
			this._Document = document;
			this._Transport = transport;
			this._Transmitters = new Dictionary<Guid, IOutputTransmitter>();
		}

		#endregion

		#region Methods

		public Clip GetClip(Guid id)
		{
			return this._Document.GetClip(id);
		}

		public Track GetTrack(Guid id)
		{
			return this._Document.GetTrack(id);
		}

		public Output GetOutput(Guid id)
		{
			return this._Document.GetOutput(id);
		}

		private IOutputTransmitter GetTransmitter(Guid id, bool create)
		{
			IOutputTransmitter transmitter;
			if(this._Transmitters.TryGetValue(id, out transmitter))
				return transmitter;
			if(create)
			{
				var output = this.GetOutput(id);
				if(output != null)
				{
					transmitter = OutputTransmitter.CreateTransmitter(output);
					this._Transmitters.Add(id, transmitter);
					return transmitter;
				}
			}
			return null;
		}

		public IOutputTransmitter GetTransmitter(Guid id)
		{
			return this.GetTransmitter(id, true);
		}

		internal void PostClipOutput(Clip clip)
		{
			if(clip == null || !clip.IsPlaying || clip.OutputId == null)
				return;
			var transmitter = this.GetTransmitter(clip.OutputId.Value);
			if(transmitter == null)
				return;
			var message = clip.BuildOutputMessage(this._Transport);
			transmitter.PostMessage(message);
		}

		public void PostActiveClipOutputs()
		{
			foreach(var clip in this.ActiveClips.ToArray())
				this.PostClipOutput(clip);
		}

		#endregion

	}

	#endregion

}
