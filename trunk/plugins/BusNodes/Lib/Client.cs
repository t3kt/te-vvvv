using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VVVV.Lib
{

	#region Client<T>

	public class Client<T> : IClient<T>
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly IBus<T> _Bus;
		private readonly Guid _Id;
		private IPort<T> _Port;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public Client(IBus<T> bus)
			: this(bus, Guid.NewGuid()) { }

		public Client(IBus<T> bus, Guid id)
		{
			Debug.Assert(bus != null);
			Debug.Assert(id != Guid.Empty);
			_Bus = bus;
			_Id = id;
		}

		#endregion

		#region Methods

		protected void AttachPort(IPort<T> port)
		{
			throw new NotImplementedException();
		}

		protected void ReleasePort()
		{
			throw new NotImplementedException();
		}

		public void SetKey(string key)
		{
			
		}

		protected virtual void Dispose(bool disposing)
		{

		}

		#endregion

		#region IGuidId Members

		public Guid Id
		{
			get { return _Id; }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
