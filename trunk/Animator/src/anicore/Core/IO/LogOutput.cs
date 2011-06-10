using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Model;

namespace Animator.Core.IO
{

	#region LogOutput

	[Output(Key = Export_Key, ElementName = Export_ElementName, Description = Export_Description)]
	public sealed class LogOutput : Output
	{

		#region Static / Constant

		internal new const string Export_Key = "log";
		internal new const string Export_ElementName = "logoutput";
		internal new const string Export_Description = "Log File Output";

		private const string FileNameDateFormat = "yyyyMMdd-HHmmss";
		private const string LogDateFormat = "yyyy.MM.dd HH:mm:ss.ffff";

		#endregion

		#region Fields

		private string _Path;
		private bool _Append;
		private StreamWriter _Writer;

		#endregion

		#region Properties

		public string Path
		{
			get { return this._Path; }
			set
			{
				if(value != this._Path)
				{
					this._Path = value;
					this.OnPropertyChanged("Path");
					this.RefreshStream();
				}
			}
		}

		private string EffectivePath
		{
			get
			{
				if(String.IsNullOrEmpty(this._Path))
					return null;
				return this._Path.Replace("[DATETIME]", DateTime.Now.ToString(FileNameDateFormat));
			}
		}

		public bool Append
		{
			get { return this._Append; }
			set
			{
				if(value != this._Append)
				{
					this._Append = value;
					this.OnPropertyChanged("Append");
					this.RefreshStream();
				}
			}
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		protected override bool PostMessageInternal(OutputMessage message)
		{
			Require.DBG_ArgNotNull(message, "message");
			this.WriteLine(OutputMessage.FormatTrace(message));
			return true;
		}

		private void RefreshStream()
		{
			this.CloseStream();
			var path = this.EffectivePath;
			if(path != null)
			{
				this._Writer = new StreamWriter(path, this._Append);
				this.WriteLine("========== BEGIN ANI OUTPUT LOG ==========");
			}
		}

		private void WriteLine(string mesg)
		{
			if(this._Writer != null)
				this._Writer.WriteLine("[{0:" + LogDateFormat + "}] {1}", DateTime.Now, mesg);
		}

		private void CloseStream()
		{
			if(this._Writer != null)
			{
				this.WriteLine("========== END ANI OUTPUT LOG ==========");
				this._Writer.WriteLine();
				this._Writer.Flush();
				//this._Writer.Close();););
				this._Writer.Dispose();
				this._Writer = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.CloseStream();
		}

		public override void ReadXElement(XElement element)
		{
			base.ReadXElement(element);
			this._Path = (string)element.Attribute(Schema.logoutput_path);
			this._Append = (bool)element.Attribute(Schema.logoutput_append);
			this.RefreshStream();
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.logoutput)
				.WithContent(
					new XAttribute(Schema.logoutput_path, this._Path ?? String.Empty),
					new XAttribute(Schema.logoutput_append, this._Append));
		}

		#endregion

	}

	#endregion

}
