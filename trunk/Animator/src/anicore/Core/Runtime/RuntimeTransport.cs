using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Animator.Core.Transport;

namespace Animator.Core.Runtime
{

	#region RuntimeTransport

	internal sealed class RuntimeTransport : ITransport
	{

		#region Nested Types

		#endregion

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

		public float BeatsPerMinute
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsPlaying
		{
			get { throw new NotImplementedException(); }
		}

		public Time Position
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void Play()
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}

		public void Pause()
		{
			throw new NotImplementedException();
		}

		public void EnqueueAction(Action action, int triggerAlignment)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
