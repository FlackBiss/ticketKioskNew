using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Lastik.Views.Popups.PopupContainers;
using Brush = System.Windows.Media.Brush;

namespace Lastik.Views.Components
{
    public class ModalControl:ContentControl
    {
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen), typeof(bool), typeof(ModalControl), new PropertyMetadata(default(bool),IsOpenChangedCallback));

        public static readonly DependencyProperty OpenCloseDurationProperty =
            DependencyProperty.Register("OpenCloseDuration", typeof(Duration), typeof(ModalControl),
                new PropertyMetadata(Duration.Automatic));


        public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(
            nameof(BackgroundOpacity), typeof(double), typeof(ModalControl), new PropertyMetadata(1.0));

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public Duration OpenCloseDuration
        {
            get { return (Duration)GetValue(OpenCloseDurationProperty); }
            set { SetValue(OpenCloseDurationProperty, value); }
        }
        

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        private bool _isClosed;

        static ModalControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalControl), new FrameworkPropertyMetadata(typeof(ModalControl)));
        }

        public ModalControl()
        {
            Visibility = Visibility.Collapsed;
            AddHandler(BasePopupContainer.ClosingEvent,new RoutedEventHandler(Closing));
        }

        private void Closing(object sender, RoutedEventArgs e)
        {
            CloseAnimated();
            _isClosed = true;
        }

        private static void IsOpenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var modal = (ModalControl)d;
            if (modal.IsOpen)
            {
                modal._isClosed = false;
                modal.OpenAnimated();
            }
            else if(!modal._isClosed)
            {
                modal.CloseAnimated();
                modal._isClosed = true;
            };

        }

        private void OpenAnimated()
        {
            Visibility = Visibility.Visible;
            var openingAnimation = new DoubleAnimation(BackgroundOpacity, OpenCloseDuration);
            Background = new SolidColorBrush
            {
                Color = Colors.Black,
                Opacity = 0
            };
            Background.BeginAnimation(Brush.OpacityProperty, openingAnimation);
        }

        private void CloseAnimated()
        {
            var closingAnimation = new DoubleAnimation(0, OpenCloseDuration);
            closingAnimation.Completed += (sender, args) => Visibility = Visibility.Collapsed;
            Background.BeginAnimation(Brush.OpacityProperty, closingAnimation);
        }


    }
}
