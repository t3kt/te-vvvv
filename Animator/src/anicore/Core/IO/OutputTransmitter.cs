using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
using Animator.Core.Model;
using Animator.Core.Runtime;
using TESharedAnnotations;

[assembly: RegisteredImplementation(typeof(IOutputTransmitter), typeof(OutputTransmitter.NullTransmitter))]
[assembly: RegisteredImplementation(typeof(IOutputTransmitter), "trace", typeof(OutputTransmitter.TraceTransmitter))]

namespace Animator.Core.IO
{

	#region OutputTransmitter

	public abstract class OutputTransmitter : IOutputTransmitter
	{

		#region NullTransmitter

		[Description("No Transmitter")]
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

		[Description("Debug Trace Transmitter")]
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

		private static readonly ImplementationRegistry<IOutputTransmitter> _TypeRegistry;

		static OutputTransmitter()
		{
			_TypeRegistry = new ImplementationRegistry<IOutputTransmitter>();
			_TypeRegistry.SetDefault(typeof(NullTransmitter));
			RegisterTypes(typeof(OutputTransmitter).Assembly);
		}

		public static void RegisterType(string key, Type type)
		{
			Require.ArgNotNull(key, "key");
			Require.ArgNotNull(type, "type");
			_TypeRegistry.RegisterType(key, type);
		}

		public static void RegisterTypes(Assembly assembly)
		{
			_TypeRegistry.RegisterTypes(assembly);
		}

		public static IEnumerable<KeyValuePair<Type, string>> GetRegisteredTypeDescriptions()
		{
			return _TypeRegistry.GetRegisteredTypeDescriptions();
		}

		public static IEnumerable<KeyValuePair<string, string>> GetRegisteredTypeDescriptionsByKey()
		{
			return from entry in _TypeRegistry.GetRegisteredTypeDescriptions()
				   select new KeyValuePair<string, string>(_TypeRegistry.GetTypeKey(entry.Key), entry.Value);
		}

		[NotNull]
		internal static IOutputTransmitter CreateTransmitter([NotNull] Output outputModel)
		{
			Require.ArgNotNull(outputModel, "outputModel");
			var transmitter = _TypeRegistry.CreateImplementation(outputModel.OutputType) ?? new NullTransmitter();
			transmitter.Initialize(outputModel);
			return transmitter;
		}

		public static bool IsNullTransmitter([CanBeNull]IOutputTransmitter transmitter)
		{
			return transmitter == null || transmitter is NullTransmitter;
		}

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