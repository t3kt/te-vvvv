#if !DEBUG
#undef ANNOTATIONS
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TESharedAnnotations
{

	#region JetBrains Annotations
#pragma warning disable 1591

	/// <summary>
	/// Indicates that marked element should be localized or not.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class LocalizationRequiredAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationRequiredAttribute"/> class.
		/// </summary>
		/// <param name="required"><see langword="true"/> if a element should be localized; otherwise, <see langword="false"/>.</param>
		public LocalizationRequiredAttribute(bool required)
		{
			Required = required;
		}

		/// <summary>
		/// Gets a value indicating whether a element should be localized.
		/// <value><see langword="true"/> if a element should be localized; otherwise, <see langword="false"/>.</value>
		/// </summary>
		public bool Required { get; set; }

		/// <summary>
		/// Returns whether the value of the given object is equal to the current <see cref="LocalizationRequiredAttribute"/>.
		/// </summary>
		/// <param name="obj">The object to test the value equality of. </param>
		/// <returns>
		/// <see langword="true"/> if the value of the given object is equal to that of the current; otherwise, <see langword="false"/>.
		/// </returns>
		public override bool Equals(object obj)
		{
			var attribute = obj as LocalizationRequiredAttribute;
			return attribute != null && attribute.Required == Required;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A hash code for the current <see cref="LocalizationRequiredAttribute"/>.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	/// <summary>
	/// Indicates that marked method builds string by format pattern and (optional) arguments. 
	/// Parameter, which contains format string, should be given in constructor.
	/// The format string should be in <see cref="string.Format(IFormatProvider,string,object[])"/> -like form
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class StringFormatMethodAttribute : Attribute
	{
		private readonly string myFormatParameterName;

		/// <summary>
		/// Initializes new instance of StringFormatMethodAttribute
		/// </summary>
		/// <param name="formatParameterName">Specifies which parameter of an annotated method should be treated as format-string</param>
		public StringFormatMethodAttribute(string formatParameterName)
		{
			myFormatParameterName = formatParameterName;
		}

		/// <summary>
		/// Gets format parameter name
		/// </summary>
		public string FormatParameterName
		{
			get { return myFormatParameterName; }
		}
	}

	/// <summary>
	/// Indicates that the function argument should be string literal and match one  of the parameters of the caller function.
	/// For example, <see cref="ArgumentNullException"/> has such parameter.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class InvokerParameterNameAttribute : Attribute
	{
	}

	/// <summary>
	/// Indicates that the marked method is assertion method, i.e. it halts control flow if one of the conditions is satisfied. 
	/// To set the condition, mark one of the parameters with <see cref="AssertionConditionAttribute"/> attribute
	/// </summary>
	/// <seealso cref="AssertionConditionAttribute"/>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class AssertionMethodAttribute : Attribute
	{
	}

	/// <summary>
	/// Indicates the condition parameter of the assertion method. 
	/// The method itself should be marked by <see cref="AssertionConditionType"/> attribute.
	/// The mandatory argument of the attribute is the assertion type.
	/// </summary>
	/// <seealso cref="AssertionMethodAttribute"/>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class AssertionConditionAttribute : Attribute
	{
		private readonly AssertionConditionType myConditionType;

		/// <summary>
		/// Initializes new instance of AssertionConditionAttribute
		/// </summary>
		/// <param name="conditionType">Specifies condition type</param>
		public AssertionConditionAttribute(AssertionConditionType conditionType)
		{
			myConditionType = conditionType;
		}

		/// <summary>
		/// Gets condition type
		/// </summary>
		public AssertionConditionType ConditionType
		{
			get { return myConditionType; }
		}
	}

	/// <summary>
	/// Specifies assertion type. If the assertion method argument satisifes the condition, then the execution continues. 
	/// Otherwise, execution is assumed to be halted
	/// </summary>
	internal enum AssertionConditionType
	{
		/// <summary>
		/// Indicates that the marked parameter should be evaluated to true
		/// </summary>
		IS_TRUE = 0,

		/// <summary>
		/// Indicates that the marked parameter should be evaluated to false
		/// </summary>
		IS_FALSE = 1,

		/// <summary>
		/// Indicates that the marked parameter should be evaluated to null value
		/// </summary>
		IS_NULL = 2,

		/// <summary>
		/// Indicates that the marked parameter should be evaluated to not null value
		/// </summary>
		IS_NOT_NULL = 3,
	}

	/// <summary>
	/// Indicates that the marked method unconditionally terminates control flow execution.
	/// For example, it could unconditionally throw exception
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class TerminatesProgramAttribute : Attribute
	{
	}

	/// <summary>
	/// Indicates that the value of marked element could be <see langword="null"/> sometimes, so the check for <see langword="null"/> is necessary before its usage
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class CanBeNullAttribute : Attribute
	{
	}

	/// <summary>
	/// Indicates that the value of marked element could never be <see langword="null"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class NotNullAttribute : Attribute
	{
	}

	/// <summary>
	/// Indicates that the value of marked type (or its derivatives) cannot be compared using '==' or '!=' operators.
	/// There is only exception to compare with <see langword="null"/>, it is permitted
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class CannotApplyEqualityOperatorAttribute : Attribute
	{
	}

	/// <summary>
	/// When applied to target attribute, specifies a requirement for any type which is marked with 
	/// target attribute to implement or inherit specific type or types
	/// </summary>
	/// <example>
	/// <code>
	/// [BaseTypeRequired(typeof(IComponent)] // Specify requirement
	/// public class ComponentAttribute : Attribute 
	/// {}
	/// 
	/// [Component] // ComponentAttribute requires implementing IComponent interface
	/// public class MyComponent : IComponent
	/// {}
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	[BaseTypeRequired(typeof(Attribute))]
	[Conditional("ANNOTATIONS")]
	internal sealed class BaseTypeRequiredAttribute : Attribute
	{
		private readonly Type[] myBaseTypes;

		/// <summary>
		/// Initializes new instance of BaseTypeRequiredAttribute
		/// </summary>
		/// <param name="baseTypes">Specifies which types are required</param>
		public BaseTypeRequiredAttribute(params Type[] baseTypes)
		{
			myBaseTypes = baseTypes;
		}

		/// <summary>
		/// Gets enumerations of specified base types
		/// </summary>
		public IEnumerable<Type> BaseTypes
		{
			get { return myBaseTypes; }
		}
	}

	/// <summary>
	/// Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library),
	/// so this symbol will not be marked as unused (as well as by other usage inspections)
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class UsedImplicitlyAttribute : Attribute
	{
		[UsedImplicitly]
		public UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			UseKindFlags = useKindFlags;
			TargetFlags = targetFlags;
		}

		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; private set; }

		/// <summary>
		/// Gets value indicating what is meant to be used
		/// </summary>
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}

	/// <summary>
	/// Should be used on attributes and causes ReSharper to not mark symbols marked with such attributes as unused (as well as by other usage inspections)
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	[Conditional("ANNOTATIONS")]
	internal sealed class MeansImplicitUseAttribute : Attribute
	{
		[UsedImplicitly]
		public MeansImplicitUseAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			UseKindFlags = useKindFlags;
			TargetFlags = targetFlags;
		}

		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; private set; }

		/// <summary>
		/// Gets value indicating what is meant to be used
		/// </summary>
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}

	[Flags]
	internal enum ImplicitUseKindFlags
	{
		Default = Access | Assign | Instantiated,

		/// <summary>
		/// Only entity marked with attribute considered used
		/// </summary>
		Access = 1,

		/// <summary>
		/// Indicates implicit assignment to a member
		/// </summary>
		Assign = 2,

		/// <summary>
		/// Indicates implicit instantiation of a type
		/// </summary>
		Instantiated = 4,
	}

	/// <summary>
	/// Specify what is considered used implicitly when marked with <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>
	/// </summary>
	[Flags]
	internal enum ImplicitUseTargetFlags
	{
		Default = Itself,

		Itself = 1,

		/// <summary>
		/// Members of entity marked with attribute are considered used
		/// </summary>
		Members = 2,

		/// <summary>
		/// Entity marked with attribute and all its members considered used
		/// </summary>
		WithMembers = Itself | Members
	}

	internal enum PointKinds
	{
		This,
		Ret,
		Par,
		LamPar,
		LamRet
	}

	internal enum PointPlurality
	{
		El,
		Col
	}

	internal sealed class ValueFlowAttribute
	{
		// Methods
		public ValueFlowAttribute(PointPlurality fromPlurality, PointKinds fromPointKinds, byte fromParameterIndex, byte fromLambdaIndex, PointPlurality toPlurality, PointKinds toPointKinds, byte toParameterIndex, byte toLambdaParameterIndex)
		{
			this.FromPlurality = fromPlurality;
			this.FromPointKind = fromPointKinds;
			this.FromParameterIndex = fromParameterIndex;
			this.FromLambdaIndex = fromLambdaIndex;
			this.ToPlurality = toPlurality;
			this.ToPointKind = toPointKinds;
			this.ToParameterIndex = toParameterIndex;
			this.ToLambdaParameterIndex = toLambdaParameterIndex;
		}

		// Properties
		public byte FromLambdaIndex { get; private set; }

		public byte FromParameterIndex { get; private set; }

		public PointPlurality FromPlurality { get; private set; }

		public PointKinds FromPointKind { get; private set; }

		public byte ToLambdaParameterIndex { get; private set; }

		public byte ToParameterIndex { get; private set; }

		public PointPlurality ToPlurality { get; private set; }

		public PointKinds ToPointKind { get; private set; }
	}

