using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Transport;

namespace Animator.Core.Composition
{

	#region TransportAttribute

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class TransportAttribute : KeyedExportAttribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public TransportAttribute()
			: base(typeof(ITransport))
		{

		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region ITransportMetadata

	public interface ITransportMetadata : IKeyedExportMetadata
	{
	}

	#endregion

}
