using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animator.Common.Diagnostics;
using Animator.Core.IO;
using Animator.Core.Model;

namespace Animator.Core.Runtime
{

	#region RuntimeOutput

	internal class RuntimeOutput : RuntimeDocumentItem
	{

		#region Static / Constant

		internal static RuntimeOutput CreateOutput(RuntimeDocument runtimeDocument, Output output)
		{
			return new RuntimeOutput(runtimeDocument, output);
		}

		#endregion

		#region Fields

		private readonly Output _Output;
		private IOutputTransmitter _Transmitter;

		#endregion

		#region Properties

		public override DocumentItem Item
		{
			get { return this._Output; }
		}

		public Output Output
		{
			get { return this._Output; }
		}

		public IOutputTransmitter Transmitter
		{
			get
			{
				if(this._Transmitter == null)
					this.InitializeTransmitter();
				return this._Transmitter;
			}
		}

		#endregion

		#region Constructors

		public RuntimeOutput(RuntimeDocument runtimeDocument, Output output)
			: base(runtimeDocument)
		{
			Require.ArgNotNull(output, "output");
			this._Output = output;
			this.AttachItem(output);
		}

		#endregion

		#region Methods

		private void InitializeTransmitter()
		{
			this._Transmitter = OutputTransmitter.CreateTransmitter(this._Output);
		}

		protected override void OnPropertyChanged(string name)
		{
			if(name == "Item.OutputType")
				this.InitializeTransmitter();
			base.OnPropertyChanged(name);
		}

		#endregion

	}

	#endregion

}
