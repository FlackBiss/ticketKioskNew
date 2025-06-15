using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Lastik.Views.Popups.PopupContainers
{
    public class DropUpContainer:BasePopupContainer
    {
        protected override void ContainerLoaded(object sender, RoutedEventArgs e)
        {
            RenderTransform = new TranslateTransform(0, ActualHeight);
            var animation = new DoubleAnimation(0, Duration)
            {
                EasingFunction = EasingFunction
            };
            StartAnimation(animation);
        }

        public override void Close()
        {
            var animation = new DoubleAnimation(ActualHeight, Duration)
            {
                EasingFunction = EasingFunction
            };
            animation.Completed += (o, args) => RaiseEvent(new RoutedEventArgs(ClosedEvent));
            StartAnimation(animation);
            RaiseEvent(new RoutedEventArgs(ClosingEvent));
        }

        protected override void StartAnimation(params Timeline[] animations)
        {
            var sb = new Storyboard();
            sb.Children.Add(animations[0]);
            Storyboard.SetTarget(animations[0], this);
            Storyboard.SetTargetProperty(animations[0], new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
            sb.Begin();
        }
    }
}
