using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using VVVV.Hosting.Pins;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;

namespace XamlNodes.Core.Pins
{

	#region XamlNodePin

	[RuntimeNameProperty("Name")]
	public abstract class XamlNodePin : DependencyObject, IDisposable
	{

		#region Static / Constant

		#endregion

		#region Fields

		private bool _HasDefaultColor;
		private double _DefaultColorRed;
		private double _DefaultColorGreen;
		private double _DefaultColorBlue;
		private double _DefaultColorAlpha;

		#endregion

		#region Properties

		public string Name { get; set; }

		public string PinName { get; set; }

		internal string ActualPinName
		{
			get { return String.IsNullOrEmpty(this.PinName) ? this.Name : this.PinName; }
		}

		public bool AsInt { get; set; }

		public double DefaultColorRed
		{
			get { return _DefaultColorRed; }
			set
			{
				_DefaultColorRed = value;
				_HasDefaultColor = true;
			}
		}

		public double DefaultColorGreen
		{
			get { return _DefaultColorGreen; }
			set
			{
				_DefaultColorGreen = value;
				_HasDefaultColor = true;
			}
		}

		public double DefaultColorBlue
		{
			get { return _DefaultColorBlue; }
			set
			{
				_DefaultColorBlue = value;
				_HasDefaultColor = true;
			}
		}

		public double DefaultColorAlpha
		{
			get { return _DefaultColorAlpha; }
			set
			{
				_DefaultColorAlpha = value;
				_HasDefaultColor = true;
			}
		}

		private double[] DefaultColorParts
		{
			get
			{
				//if(!_HasDefaultColor)
				//    return null;
				return new[] { _DefaultColorRed, _DefaultColorGreen, _DefaultColorBlue, _DefaultColorAlpha };
			}
		}

		public string DefaultEnumEntry { get; set; }

		public string DefaultString { get; set; }

		public double DefaultValue { get; set; }

		public List<double> DefaultValues { get; set; }

		public List<string> DimensionNames { get; set; }

		public string EnumName { get; set; }

		public string FileMask { get; set; }

		public bool HasAlpha { get; set; }

		public bool IsBang { get; set; }

		public bool IsPinGroup { get; set; }

		public bool IsSingle { get; set; }

		public bool Lazy { get; set; }

		public int MaxChars { get; set; }

		public double MinValue { get; set; }

		public double MaxValue { get; set; }

		public int Order { get; set; }

		public double StepSize { get; set; }

		public StringType StringType { get; set; }

		public PinVisibility Visibility { get; set; }

		public XamlNodePinDataType DataType { get; set; }

		public Type CustomDataType { get; set; }

		internal Type ActualDataType
		{
			get
			{
				switch(this.DataType)
				{
				case XamlNodePinDataType.Value:
					if(this.CustomDataType != null)
						return this.CustomDataType;
					return this.AsInt ? typeof(int) : typeof(double);
				case XamlNodePinDataType.String:
					return typeof(string);
				case XamlNodePinDataType.Color:
					return typeof(RGBAColor);
				//case XamlNodePinDataType.Custom:
				default:
					if(this.CustomDataType == null)
						throw new InvalidOperationException("CustomDataType not provided");
					return this.CustomDataType;
				}
			}
		}

		internal IPluginIO InternalIO { get; set; }

		#endregion

		#region Constructors

		protected XamlNodePin()
		{
			this.FileMask = "All Files (*.*)|*.*";
			this.Visibility = PinVisibility.True;
			this.MaxChars = -1;
			//this.DefaultString = String.Empty;
			this.MinValue = PinAttribute.DefaultMinValue;
			this.MaxValue = PinAttribute.DefaultMaxValue;
			this.StepSize = PinAttribute.DefaultStepSize;
			//this.DefaultValues = new List<double> { 0, 0, 0, 0 };
			this.HasAlpha = true;
			////defaultcolor...?
			this.EnumName = "Empty";
		}

		#endregion

		#region Methods

		internal abstract PinAttribute CreatePinAttribute();

		protected PinAttribute InitPinAttribute(PinAttribute attr)
		{
			attr.AsInt = this.AsInt;
			if(_HasDefaultColor)
				attr.DefaultColor = this.DefaultColorParts;
			attr.DefaultEnumEntry = this.DefaultEnumEntry;
			attr.DefaultString = this.DefaultString;
			attr.DefaultValue = this.DefaultValue;
			attr.DefaultValues = this.DefaultValues.ToArrayOrNull();
			attr.DimensionNames = this.DimensionNames.ToArrayOrNull();
			attr.EnumName = this.EnumName;
			attr.FileMask = this.FileMask;
			attr.HasAlpha = this.HasAlpha;
			attr.IsBang = this.IsBang;
			attr.IsPinGroup = this.IsPinGroup;
			attr.IsSingle = this.IsSingle;
			attr.Order = this.Order;
			attr.StepSize = this.StepSize;
			attr.StringType = this.StringType;
			attr.Visibility = this.Visibility;
			return attr;
		}

