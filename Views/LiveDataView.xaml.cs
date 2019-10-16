using System;
using System.Windows;
using System.Windows.Controls;

namespace iRacingLiveDataOverlay.Views
{
    /// <summary>
    /// Interaction logic for LiveDataView.xaml
    /// </summary>
    public partial class LiveDataView : Window
    {
        public LiveDataView()
        {
            InitializeComponent();
        }

    }

    public partial class CustomEvents : Control
    {
        // Create a custom routed event by first registering a RoutedEventID 
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent CustomEvent = EventManager.RegisterRoutedEvent("PositionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomEvents));

        // Provide CLR accessors for the event 
        public event RoutedEventHandler CustomTest
        {
            add { AddHandler(CustomEvent, value); }
            remove { RemoveHandler(CustomEvent, value); }
        }

        private void AddHandler(RoutedEvent customEvent, RoutedEventHandler value)
        {
            throw new NotImplementedException();
        }

        public void RaiseMyEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(CustomEvent);
            RaiseEvent(newEventArgs);
        }

    }
}
