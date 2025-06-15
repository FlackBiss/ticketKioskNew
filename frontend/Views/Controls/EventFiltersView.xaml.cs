using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace Lastik.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для EventFiltersView.xaml
    /// </summary>
    public partial class EventFiltersView : UserControl
    {
        public static readonly DependencyProperty OpenCalendarCommandProperty = DependencyProperty.Register(nameof(OpenCalendarCommand), typeof(ICommand), typeof(EventFiltersView), new PropertyMetadata(default(ICommand)));
        public static readonly DependencyProperty OpenFilterCommandProperty = DependencyProperty.Register(nameof(OpenFilterCommand), typeof(ICommand), typeof(EventFiltersView), new PropertyMetadata(default(ICommand)));
        public static readonly DependencyProperty HasDaysProperty = DependencyProperty.Register(nameof(HasDays), typeof(bool), typeof(EventFiltersView), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty DaySelectedCommandProperty = DependencyProperty.Register(nameof(DaySelectedCommand), typeof(ICommand), typeof(EventFiltersView), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty EventDaysProperty = DependencyProperty.Register(
            nameof(EventDays), typeof(List<DateTime>), typeof(EventFiltersView), new PropertyMetadata(default(List<DateTime>),EventDaysChanged));

        private static void EventDaysChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        =>((EventFiltersView)d).HasDays = ((List<DateTime>)e.NewValue).Count > 0;

        public static readonly DependencyProperty SelectedEventDayProperty = DependencyProperty.Register(
            nameof(SelectedEventDay), typeof(DateTime?), typeof(EventFiltersView), new FrameworkPropertyMetadata(default(DateTime?),FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public List<DateTime> EventDays
        {
            get { return (List<DateTime>)GetValue(EventDaysProperty); }
            set { SetValue(EventDaysProperty, value); }
        }

        public DateTime? SelectedEventDay
        {
            get { return (DateTime?)GetValue(SelectedEventDayProperty); }
            set { SetValue(SelectedEventDayProperty, value); }
        }

        public ICommand DaySelectedCommand
        {
            get => (ICommand)GetValue(DaySelectedCommandProperty);
            set => SetValue(DaySelectedCommandProperty, value);
        }

        public bool HasDays
        {
            get=>(bool) GetValue(HasDaysProperty);
            private set =>SetValue(HasDaysProperty, value);
        }

        public ICommand OpenCalendarCommand
        {
            get => (ICommand)GetValue(OpenCalendarCommandProperty);
            set => SetValue(OpenCalendarCommandProperty, value);
        }

        public ICommand OpenFilterCommand
        {
            get => (ICommand)GetValue(OpenFilterCommandProperty);
            set => SetValue(OpenFilterCommandProperty, value);
        }

        public EventFiltersView()
        {
            InitializeComponent();
        }
    }
}
