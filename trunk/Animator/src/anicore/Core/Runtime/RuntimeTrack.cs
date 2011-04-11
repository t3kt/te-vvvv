using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Transport;
using TESharedAnnotations;

namespace Animator.Core.Runtime
{

	#region RuntimeTrack

	internal sealed class RuntimeTrack : RuntimeDocumentItem
	{

		#region Static / Constant

		internal static RuntimeTrack CreateTrack(RuntimeDocument runtimeDocument, Track track)
		{
			return new RuntimeTrack(runtimeDocument, track);
		}

		#endregion

		#region Fields

		private readonly Track _Track;
		private RuntimeOutput _Output;
		private RuntimeClip _ActiveClip;

		#endregion

		#region Properties

		public override DocumentItem Item
		{
			get { return this._Track; }
		}

		public Track Track
		{
			get { return this._Track; }
		}

		[CanBeNull]
		public RuntimeOutput Output
		{
			get { return this._Output; }
		}

		[CanBeNull]
		public IOutputTransmitter OutputTransmitter
		{
			get { return this._Output == null ? null : this._Output.Transmitter; }
		}

		public Guid? ActiveClipId
		{
			get { return this._ActiveClip == null ? (Guid?)null : this._ActiveClip.Item.Id; }
			set
			{
				if(value != this.ActiveClipId)
				{
					this._ActiveClip = value == null ? null : this.RuntimeDocument.GetClip(value.Value);
					OnPropertyChanged("ActiveClipId");
				}
			}
		}

		[CanBeNull]
		internal RuntimeClip ActiveClip
		{
			get { return this._ActiveClip; }
		}

		#endregion

		#region Constructors

		public RuntimeTrack(RuntimeDocument runtimeDocument, Track track)
			: base(runtimeDocument)
		{
			Require.ArgNotNull(track, "track");
			this._Track = track;
			this.AttachItem(track);
			this.InitializeOutput();
		}

		#endregion

		#region Methods

		private void InitializeOutput()
		{
			this._Output = this._Track.OutputId == null ? null : this.RuntimeDocument.GetOutput(this._Track.OutputId.Value);
		}

		protected override void OnPropertyChanged(string name)
		{
			if(name == "Item.OutputId")
				this.InitializeOutput();
			base.OnPropertyChanged(name);
		}

		internal bool TryGetValue(ITransport transport, out object value)
		{
			if(this._ActiveClip == null)
			{
				value = null;
				return false;
			}
			value = this._ActiveClip.GetValue(transport);
			return true;
		}

		private string GetTargetKey()
		{
			var key = this._Track.TargetKey;
			return this._ActiveClip == null ? key : key + this._ActiveClip.Clip.TargetKey;
		}

		[CanBeNull]
		internal OutputMessage BuildOutputMessage(ITransport transport)
		{
			object value;
			if(!this.TryGetValue(transport, out value))
				return null;
			var key = GetTargetKey();
			return new OutputMessage(key, value);
		}

		internal void PostOutput(ITransport transport)
		{
			var transmitter = this.OutputTransmitter;
			if(transmitter == null)
				return;
			var message = this.BuildOutputMessage(transport);
			if(message != null)
				transmitter.PostMessage(message);
		}

		#endregion

	}

	#endregion

}
