using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendReceiveNodes.Core
{

	#region IPort

	internal interface IPort
	{

		string Key { get; }

		event EventHandler<PortEventArgs> RefCountChanged;

		event EventHandler<PortEventArgs> DataUpdated;

		void AddReference();

		bool RemoveReference();

		Array GetData();

		void SetData(Array data);

	}

	#endregion

	#region Port<T>

	internal class Port<T> : IPort
	{

		#region Static/Constant

		private static readonly T[] _Empty = new T[0];

		#endregion

		#region Fields

		private readonly string _Key;
		private int _RefCount;
		private T[] _Data;

		#endregion

		#region Properties

		public string Key
		{
			get { return this._Key; }
		}

		#endregion

		#region Events

		public event EventHandler<PortEventArgs> RefCountChanged;

		public event EventHandler<PortEventArgs> DataUpdated;

		#endregion

		#region Constructors

		public Port(string key)
		{
			this._Key = key;
		}

		#endregion

		#region Methods

		private void OnRefCountChanged()
		{
			var handler = this.RefCountChanged;
			if(handler != null)
				handler(this, new PortEventArgs(this));
		}

		public void AddReference()
		{
			this._RefCount++;
			this.OnRefCountChanged();
		}

		public bool RemoveReference()
		{
			this._RefCount--;
			this.OnRefCountChanged();
			if(this._RefCount <= 0)
			{
				this._RefCount = 0;
				return true;
			}
			return false;
		}

		public T[] GetData()
		{
			return this._Data ?? _Empty;
		}

		public void SetData(T[] data)
		{
			this._Data = data;
			var handler = this.DataUpdated;
			if(handler != null)
				handler(this, new PortEventArgs(this));
		}

		#endregion

		#region IPort Members

		Array IPort.GetData()
		{
			return this.GetData();
		}

		void IPort.SetData(Array data)
		{
			this.SetData((T[])data);
		}

		#endregion

	}

	#endregion

}
