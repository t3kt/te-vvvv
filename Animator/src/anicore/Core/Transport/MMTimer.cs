using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Animator.Common.Diagnostics;

namespace Animator.Core.Transport
{

	#region MMTimer

	internal sealed class MMTimer : IDisposable
	{

		#region Static / Constant

		//private static readonly ConcurrentDictionary<uint, int> _ResolutionUsageCounts = new ConcurrentDictionary<uint, int>();

		//private static void IncrementCount(uint resolution)
		//{
		//    _ResolutionUsageCounts.AddOrUpdate(resolution,
		//                                       r =>
		//                                       {
		//                                           NativeMethods.timeBeginPeriod(r);
		//                                           return 1;
		//                                       },
		//                                       (r, c) =>
		//                                       {
		//                                           if(c <= 0)
		//                                           {
		//                                               c = 0;
		//                                               NativeMethods.timeBeginPeriod(r);
		//                                           }
		//                                           return c + 1;
		//                                       });
		//}

		//private static void DecrementCount(uin)

		private static Func<bool> WrapCallback(Action callback)
		{
			Require.ArgNotNull(callback, "callback");
			return () =>
					{
						callback();
						return true;
					};
		}

		#endregion

		#region Fields

		private readonly Func<bool> _Callback;
		private readonly uint _Delay;
		private readonly uint _Resolution;
		private readonly NativeMethods.fuEvent _Flags;
		private uint _Id;
		private bool _Disposed;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public MMTimer(Func<bool> callback, uint msDelay, uint msResolution = 0u, bool repeat = true)
		{
			Require.ArgNotNull(callback, "callback");
			this._Callback = callback;
			this._Delay = msDelay;
			this._Resolution = msResolution;
			this._Flags = NativeMethods.fuEvent.TIME_CALLBACK_FUNCTION | (repeat ? NativeMethods.fuEvent.TIME_PERIODIC : NativeMethods.fuEvent.TIME_ONESHOT);
		}

		public MMTimer(Action callback, uint msDelay, uint msResolution = 0u, bool repeat = true)
			: this(WrapCallback(callback), msDelay, msResolution, repeat)
		{
		}

		~MMTimer()
		{
			this.Dispose(false);
		}

		#endregion

		#region Methods

		public void Start()
		{
			this.Stop();
			lock(this)
			{
				this._Id = NativeMethods.timeSetEvent(this._Delay, this._Resolution, this.EventCallback, UIntPtr.Zero, this._Flags);
				if(this._Id == 0)
				{
					var error = Marshal.GetLastWin32Error();
					throw new Win32Exception(error);
				}
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