#pragma warning restore 1591

	#endregion

	#region IncompleteAttribute

	/// <summary>
	/// Indicates that an item is incomplete,
	/// optionally specifying the name of a larger incomplete sub-system
	/// </summary>
	[Conditional("ANNOTATIONS")]
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	internal sealed class IncompleteAttribute : Attribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly string _Message;

		#endregion

		#region Properties

		/// <summary>
		/// Message describing the nature of the incomplete item
		/// </summary>
		public string Message { get { return _Message; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Indicates that an item is incomplete,
		/// specifying the name of a larger incomplete sub-system
		/// </summary>
		/// <param name="name"></param>
		public IncompleteAttribute(string name)
		{
			_Message = "Incomplete";
			if(!String.IsNullOrEmpty(name))
				_Message += ": " + name;
		}

		/// <summary>
		/// Indicates that an item is incomplete
		/// </summary>
		public IncompleteAttribute() : this(null) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion

	#region ImmutableTypeAttribute

	///<summary>
	/// Indicates that instances of a type cannot be modified in any way
	/// once constructed
	///</summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	[Conditional("ANNOTATIONS")]
	internal sealed class ImmutableTypeAttribute : Attribute
	{

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

	}

	#endregion

	#region UnusedAttribute

	/// <summary>
	/// Indicates that an item is unused (though not necessarily obsolete),
	/// optionally specifying the name of a larger incomplete sub-system
	/// </summary>
	[Conditional("ANNOTATIONS")]
	internal sealed class UnusedAttribute : Attribute
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly string _Message;

		#endregion

		#region Properties

		/// <summary>
		/// Message describing the nature of the unused item
		/// </summary>
		public string Message { get { return _Message; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Indicates that an item is unused (though not necessarily obsolete),
		/// specifying the name of a larger incomplete sub-system
		/// </summary>
		/// <param name="name"></param>
		public UnusedAttribute(string name)
		{
			_Message = "Unused";
			if(!String.IsNullOrEmpty(name))
				_Message += ": " + name;
		}

		/// <summary>
		/// Indicates that an item is unused (though not necessarily obsolete)
		/// </summary>
		public UnusedAttribute() : this(null) { }

		#endregion

		#region Methods

		#endregion

	}

	#endregion


}
