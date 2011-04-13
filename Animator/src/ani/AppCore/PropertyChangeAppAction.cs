using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Animator.Common.Diagnostics;

namespace Animator.AppCore
{

	#region PropertyChangeAppAction

	internal sealed class PropertyChangeAppAction : IAppAction
	{

		#region Static / Constant

		private static bool IsStatic(PropertyInfo property)
		{
			return (property.CanRead && property.GetGetMethod().IsStatic) ||
				   (property.CanWrite && property.GetSetMethod().IsStatic);
		}

		private static object GetValue(PropertyInfo property, object target)
		{
			if(!property.CanRead)
				throw new NotSupportedException();
			if(IsStatic(property))
				return property.GetValue(null, null);
			Debug.Assert(target != null);
			return property.GetValue(target, null);
		}

		private static void SetValue(PropertyInfo property, object target, object value)
		{
			if(!property.CanWrite)
				throw new NotSupportedException();
			if(IsStatic(property))
			{
				property.SetValue(null, value, null);
			}
			else
			{
				Debug.Assert(target != null);
				property.SetValue(target, value, null);
			}
		}

		#endregion

		#region Fields

		private readonly string _Name;
		private readonly bool _SupportsUndo;
		private readonly PropertyInfo _Property;
		private readonly string _PropertyName;

		#endregion

		#region Properties

		public bool SupportsUndo
		{
			get { return this._SupportsUndo; }
		}

		public string Name
		{
			get { return this._Name; }
		}

		#endregion

		#region Constructors

		public PropertyChangeAppAction(PropertyInfo property, string name = null, bool supportsUndo = true)
		{
			Require.ArgNotNull(property, "property");
			this._Property = property;
			this._Name = name ?? String.Format("Change {0}", property.Name);
			this._SupportsUndo = supportsUndo;
		}

		public PropertyChangeAppAction(string propertyName, string name = null, bool supportsUndo = true)
		{
			Require.ArgNotNull(propertyName, "propertyName");
			this._PropertyName = propertyName;
			this._Name = name ?? String.Format("Change {0}", propertyName);
			this._SupportsUndo = supportsUndo;
		}

		#endregion

		#region Methods

		private PropertyInfo GetProperty(object target)
		{
			if(this._Property != null)
				return this._Property;
			Require.ArgNotNull(target, "target");
			var property = target.GetType().GetProperty(this._PropertyName);
			Debug.Assert(property != null);
			return property;
		}

		public void Perform(object target, object newState, out object oldState, out bool canUndo)
		{
			var property = this.GetProperty(target);
			if(property.CanRead)
			{
				oldState = GetValue(property, target);
				canUndo = this._SupportsUndo;
			}
			else
			{
				oldState = null;
				canUndo = false;
			}
			SetValue(property, target, newState);
		}

		public void Undo(object target, object oldState)
		{
			if(!this._SupportsUndo)
				throw new NotSupportedException();
			var property = this.GetProperty(target);
			SetValue(property, target, oldState);
		}

		#endregion

	}

	#endregion

}
