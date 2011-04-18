using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common.Diagnostics;
using Animator.Core.Model;
using Animator.Core.Runtime;
using Animator.Core.Transport;

[assembly: RegisteredImplementation(typeof(Clip), "stepclip", typeof(StepClip))]

namespace Animator.Core.Model
{

	#region StepClip

	[Description("Step Sequence Clip")]
	public sealed class StepClip : Clip
	{

		#region Static / Constant

		#endregion

		#region Fields

		private ObservableCollection<float> _Steps;

		#endregion

		#region Properties

		public ObservableCollection<float> Steps
		{
			get
			{
				if(this._Steps == null)
				{
					this._Steps = new ObservableCollection<float>();
					this.AttachSteps(this._Steps);
				}
				return _Steps;
			}
			set
			{
				if(value != this._Steps)
				{
					this.DetachSteps(this._Steps);
					this._Steps = value;
					this.AttachSteps(value);
					this.OnPropertyChanged("Steps");
				}
			}
		}

		public int StepCount
		{
			get { return this._Steps == null ? 0 : this._Steps.Count; }
			set
			{
				Require.ArgNotNegative(value, "value");
				if(this._Steps == null)
				{
					this.Steps = new ObservableCollection<float>(Enumerable.Repeat(0.0f, value));
				}
				else if(value < this._Steps.Count)
				{
					using(this.SuspendNotifyScope())
					{
						while(this._Steps.Count > value)
							this._Steps.RemoveAt(this._Steps.Count - 1);
					}
					this.OnPropertyChanged("Steps");
				}
				else if(value > this._Steps.Count)
				{
					using(this.SuspendNotifyScope())
					{
						var endVal = this._Steps.LastOrDefault();
						while(this._Steps.Count < value)
							this._Steps.Add(endVal);
					}
					this.OnPropertyChanged("Steps");
				}
			}
		}

		#endregion

		#region Constructors

		public StepClip() { }

		public StepClip(Guid id)
			: base(id) { }

		public StepClip(XElement element)
			: base(element)
		{
			this.ReadXElement(element);
		}

		#endregion

		#region Methods

		private void AttachSteps(ObservableCollection<float> steps)
		{
			if(steps != null)
				steps.CollectionChanged += this.Steps_CollectionChanged;
		}

		private void DetachSteps(ObservableCollection<float> steps)
		{
			if(steps != null)
				steps.CollectionChanged -= this.Steps_CollectionChanged;
		}

		public void SetSteps(IList<float> steps)
		{
			Require.ArgNotNull(steps, "steps");
			this.StepCount = steps.Count;
			for(var i = 0; i < steps.Count; i++)
				this.Steps[i] = steps[i];
		}

		private int GetStep(Time position)
		{
			position = position % this.Duration;
			var stepLength = this.Duration / this.StepCount;
			return (int)(position / stepLength);
		}

		internal override object GetValue(Time position)
		{
			if(this.StepCount == 0)
				return 0.0f;
			var step = this.GetStep(position);
			return this._Steps[step];
		}

		private void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			this.SuspendNotify();
			try
			{
				this.Id = (Guid)element.Attribute(Schema.clip_id);
				this.Name = (string)element.Attribute(Schema.clip_name);
				this.Duration = (float)element.Attribute(Schema.clip_dur);
				this.TriggerAlignment = (int?)element.Attribute(Schema.clip_align) ?? Document.NoAlignment;
				this.Steps = new ObservableCollection<float>(element.Elements(Schema.stepclip_step).Select(e => (float)e));
			}
			finally
			{
				this.ResumeNotify();
				this.OnPropertyChanged(null);
			}
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.stepclip)
				.WithContent(this._Steps == null ? null : this._Steps.Select(step => new XElement(Schema.stepclip_step, step)));
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

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing)
			{
				this.DetachSteps(this._Steps);
				this._Steps = null;
			}
		}

		#endregion

	}

	#endregion

}
