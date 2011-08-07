using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Animator.Common.Diagnostics;
using TESharedAnnotations;

namespace Animator.Core.Runtime.ObjectStates
{

	#region ObjectState

	[TypeDescriptionProvider(typeof(StateType.Provider))]
	public class ObjectState : INotifyPropertyChanged
	{

		#region Static / Constant

		[NotNull]
		public static ObjectState CreateState([NotNull] Type objectType, [NotNull] object target)
		{
			Require.ArgNotNull(objectType, "objectType");
			Require.ArgNotNull(target, "target");
			var type = new StateType(objectType, target);
			return type.ExtractState();
		}

		[NotNull]
		public static ObjectState CreateState<T>([NotNull] T target)
			where T : class
		{
			return CreateState(typeof(T), target);
		}

		#endregion

		#region Fields

		private readonly StateType _Type;
		private readonly object _Target;
		private readonly Dictionary<string, object> _Properties;

		#endregion

		#region Properties

		internal StateType Type
		{
			get { return this._Type; }
		}

		internal object Target
		{
			get { return this._Target; }
		}

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string name)
		{
			var handler = this.PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Constructors

		internal ObjectState([NotNull] StateType type, [NotNull] object target)
		{
			Require.ArgNotNull(type, "type");
			Require.ArgNotNull(target, "target");
			this._Type = type;
			this._Target = target;
			this._Properties = new Dictionary<string, object>();
			this.Load();
		}

		#endregion

		#region Methods

		public void Load()
		{
			this._Properties.Clear();
			this._Type.ExtractState(this);
			this.OnPropertyChanged(null);
		}

		public void Save()
		{
			this._Type.SaveState(this);
		}

		public object GetProperty(string key)
		{
			object value;
			return this._Properties.TryGetValue(key, out value) ? value : null;
		}

		public void SetProperty(string key, object value)
		{
			this._Properties[key] = value;
			this.OnPropertyChanged(key);
		}

		public void RemoveProperty(string key)
		{
			this._Properties.Remove(key);
			this.OnPropertyChanged(key);
		}

		#endregion

	}

	#endregion

}
