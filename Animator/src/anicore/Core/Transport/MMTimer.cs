using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Common.Diagnostics;

namespace Animator.Core.Transport
{

	#region MMTimer

	internal sealed class MMTimer : IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly Func<bool> _Callback;
		private uint _Id;
		private bool _Disposed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public MMTimer(Func<bool> callback)
		{
			Require.ArgNotNull(callback, "callback");
			this._Callback = callback;
		}

		public MMTimer(Action callback)
		{
			Require.ArgNotNull(callback, "callback");
			this._Callback = () =>
							 {
								 callback();
								 return true;
							 };
		}

		~MMTimer()
		{
			this.Dispose(false);
		}

		#endregion

		#region Methods

		public void Start(uint ms, bool repeat)
		{
			this.Stop();
			var flags = NativeMethods.fuEvent.TIME_CALLBACK_FUNCTION | (repeat ? NativeMethods.fuEvent.TIME_PERIODIC : NativeMethods.fuEvent.TIME_ONESHOT);
			lock(this)
			{
				this._Id = NativeMethods.timeSetEvent(ms, 0, this.EventCallback, UIntPtr.Zero, flags);
				if(this._Id == 0)
					throw new NotImplementedException();
			}
		}

		public void Stop()
		{
			lock(this)
			{
				if(this._Id != 0)
				{
					NativeMethods.timeKillEvent(this._Id);
					this._Id = 0;
				}
			}
		}

		private void EventCallback(uint id, uint msg, ref UIntPtr userCtx, UIntPtr rsv1, UIntPtr rsv2)
		{
			if(!this._Callback())
				this.Stop();
		}

		private void Dispose(bool disposing)
		{
			if(disposing && !this._Disposed)
			{
				this.Stop();
			}
			this._Disposed = true;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}

	#endregion

}
