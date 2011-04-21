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
		private readonly Dictionary<Guid, ClipState> _ClipStates;
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

		public IEnumerable<ClipState> ActiveClips
		{
			get
			{
				return from state in this._ClipStates.Values
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
			this._ClipStates = new Dictionary<Guid, ClipState>();
			this._Transmitters = new Dictionary<Guid, IOutputTransmitter>();
		}

		#endregion

		#region Methods

		public void EnsureAllClipStates()
		{
			foreach(var clip in this._Document.Clips)
				this.GetClipState(clip.Id, true);
		}

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

		private ClipState GetClipState(Guid id, bool create)
		{
			ClipState state;
			if(this._ClipStates.TryGetValue(id, out state))
				return state;
			if(create)
			{
				var clip = this.GetClip(id);
				if(clip != null)
				{
					state = new ClipState(this._Transport, clip);
					this._ClipStates.Add(id, state);
					return state;
				}
			}
			return null;
		}

		public ClipState GetClipState(Guid id)
		{
			return this.GetClipState(id, true);
		}

		public ClipState GetTrackActiveClipState(Guid id)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
