using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Composition;
using Animator.Core.Runtime;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region StepClip

	[ClipDataEditor("Animator.UI.Editors.StepListEditor, " + TEShared.AssemblyRef.ani)]
	[Clip(ElementName = "stepclip", Key = "stepclip", Description = "Step Sequence Clip")]
	public sealed class StepClip : Clip
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ObservableCollection<float> _Steps;

		#endregion

		#region Properties

		[Category(TEShared.Names.Category_Common)]
		public ObservableCollection<float> Steps
		{
			get { return _Steps; }
		}

		[Category(TEShared.Names.Category_Output)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int StepCount
		{
			get { return this._Steps.Count; }
			set
			{
				Require.ArgNotNegative(value, "value");
				if(this._Steps.Count == 0)
				{
					this._Steps.ReplaceContents(Enumerable.Repeat(0.0f, value));
				}
				else if(value < this._Steps.Count)
				{
					while(this._Steps.Count > value)
						this._Steps.RemoveAt(this._Steps.Count - 1);
					this.OnPropertyChanged("Steps");
				}
				else if(value > this._Steps.Count)
				{
					var endVal = this._Steps.LastOrDefault();
					while(this._Steps.Count < value)
						this._Steps.Add(endVal);
					this.OnPropertyChanged("Steps");
				}
			}
		}

		#endregion

		#region Constructors

		public StepClip()
			: this(Guid.NewGuid()) { }

		public StepClip(Guid id)
			: base(id)
		{
			this._Steps = new ObservableCollection<float>();
			this._Steps.CollectionChanged += this.Steps_CollectionChanged;
		}

		public StepClip(XElement element)
			: this()
		{
			this.ReadXElement(element);
		}

		#endregion

		#region Methods

		private int GetStepIndex(Time position)
		{
			if(this.StepCount == 0)
				return 0;
			position = position % this.Duration;
			var stepLength = this.Duration / this.StepCount;
			return (int)(position / stepLength);
		}

		internal override object GetValue(Time position)
		{
			if(this.StepCount == 0)
				return 0.0f;
			var step = this.GetStepIndex(position);
			return this._Steps[step];
		}

		public override void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			base.ReadXElement(element);
			this._Steps.ReplaceContents(element.Elements(Schema.stepclip_step).Select(e => (float)e));
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.stepclip)
				.WithContent(this._Steps.Select(step => new XElement(Schema.stepclip_step, step)));
		}

		private void Steps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Steps");
		}

		public override bool Equals(Clip other)
		{
			if(!base.Equals(other))
				return false;
			var stepClip = other as StepClip;
			if(stepClip == null)
				return false;
			return stepClip._Steps.Count == this._Steps.Count &&
				   stepClip._Steps.SequenceEqual(this._Steps);
		}

		#endregion

	}

	#endregion

}
