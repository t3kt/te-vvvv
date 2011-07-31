using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Animator.Core.Composition;
using Animator.Core.IO;
using Animator.Core.Model;

namespace Animator.Tests.Utils
{

	#region CollectorOutput

	[AniExport(typeof(Output), Key = Export_Key)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	internal sealed class CollectorOutput : Output
	{

		#region Static / Constant

		internal new const string Export_Key = "test.collector";
		internal new const string Export_ElementName = null;
		internal new const string Export_Description = "Collector TEST Output";

		#endregion

		#region Fields

		public readonly List<OutputMessage> Messages = new List<OutputMessage>();

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override bool PostMessageInternal(OutputMessage message)
		{
			if(message != null)
				this.Messages.Add(message);
			return true;
		}

		#endregion

	}

	#endregion

}
