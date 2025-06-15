using Lastik.Models.Geometry.Entities;
using System.Windows;
using System.Windows.Controls;
using Lastik.Models.Schedules.Entities;
using Lastik.ViewModels.HallGeometry;

namespace Lastik.Helpers;

public class ChoosePlaceViewSelector:DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is null) return base.SelectTemplate(item, container);
        var element = (FrameworkElement)container;
        var vm = (ChoosePlaceViewModel)item;

        if (vm.Schedule.IsSeasonHead) return 
            (DataTemplate)element.FindResource("AbonementChoosePlaceTemplate");

        return (DataTemplate)element.FindResource("EventChoosePlaceTemplate");
    }
}