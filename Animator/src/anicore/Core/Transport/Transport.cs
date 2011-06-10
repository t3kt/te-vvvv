using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Animator.Core.Composition;

namespace Animator.Core.Transport
{

	#region Transport

	public abstract class Transport : IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Common)]
		public abstract TimeSpan Position { get; set; }

		[Category(TEShared.Names.Category_Common)]
		public abstract TransportState State { get; protected set; }

		[Category(TEShared.Names.Category_Transport)]
		public virtual float BeatsPerMinute { get; internal set; }

		#endregion

		#region Events

		public event EventHandler Tick;

		public event EventHandler StateChanged;

		public event EventHandler PositionChanged;

		[Browsable(false)]
		protected EventHandler TickHandler
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

		protected virtual void OnTick()
		{
			var handler = this.Tick;
			if(handler != null)
				handler(this, EventArgs.Empty);
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

		protected Transport()
		{

		}

		~Transport()
		{
			this.Dispose(false);
		}

		#endregion

		#region Methods

		public virtual void SetParameters(IDictionary<string, string> parameters)
		{
		}

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

	[Transport(Key = Export_Key, Description = Export_Description)]
	internal sealed class NullTransport : Transport
	{

		#region Static / Constant

		internal const string Export_Key = "null";
		internal const string Export_Description = "No Transport";

		#endregion

		#region ITransport Members

		public override TimeSpan Position
		{
			get { return TimeSpan.Zero; }
			set { }
		}

		public override TransportState State
		{
			get { return TransportState.Stopped; }
			protected set { }
		}

		public override void Play()
		{
		}

		public override void Stop()
		{
		}

		public override void Pause()
		{
		}

		#endregion

	}

	#endregion

}
