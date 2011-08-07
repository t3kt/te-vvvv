using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Composition;
using TEShared;

namespace Animator.Core.Transport
{

	#region TransportBase

	public abstract class TransportBase : ITransportController
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		[Category(Names.Category_Common)]
		public abstract TimeSpan Position { get; set; }

		[Category(Names.Category_Common)]
		public abstract TransportState State { get; protected set; }

		#endregion

		#region Events

		public event EventHandler<TransportTickEventArgs> Tick;

		public event EventHandler StateChanged;

		public event EventHandler PositionChanged;

		[Browsable(false)]
		protected EventHandler<TransportTickEventArgs> TickHandler
		{
			get { return this.Tick; }
		}

		[Browsable(false)]
		protected EventHandler StateChangedHandler
		{
			get { return this.StateChanged; }
		}

		[Browsable(false)]
		protected EventHandler PositionChangedHandler
		{
			get { return this.PositionChanged; }
		}

		protected virtual void OnTick(TimeSpan interval)
		{
			var handler = this.Tick;
			if(handler != null)
				handler(this, new TransportTickEventArgs(interval));
		}

		protected virtual void OnStateChanged()
		{
			var handler = this.StateChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		protected virtual void OnPositionChanged()
		{
			var handler = this.PositionChanged;
			if(handler != null)
				handler(this, EventArgs.Empty);
		}

		#endregion

		#region Constructors

		protected TransportBase()
		{

		}

		~TransportBase()
		{
			this.Dispose(false);
		}

		#endregion

		#region Methods

		public virtual void Play()
		{
			this.State = TransportState.Playing;
		}

		public virtual void Stop()
		{
			this.State = TransportState.Stopped;
			this.Position = TimeSpan.Zero;
		}

		public virtual void Pause()
		{
			//this.State = TransportState.Paused;
			this.State = TransportState.Stopped;
		}

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

	#region NullTransport

	[AniExport(typeof(ITransportController), Key = Export_Key, Description = Export_Description)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	internal sealed class NullTransport : ITransportController
	{

		#region Static / Constant

		internal const string Export_Key = "null";
		internal const string Export_Description = "No Transport";

		#endregion

		#region ITransportController Members

		public TimeSpan Position
		{
			get { return TimeSpan.Zero; }
			set { }
		}

		public TransportState State
		{
			get { return TransportState.Stopped; }
		}

		public void Play()
		{
		}

		public void Stop()
		{
		}

		public void Pause()
		{
		}

		public event EventHandler<TransportTickEventArgs> Tick;

		public event EventHandler StateChanged;

		public event EventHandler PositionChanged;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}

	#endregion

}