		internal object CreatePin(IPluginHost host)
		{
			return PinFactory.CreatePin(host, this.CreatePinAttribute(), this.ActualDataType);
		}

		internal abstract void AttachHost(IXamlNodeHost host);

		#endregion

		#region IDisposable Members

		protected abstract void Dispose(bool disposing);

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion


	#region XamlNodePin<T>

	public abstract class XamlNodePin<T> : DependencyObject, IDisposable
	{

		#region Static/Constant

		#endregion

		#region Fields

		private bool _HasDefaultColor;
		private double _DefaultColorRed;
		private double _DefaultColorGreen;
		private double _DefaultColorBlue;
		private double _DefaultColorAlpha;

		#endregion

		#region Properties

		public string Name { get; set; }

		public string PinName { get; set; }

		internal string ActualPinName
		{
			get { return String.IsNullOrEmpty(this.PinName) ? this.Name : this.PinName; }
		}

		public bool AsInt { get; set; }

		public double DefaultColorRed
		{
			get { return _DefaultColorRed; }
			set
			{
				_DefaultColorRed = value;
				_HasDefaultColor = true;
			}
		}

		public double DefaultColorGreen
		{
			get { return _DefaultColorGreen; }
			set
			{
				_DefaultColorGreen = value;
				_HasDefaultColor = true;
			}
		}

		public double DefaultColorBlue
		{
			get { return _DefaultColorBlue; }
			set
			{
				_DefaultColorBlue = value;
				_HasDefaultColor = true;
			}
		}

		public double DefaultColorAlpha
		{
			get { return _DefaultColorAlpha; }
			set
			{
				_DefaultColorAlpha = value;
				_HasDefaultColor = true;
			}
		}

		private double[] DefaultColorParts
		{
			get
			{
				//if(!_HasDefaultColor)
				//    return null;
				return new[] { _DefaultColorRed, _DefaultColorGreen, _DefaultColorBlue, _DefaultColorAlpha };
			}
		}

		public string DefaultEnumEntry { get; set; }

		public string DefaultString { get; set; }

		public double DefaultValue { get; set; }

		public List<double> DefaultValues { get; set; }

		public List<string> DimensionNames { get; set; }

		public string EnumName { get; set; }

		public string FileMask { get; set; }

		public bool HasAlpha { get; set; }

		public bool IsBang { get; set; }

		public bool IsPinGroup { get; set; }

		public bool IsSingle { get; set; }

		public bool Lazy { get; set; }

		public int MaxChars { get; set; }

		public double MinValue { get; set; }

		public double MaxValue { get; set; }

		public int Order { get; set; }

		public double StepSize { get; set; }

		public StringType StringType { get; set; }

		public PinVisibility Visibility { get; set; }

		internal Pin<T> Pin { get; private set; }

		#endregion

		#region Constructors

		protected XamlNodePin()
		{
			this.FileMask = "All Files (*.*)|*.*";
			this.Visibility = PinVisibility.True;
			this.MaxChars = -1;
			//this.DefaultString = String.Empty;
			this.MinValue = PinAttribute.DefaultMinValue;
			this.MaxValue = PinAttribute.DefaultMaxValue;
			this.StepSize = PinAttribute.DefaultStepSize;
			//this.DefaultValues = new List<double> { 0, 0, 0, 0 };
			this.HasAlpha = true;
			////defaultcolor...?
			this.EnumName = "Empty";
		}

		#endregion

		#region Methods

		internal abstract PinAttribute CreatePinAttribute();

		protected PinAttribute InitPinAttribute(PinAttribute attr)
		{
			attr.AsInt = this.AsInt;
			if(_HasDefaultColor)
				attr.DefaultColor = this.DefaultColorParts;
			attr.DefaultEnumEntry = this.DefaultEnumEntry;
			attr.DefaultString = this.DefaultString;
			attr.DefaultValue = this.DefaultValue;
			attr.DefaultValues = this.DefaultValues.ToArrayOrNull();
			attr.DimensionNames = this.DimensionNames.ToArrayOrNull();
			attr.EnumName = this.EnumName;
			attr.FileMask = this.FileMask;
			attr.HasAlpha = this.HasAlpha;
			attr.IsBang = this.IsBang;
			attr.IsPinGroup = this.IsPinGroup;
			attr.IsSingle = this.IsSingle;
			attr.Order = this.Order;
			attr.StepSize = this.StepSize;
			attr.StringType = this.StringType;
			attr.Visibility = this.Visibility;
			return attr;
		}

		internal virtual void AttachHost(IXamlNodeHost host)
		{
			
		}

		protected abstract Pin<T> CreatePin(IPluginHost host);

		#endregion

		#region IDisposable Members

		protected virtual void Dispose(bool disposing)
		{

		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
