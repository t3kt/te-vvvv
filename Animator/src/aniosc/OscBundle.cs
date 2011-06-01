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
using TESharedAnnotations;

namespace Animator.Osc
{
	internal class OscBundle : OscPacket
	{
		#region Static / Constant

		protected const string BUNDLE = "#bundle";

		[CanBeNull]
		public new static OscBundle Unpack(byte[] bytes, ref int start, int end)
		{
			var address = UnpackString(bytes, ref start);
			//Console.WriteLine("bundle: " + address);
			if(!address.Equals(BUNDLE)) return null; // TODO

			var timestamp = UnpackLong(bytes, ref start);
			var bundle = new OscBundle(timestamp);

			while(start < end)
			{
				var length = UnpackInt(bytes, ref start);
				var sub_end = start + length;
				//Console.WriteLine(bytes.Length +" "+ start+" "+length+" "+sub_end);
				bundle.Append(OscPacket.Unpack(bytes, ref start, sub_end));
			}

			return bundle;
		}

		#endregion

		#region Fields

		private readonly long timestamp;

		#endregion

		#region Properties

		public override bool IsBundle
		{
			get { return true; }
		}

		#endregion

		#region Constructors

		public OscBundle(long ts)
		{
			this.address = BUNDLE;
			this.timestamp = ts;
		}

		public OscBundle()
			: this(0) { }

		#endregion

		#region Methods

		protected override void Pack()
		{
			var data = new ArrayList();

			AddBytes(data, PackString(this.Address));
			PadNull(data);
			AddBytes(data, PackLong(0)); // TODO

			foreach(var value in this.Values)
			{
				if(value is OscPacket)
				{
					var bs = ((OscPacket)value).BinaryData;
					AddBytes(data, PackInt(bs.Length));
					AddBytes(data, bs);
				}
				else
				{
					// TODO
				}
			}

			this.binaryData = (byte[])data.ToArray(typeof(byte));
		}

		public long getTimeStamp()
		{
			return timestamp;
		}

		public override void Append(object value)
		{
			if(!(value is OscPacket))
				throw new ArgumentException("Value must be an OscPacket", "value");
			values.Add(value);
		}

		#endregion

	}
}