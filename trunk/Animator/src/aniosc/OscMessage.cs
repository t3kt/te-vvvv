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
	internal sealed class OscMessage : OscPacket
	{
		#region Static / Constant

		internal const char INTEGER = 'i';
		internal const char FLOAT = 'f';
		internal const char LONG = 'h';
		internal const char DOUBLE = 'd';
		internal const char STRING = 's';
		internal const char SYMBOL = 'S';
		//internal const char BLOB = 'b';
		//internal const char ALL = '*';

		[NotNull]
		public static OscMessage Unpack(byte[] bytes, ref int start)
		{
			var address = UnpackString(bytes, ref start);
			//Console.WriteLine("address: " + address);
			var msg = new OscMessage(address);

			var tags = UnpackString(bytes, ref start).ToCharArray();
			//Console.WriteLine("tags: " + new string(tags));
			foreach(var tag in tags)
			{
				//Console.WriteLine("tag: " + tag + " @ "+start);
				if(tag == ',') continue;
				else if(tag == INTEGER) msg.Append(UnpackInt(bytes, ref start));
				else if(tag == LONG) msg.Append(UnpackLong(bytes, ref start));
				else if(tag == DOUBLE) msg.Append(UnpackDouble(bytes, ref start));
				else if(tag == FLOAT) msg.Append(UnpackFloat(bytes, ref start));
				else if(tag == STRING || tag == SYMBOL) msg.Append(UnpackString(bytes, ref start));
				else Console.WriteLine("unknown tag: " + tag);
			}

			return msg;
		}

		#endregion

		#region Fields

		private string typeTag;

		#endregion

		#region Properties

		public override bool IsBundle
		{
			get { return false; }
		}

		#endregion

		#region Constructors

		public OscMessage(string address)
		{
			this.typeTag = ",";
			this.Address = address;
		}

		#endregion

		#region Methods

		protected override void Pack()
		{
			var data = new ArrayList();

			AddBytes(data, PackString(this.address));
			PadNull(data);
			AddBytes(data, PackString(this.typeTag));
			PadNull(data);

			foreach(var value in this.Values)
			{
				if(value is int) AddBytes(data, PackInt((int)value));
				else if(value is long) AddBytes(data, PackLong((long)value));
				else if(value is float) AddBytes(data, PackFloat((float)value));
				else if(value is double) AddBytes(data, PackDouble((double)value));
				else if(value is string)
				{
					AddBytes(data, PackString((string)value));
					PadNull(data);
				}
				else
				{
					// TODO
				}
			}

			this.binaryData = (byte[])data.ToArray(typeof(byte));
		}

		public override void Append(object value)
		{
			if(value is int)
			{
				AppendTag(INTEGER);
			}
			else if(value is long)
			{
				AppendTag(LONG);
			}
			else if(value is float)
			{
				AppendTag(FLOAT);
			}
			else if(value is double)
			{
				AppendTag(DOUBLE);
			}
			else if(value is string)
			{
				AppendTag(STRING);
			}
			else
			{
				throw new ArgumentException("Type of value not supported", "value");
			}
			values.Add(value);
		}

		private void AppendTag(char type)
		{
			this.typeTag += type;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(this.Address + " ");
			foreach(var t in values)
				sb.Append(t + " ");
			return sb.ToString();
		}

		#endregion

	}
}