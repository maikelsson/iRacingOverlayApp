using iRacingSimulator;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace iRacingLiveDataOverlay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Sim.Instance.Stop();
        }
    }

    public partial class CustomEvents : UserControl
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
