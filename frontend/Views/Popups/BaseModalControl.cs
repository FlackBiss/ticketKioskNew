using Lastik.Views.Popups.PopupContainers;
using System.Windows;
using System.Windows.Controls;

namespace Lastik.Views.Popups
{
    public class BaseModalControl:ContentControl
    {
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(
            name: "Closed",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(BaseModalControl));

        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent(
            name: "Closing",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(BaseModalControl));

        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        public static readonly RoutedEvent OpeningEvent = EventManager.RegisterRoutedEvent(
            name: "Opening",
            routingStrategy: RoutingStrategy.Tunnel,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(BaseModalControl));

        public event RoutedEventHandler Opening
        {
            add { AddHandler(OpeningEvent, value); }
            remove { RemoveHandler(OpeningEvent, value); }
        }
    }
}
