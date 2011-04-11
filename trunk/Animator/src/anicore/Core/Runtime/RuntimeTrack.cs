using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Common.Diagnostics;
using Animator.Core.Model;

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

		#endregion

		#region Constructors

		public RuntimeTrack(RuntimeDocument runtimeDocument, Track track)
			: base(runtimeDocument)
		{
			Require.ArgNotNull(track, "track");
			this._Track = track;
			this.AttachItem(track);
		}

		#endregion

		#region Methods

		private void InitializeOutput()
		{
			this._Output = this._Track.OutputId == null ? null : this.RuntimeDocument.GetOutput(this._Track.OutputId.Value);
			throw new NotImplementedException();
		}

		protected override void OnItemPropertyChanged(string name)
		{
			if(name == "OutputId")
				this.InitializeOutput();
			base.OnItemPropertyChanged(name);
		}

		#endregion

	}

	#endregion

}
