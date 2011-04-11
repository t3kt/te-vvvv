using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Animator.Common;
using Animator.Common.Diagnostics;
using Animator.Core.Transport;

namespace Animator.Core.Model
{

	#region StepClip

	public sealed class StepClip : Clip
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly ObservableCollection<float> _Steps;

		#endregion

		#region Properties

		public IList<float> Steps
		{
			get { return _Steps; }
		}

		public int StepCount
		{
			get { return _Steps.Count; }
			set
			{
				Require.ArgPositive(value, "value");
				if(value < _Steps.Count)
				{
					using(this.SuspendNotifyScope())
					{
						while(_Steps.Count > value)
							_Steps.RemoveAt(_Steps.Count - 1);
					}
					OnPropertyChanged("Steps");
				}
				else if(value > _Steps.Count)
				{
					using(this.SuspendNotifyScope())
					{
						var endVal = _Steps.LastOrDefault();
						while(_Steps.Count < value)
							_Steps.Add(endVal);
					}
					OnPropertyChanged("Steps");
				}
			}
		}

		#endregion

		#region Constructors

		private StepClip(IDocumentItem parent)
			: base(parent)
		{
			_Steps = new ObservableCollection<float> { 0.0f };
			_Steps.CollectionChanged += this.Steps_CollectionChanged;
		}

		public StepClip(IDocumentItem parent, Guid id)
			: this(parent)
		{
			this.Id = id;
		}

		public StepClip(IDocumentItem parent, XElement element)
			: this(parent)
		{
			base.ReadXElement(element);
			this.ReadXElement(element);
		}

		#endregion

		#region Methods

		public void SetSteps(IList<float> steps)
		{
			Require.ArgNotNull(steps, "steps");
			this.StepCount = steps.Count;
			for(var i = 0; i < steps.Count; i++)
				this.Steps[i] = steps[i];
		}

		public void SetSteps(params float[] steps)
		{
			SetSteps((IList<float>)steps);
		}

		private int GetStep(Time position)
		{
			position = position % this.Duration;
			var stepLength = this.Duration / this.StepCount;
			//return (int)Math.Round((double)(position / stepLength));
			return (int)(position / stepLength);
		}

		internal override object GetValue(Time position)
		{
			if(this.StepCount == 0)
				return 0.0f;
			var step = this.GetStep(position);
			return this._Steps[step];
		}

		private new void ReadXElement(XElement element)
		{
			Require.ArgNotNull(element, "element");
			SuspendNotify();
			try
			{
				this.Id = (Guid)element.Attribute(Schema.clip_id);
				this.Name = (string)element.Attribute(Schema.clip_name);
				this.Duration = (float)element.Attribute(Schema.clip_dur);
				this.TriggerAlignment = (int?)element.Attribute(Schema.clip_align) ?? Document.NoAlignment;
				this.ClipType = (string)element.Attribute(Schema.clip_type);
				this.Parameters = ModelUtil.ReadParametersXElement(element.Element(Schema.clip_params));
				var steps = element.Elements(Schema.stepclip_step).Select(e => (float)e).ToList();
				Debug.Assert(steps.Count > 0);
				if(steps.Count == 0)
					steps = new List<float> { 0.0f };
				_Steps.Clear();
				_Steps.AddRange(steps);
			}
			finally
			{
				ResumeNotify();
				OnPropertyChanged(null);
			}
		}

		public override XElement WriteXElement(XName name = null)
		{
			return base.WriteXElement(name ?? Schema.stepclip)
				.WithContent(from step in _Steps
							 select new XElement(Schema.stepclip_step, step));
		}

		private void Steps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged("Steps");
		}

		public override bool Equals(Clip other)
		{
			if(!base.Equals(other))
				return false;
			var stepClip = other as StepClip;
			if(stepClip == null)
				return false;
			return stepClip._Steps.Count != this._Steps.Count &&
				   stepClip._Steps.SequenceEqual(this._Steps);
		}

		#endregion

	}

	#endregion

}
