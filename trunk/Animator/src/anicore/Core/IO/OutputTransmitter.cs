using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Runtime;

[assembly: RegisteredImplementation(typeof(IOutputTransmitter), typeof(OutputTransmitter.NullTransmitter))]

namespace Animator.Core.IO
{

	#region OutputTransmitter

	public abstract class OutputTransmitter : IOutputTransmitter
	{

		#region NullTransmitter

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

			protected override bool PostMessage(OutputMessage message)
			{
				return false;
			}

			#endregion

		}

		#endregion

		#region Static / Constant

		static OutputTransmitter()
		{
			ImplementationRegistry<IOutputTransmitter>.SetDefault(typeof(NullTransmitter));
			//RegisterType(String.Empty, typeof(NullTransmitter));
			//RegisterType("null", typeof(NullTransmitter));
		}

		public static void RegisterType(string key, Type type)
		{
			Require.ArgNotNull(key, "key");
			Require.ArgNotNull(type, "type");
			ImplementationRegistry<IOutputTransmitter>.RegisterType(key, type);
		}

		public static void RegisterTypes(Assembly assembly)
		{
			ImplementationRegistry<IOutputTransmitter>.RegisterTypes(assembly);
		}

		internal static IOutputTransmitter CreateTransmitter(Output outputModel)
		{
			Require.ArgNotNull(outputModel, "outputModel");
			var transmitter = ImplementationRegistry<IOutputTransmitter>.CreateImplementation(outputModel.OutputType) ?? new NullTransmitter();
			transmitter.Initialize(outputModel);
			return transmitter;
		}

		#endregion

		#region Fields

		private Guid _Id;

		#endregion

		#region Properties

		public Guid Id
		{
			get { return this._Id; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected virtual void InitializeInternal(Output outputModel)
		{

		}

		protected abstract bool PostMessage(OutputMessage message);

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

		public void Initialize(Output outputModel)
		{
			Require.ArgNotNull(outputModel, "outputModel");
			_Id = outputModel.Id;
			this.InitializeInternal(outputModel);
		}

		void IOutputTransmitter.PostMessage(OutputMessage message)
		{
			if(message != null)
			{
				if(!this.PostMessage(message))
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
