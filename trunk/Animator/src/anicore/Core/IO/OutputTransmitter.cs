using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Model;

namespace Animator.Core.IO
{

	#region OutputTransmitter

	public abstract class OutputTransmitter : IOutputTransmitter
	{

		#region NullTransmitter

		[OutputTransmitter(Key = "null", Description = "No Transmitter")]
		internal sealed class NullTransmitter : OutputTransmitter
		{

			#region Static/Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			protected override bool PostMessageInternal(OutputMessage message)
			{
				return false;
			}

			#endregion

		}

		#endregion

		#region TraceTransmitter

		[OutputTransmitter(Key = "trace", Description = "Debug Trace Transmitter")]
		internal sealed class TraceTransmitter : OutputTransmitter
		{

			#region Static / Constant

			private const string DefaultCategory = "OUTPUT";
			private const string DefaultPrefix = "[msg] ";

			#endregion

			#region Fields

			private string _Category = DefaultCategory;
			private string _Prefix = DefaultPrefix;

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			public override void Initialize(Output outputModel)
			{
				Require.ArgNotNull(outputModel, "outputModel");
				this._Category = outputModel.GetParameter("TraceCategory") ?? DefaultCategory;
				this._Prefix = outputModel.GetParameter("TracePrefix") ?? DefaultPrefix;
			}

			protected override bool PostMessageInternal(OutputMessage message)
			{
				var str = OutputMessage.FormatTrace(message);
				if(!String.IsNullOrWhiteSpace(this._Prefix))
					str = this._Prefix + " " + str;
				Trace.WriteLine(str, this._Category);
				return true;
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		~OutputTransmitter()
		{
			Dispose(false);
		}

		#endregion

		#region Methods

		protected abstract bool PostMessageInternal(OutputMessage message);

		protected virtual void OnMessageDropped(OutputMessage message)
		{
			if(message == null)
				return;
			var handler = this.MessageDropped;
			if(handler != null)
				handler(this, new OutputMessageEventArgs(message));
		}

		#endregion

		#region IOutputTransmitter Members

		public virtual void Initialize(Output outputModel)
		{
			Require.ArgNotNull(outputModel, "outputModel");
		}

		public void PostMessage(OutputMessage message)
		{
			if(message != null)
			{
				if(!this.PostMessageInternal(message))
					this.OnMessageDropped(message);
			}
		}

		public event EventHandler<OutputMessageEventArgs> MessageDropped;

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
