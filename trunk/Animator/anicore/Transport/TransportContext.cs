using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Animator.Core.Transport
{

	#region TransportContext

	public sealed class TransportContext : DependencyObject
	{

		#region Static / Constant

		public static readonly DependencyProperty StateProperty;
		public static readonly DependencyProperty PositionProperty;
		public static readonly DependencyProperty BarLengthProperty;

		//public static readonly RoutedEvent StateChangedEvent;
		//public static readonly RoutedEvent PositionChangedEvent;

		static TransportContext()
		{
			StateProperty = DependencyProperty.Register("State", typeof(TransportState), typeof(TransportContext),
				new PropertyMetadata(TransportState.Stopped, OnStateChanged));
			PositionProperty = DependencyProperty.Register("Position", typeof(TransportTime), typeof(TransportContext),
				new PropertyMetadata(TransportTime.Zero, OnPositionChanged));

			BarLengthProperty = DependencyProperty.RegisterAttached("BarLength", typeof(int), typeof(TransportContext),
																	new PropertyMetadata(4), value => (int)value > 0);

			//StateChangedEvent = EventManager.RegisterRoutedEvent("StateChanged", RoutingStrategy.Tunnel, typeof(TransportEventHandler), typeof(TransportContext));
			//PositionChangedEvent = EventManager.RegisterRoutedEvent("PositionChanged", RoutingStrategy.Tunnel, typeof (TransportEventHandler), typeof (TransportContext));
		}

		private static void OnStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var ctx = obj as TransportContext;
			if(ctx == null)
				return;
			throw new NotImplementedException();
		}

		private static void OnPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var ctx = obj as TransportContext;
			if(ctx == null)
				return;
			throw new NotImplementedException();
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		public TransportState State
		{
			get { return (TransportState)this.GetValue(StateProperty); }
			set { this.SetValue(StateProperty, value); }
		}

		public TransportTime Position
		{
			get { return (TransportTime)this.GetValue(PositionProperty); }
			set { this.SetValue(PositionProperty, value); }
		}

		public int BarLength
		{
			get { return (int)this.GetValue(BarLengthProperty); }
			set { this.SetValue(BarLengthProperty, value); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion

	}

	#endregion

}
