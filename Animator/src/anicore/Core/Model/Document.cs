using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region Document

	public class Document : IDocument
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

		#region IDocument Members

		public Time Duration
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public float BeatsPerMinute
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public int TriggerAlignment
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public ICollection<ITrack> Tracks
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public ITrack GetTrack(Guid id)
		{
			throw new NotImplementedException();
		}

		public ICollection<IOutput> Outputs
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public IOutput GetOutput(Guid id)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDocumentItem Members

		IDocumentItem IDocumentItem.Parent
		{
			get { return null; }
		}

		IDocument IDocumentItem.Document
		{
			get { return this; }
		}

		#endregion

		#region IGuidId Members

		public Guid Id { get; private set; }

		#endregion

		#region INamed Members

		public string Name { get; set; }

		#endregion

		#region IDocumentItemContainer Members

		public IDocumentItem GetItem(Guid id)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

}
