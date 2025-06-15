using System.Windows;
using System.Windows.Media.Animation;
using Microsoft.Xaml.Behaviors;
using Size = System.Windows.Size;

namespace Lastik.Helpers;

class AnimatedSizeChangingBehavior : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty AnimateWidthProperty = DependencyProperty.Register(
        nameof(AnimateWidth), typeof(bool), typeof(AnimatedSizeChangingBehavior), new PropertyMetadata(default(bool)));

    public bool AnimateWidth
    {
        get => (bool) GetValue(AnimateWidthProperty);
        set => SetValue(AnimateWidthProperty, value);
    }

    public static readonly DependencyProperty AnimateHeightProperty = DependencyProperty.Register(
        nameof(AnimateHeight), typeof(bool), typeof(AnimatedSizeChangingBehavior), new PropertyMetadata(default(bool)));

    public bool AnimateHeight
    {
        get => (bool) GetValue(AnimateHeightProperty);
        set => SetValue(AnimateHeightProperty, value);
    }

    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        nameof(Duration), typeof(Duration), typeof(AnimatedSizeChangingBehavior), new PropertyMetadata(default(Duration)));

    public Duration Duration
    {
        get => (Duration) GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    private bool _isAnimating;

    protected override void OnAttached()
    {
        AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
        AssociatedObject.Unloaded += AssociatedObjectUnloaded;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
    }

    private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
    {
        AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
        AssociatedObject.Unloaded -= AssociatedObjectUnloaded;
    }

    private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_isAnimating || !(AnimateWidth || AnimateHeight))
            return;

        AnimatedSizeChanging(e.PreviousSize, e.NewSize);
    }

    private void AnimatedSizeChanging(Size prevSize, Size newSize)
    {
        var storyboard = new Storyboard();

        if (AnimateWidth)
        {
            var widthAnimation = GetSizeAnimation(FrameworkElement.WidthProperty, prevSize.Width, newSize.Width);
            storyboard.Children.Add(widthAnimation);
        }

        if (AnimateHeight)
        {
            var heightAnimation = GetSizeAnimation(FrameworkElement.HeightProperty, prevSize.Height, newSize.Height);
            storyboard.Children.Add(heightAnimation);
        }

        storyboard.Completed += (_, _) =>
        {
            if (AnimateWidth)
                ResetProperty(FrameworkElement.WidthProperty);

            if (AnimateHeight)
                ResetProperty(FrameworkElement.HeightProperty);

            _isAnimating = false;
        };

        _isAnimating = true;
        storyboard.Begin(AssociatedObject);
    }

    private void ResetProperty(DependencyProperty property)
    {
        AssociatedObject.ApplyAnimationClock(property, null);
        AssociatedObject.SetValue(property, double.NaN);
    }

    private DoubleAnimation GetSizeAnimation(DependencyProperty property, double? from = null, double? to = null)
    {
        var sizeAnimation = GetDoubleAnimation(from, to);

        Storyboard.SetTarget(sizeAnimation, AssociatedObject);
        Storyboard.SetTargetProperty(sizeAnimation, new PropertyPath(property));

        return sizeAnimation;
    }

    private DoubleAnimation GetDoubleAnimation(double? from = null, double? to = null)
    {
        return new DoubleAnimation
        {
            From = from,
            To = to,
            Duration = Duration,
            EasingFunction = new PowerEase()
        };
    }
}