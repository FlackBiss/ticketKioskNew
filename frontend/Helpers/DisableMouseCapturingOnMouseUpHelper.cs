using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lastik.Helpers
{
    public class DisableMouseCapturingOnMouseUpHelper
    {
        public static readonly DependencyProperty HandleProperty = DependencyProperty.RegisterAttached(
            "Handle", typeof(bool), typeof(DisableMouseCapturingOnMouseUpHelper), new PropertyMetadata(default(bool),
            HandlePropertyChangedCallback));

        public static void SetHandle(DependencyObject element, bool value)=>element.SetValue(HandleProperty, value);

        public static bool GetHandle(DependencyObject element) =>(bool)element.GetValue(HandleProperty);
    

        private static void HandlePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement element ||
                e.OldValue is not bool oldValue ||
                e.NewValue is not bool newValue)
                return;

            switch (oldValue)
            {
                case true when !newValue:
                    element.PreviewMouseUp -= Element_MouseUp;
                    break;
                case false when newValue:
                    element.PreviewMouseUp += Element_MouseUp;
                    break;
            }
        }

        private static void Element_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Mouse.Captured is Calendar 
                || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
                Mouse.Capture(null);
        }
    }
}
