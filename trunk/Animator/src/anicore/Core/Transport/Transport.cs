using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Runtime;
using Animator.Core.Transport;
using TESharedAnnotations;

[assembly: RegisteredImplementation(typeof(ITransport), typeof(Transport.NullTransport))]
[assembly: RegisteredImplementation(typeof(ITransport), "null", typeof(Transport.NullTransport))]

namespace Animator.Core.Transport
{

	#region Transport

	public abstract class Transport : ITransport, IDisposable
	{

		#region NullTransport

		internal sealed class NullTransport : ITransport
		{

			#region Static / Constant

			#endregion

			#region Fields

			#endregion

			#region Properties

			#endregion

			#region Constructors

			#endregion

			#region Methods

			#endregion

			#region ITransport Members

			public void SetParameters(IDictionary<string, string> parameters)
			{
			}

			public Time Position
			{
				get { return 0; }
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

			public event EventHandler PositionChanged;

			public event EventHandler StateChanged;

			public event EventHandler Tick;

			#endregion

		}

		#endregion

		#region Static / Constant

		private static readonly ImplementationRegistry<ITransport> _TypeRegistry;

		public static IImplementationRegistry TypeRegistry
		{
			get { return _TypeRegistry; }
		}

		static Transport()
		{
			_TypeRegistry = new ImplementationRegistry<ITransport>();
			_TypeRegistry.SetDefault(typeof(NullTransport));
			_TypeRegistry.RegisterTypes(typeof(Transport).Assembly);
		}

		[NotNull]
		internal static ITransport CreateTransport([CanBeNull] string transportType, [CanBeNull]IDictionary<string, string> parameters)
		{
			var transport = _TypeRegistry.CreateImplementation(transportType) ?? new NullTransport();
			transport.SetParameters(parameters);
			return transport;
		}

		internal static bool IsNullTransport([CanBeNull]ITransport transport)
		{
			return transport == null || transport is NullTransport;
		}

		#endregion

		#region Fields

		private Time _Position;
		private TransportState _State;

		#endregion

		#region Properties

		public virtual void SetParameters(IDictionary<string, string> parameters)
		{
		}

		public virtual Time Position
		{
			get { return this._Position; }
			set
			{
				if(value != this._Position)
				{
					this._Position = value;
					this.OnPositionChanged();
				}
			}
		}

		public virtual TransportState State
		{
			get { return this._State; }
			protected set
			{
				if(value != this._State)
				{
					this._State = value;
					this.OnStateChanged();
				}
			}
		}

		#endregion

		#region Events

		public event EventHandler Tick;

		public event EventHandler StateChanged;

		public event EventHandler PositionChanged;

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

		public virtual void Play()
		{
			this.State = TransportState.Playing;
		}

		public virtual void Stop()
		{
			this.State = TransportState.Stopped;
			this.Position = 0;
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

}
