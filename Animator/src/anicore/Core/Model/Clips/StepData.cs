using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Windows.Markup;
using System.Xml.Linq;
using Animator.Common;
using Animator.Core.Composition;
using Animator.Core.Runtime;

namespace Animator.Core.Model.Clips
{

	#region StepData

	[AniExport(typeof(ClipPropertyData),
		Key = Export_Key,
		ElementName = Export_ElementName,
		Description = Export_Description)]
	[ClipPropertyDataEditor("Animator.UI.Clips.StepDataEditor, " + TEShared.AssemblyRef.ani)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	[ContentProperty("Steps")]
	public sealed class StepData : ClipPropertyData
	{

		#region Static / Constant

		internal const string Export_Key = "step";
		internal const string Export_ElementName = "stepprop";
		internal const string Export_Description = "Step Data";

		#endregion

		#region Fields

		private readonly ObservableCollection<double> _Steps;

		#endregion

		#region Properties

		public ObservableCollection<double> Steps
		{
			get { return this._Steps; }
		}

		#endregion

		#region Constructors

		public StepData()
		{
			this._Steps = new ObservableCollection<double>();
			this._Steps.CollectionChanged += this.Steps_CollectionChanged;
		}

		#endregion

		#region Methods

		public bool SetCount(int count)
		{
			if(count <= 0 || count == this._Steps.Count)
				return false;
			if(count < this._Steps.Count)
			{
				while(count < this._Steps.Count)
					this._Steps.RemoveAt(this._Steps.Count - 1);
			}
			else
			{
				while(count > this._Steps.Count)
					this._Steps.Add(0);
			}
			return true;
		}

		public void Resize(int amount)
		{
			if(amount == 0)
				return;
			if(amount < 0)
			{
				for(var i = 0; this._Steps.Count > 1 && i < -amount; i++)
					this._Steps.RemoveAt(this._Steps.Count - 1);
			}
			else
			{
				for(var i = 0; i < amount; i++)
					this._Steps.Add(0);
			}
		}

		public override void ReadXElement(XElement element)
		{
			base.ReadXElement(element);
			this._Steps.Clear();
			this._Steps.AddRange(element.Elements(Schema.stepprop_step).Select(e => (double)e));
		}

		private void Steps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnPropertyChanged("Steps");
		}

		internal int GetIndex(double position)
		{
			if(this._Steps.Count == 0)
				return 0;
			position = CommonUtil.Clamp(position, 0, 1);
			var i = position * this._Steps.Count;
			var j = (double)Math.Floor(i);
			return (int)CommonUtil.Clamp(j, 0, this._Steps.Count - 1);
			//return (int)Math.Floor(CommonUtil.MapClamped(position, 0f, 1f, 0f, this._Steps.Count - 1));
		}

		public override object GetValue(double position)
		{
			if(this._Steps.Count == 0)
				return 0f;
			var i = this.GetIndex(position);
			Debug.Assert(i >= 0 && i < this._Steps.Count);
			return this._Steps[i];
		}

		public override XElement WriteXElement(XName name = null)
		{
			return new XElement(name ?? Schema.stepprop,
				this.WriteCommonXAttributes(),
				this._Steps.Select(s => new XElement(Schema.stepprop_step, s)));
		}

		#endregion

	}

	#endregion

}
