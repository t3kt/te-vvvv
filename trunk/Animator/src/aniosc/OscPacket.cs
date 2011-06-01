#region licence/info

// OSC.NET - Open Sound Control for .NET
// http://luvtechno.net/
//
// Copyright (c) 2006, Yoshinori Kawasaki 
// All rights reserved.
//
// Changes and improvements:
// Copyright (c) 2005-2008 Martin Kaltenbrunner <mkalten@iua.upf.edu>
// As included with    
// http://reactivision.sourceforge.net/
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
// * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// * Neither the name of "luvtechno.net" nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY 
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion licence/info

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TESharedAnnotations;

namespace Animator.Osc
{
	internal abstract class OscPacket
	{
		#region Static / Constant

		protected static void AddBytes(ArrayList data, byte[] bytes)
		{
			foreach(var b in bytes)
			{
				data.Add(b);
			}
		}

		protected static void PadNull(ArrayList data)
		{
			const byte zero = 0;
			var pad = 4 - (data.Count % 4);
			for(var i = 0; i < pad; i++)
			{
				data.Add(zero);
			}
		}

		[NotNull]
		protected static byte[] SwapEndian(byte[] data)
		{
			var swapped = new byte[data.Length];
			for(int i = data.Length - 1, j = 0; i >= 0; i--, j++)
			{
				swapped[j] = data[i];
			}
			return swapped;
		}

		protected static byte[] PackInt(int value)
		{
			var data = BitConverter.GetBytes(value);
			if(BitConverter.IsLittleEndian) data = SwapEndian(data);
			return data;
		}

		protected static byte[] PackLong(long value)
		{
			var data = BitConverter.GetBytes(value);
			if(BitConverter.IsLittleEndian) data = SwapEndian(data);
			return data;
		}

		protected static byte[] PackFloat(float value)
		{
			var data = BitConverter.GetBytes(value);
			if(BitConverter.IsLittleEndian) data = SwapEndian(data);
			return data;
		}

		protected static byte[] PackDouble(double value)
		{
			var data = BitConverter.GetBytes(value);
			if(BitConverter.IsLittleEndian) data = SwapEndian(data);
			return data;
		}

		[NotNull]
		protected static byte[] PackString(string value)
		{
			return Encoding.ASCII.GetBytes(value);
		}

		protected static int UnpackInt(byte[] bytes, ref int start)
		{
			var data = new byte[4];
			for(var i = 0; i < 4; i++, start++) data[i] = bytes[start];
			if(BitConverter.IsLittleEndian) data = SwapEndian(data);
			return BitConverter.ToInt32(data, 0);
		}

		protected static long UnpackLong(byte[] bytes, ref int start)
		{
			var data = new byte[8];
			for(var i = 0; i < 8; i++, start++) data[i] = bytes[start];
			if(BitConverter.IsLittleEndian) data = SwapEndian(data);
			return BitConverter.ToInt64(data, 0);
		}

		protected static float UnpackFloat(byte[] bytes, ref int start)
		{
			var data = new byte[4];
			for(var i = 0; i < 4; i++, start++) data[i] = bytes[start];
			if(BitConverter.IsLittleEndian) data = SwapEndian(data);
			return BitConverter.ToSingle(data, 0);
		}

		protected static double UnpackDouble(byte[] bytes, ref int start)
		{
			var data = new byte[8];
			for(var i = 0; i < 8; i++, start++) data[i] = bytes[start];
			if(BitConverter.IsLittleEndian) data = SwapEndian(data);
			return BitConverter.ToDouble(data, 0);
		}

		[NotNull]
		protected static string UnpackString(byte[] bytes, ref int start)
		{
			var count = 0;
			for(var index = start; bytes[index] != 0; index++, count++) ;
			var s = Encoding.ASCII.GetString(bytes, start, count);
			start += count + 1;
			start = (start + 3) / 4 * 4;
			return s;
		}

		public static OscPacket Unpack(byte[] bytes)
		{
			var start = 0;
			return Unpack(bytes, ref start, bytes.Length);
		}

		public static OscPacket Unpack(byte[] bytes, ref int start, int end)
		{
			if(bytes[start] == '#') return OscBundle.Unpack(bytes, ref start, end);
			else return OscMessage.Unpack(bytes, ref start);
		}

		#endregion

		#region Fields

		protected string address;
		protected byte[] binaryData;

		protected ArrayList values;

		#endregion

		#region Properties

		public byte[] BinaryData
		{
			get
			{
				this.Pack();
				return binaryData;
			}
		}

		public string Address
		{
			get { return address; }
			set
			{
				// TODO: validate
				address = value;
			}
		}

		[NotNull]
		public ArrayList Values
		{
			get { return (ArrayList)values.Clone(); }
		}

		public abstract bool IsBundle { get; }

		#endregion

		#region Constructors

		protected OscPacket()
		{
			this.values = new ArrayList();
		}

		#endregion

		#region Methods

		protected abstract void Pack();

		public abstract void Append(object value);

		#endregion

	}
}