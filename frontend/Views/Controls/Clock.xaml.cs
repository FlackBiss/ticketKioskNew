using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace Lastik.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        public Clock()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty RemainingTimeProperty = DependencyProperty.Register(
            nameof(RemainingTime), typeof(TimeSpan), typeof(Clock), new PropertyMetadata(default(TimeSpan),RemainingTimeChanged));

        
        public TimeSpan RemainingTime
        {
            get { return (TimeSpan)GetValue(RemainingTimeProperty); }
            set { SetValue(RemainingTimeProperty, value); }
        }

        private static void RemainingTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not Clock clock) return;
            if(e.NewValue is not TimeSpan time) return;

            var decadeMinutes = time.Minutes / 10;
            var minutes = time.Minutes%10;
            var decadeSeconds = time.Seconds / 10;
            var seconds = time.Seconds%10;
            clock.DecadeMinutes.Text = decadeMinutes.ToString();
            clock.Minutes.Text = minutes.ToString();
            clock.DecadeSeconds.Text = decadeSeconds.ToString();
            clock.Seconds.Text = seconds.ToString();
        }
    }
}
