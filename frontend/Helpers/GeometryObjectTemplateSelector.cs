using Lastik.Models.Geometry.Entities;
using System.Windows;
using System.Windows.Controls;

namespace Lastik.Helpers
{
    public class GeometryObjectTemplateSelector:DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = (FrameworkElement)container;
            var geometryObject = (GeometryObject)item;
            if (geometryObject.Text is null)
            {
                return (DataTemplate)element.FindResource("PathGeometryTemplate");
            }

            if (geometryObject.Path is null)
            {
                return (DataTemplate)element.FindResource("TextGeometryTemplate");
            }

            return (DataTemplate)element.FindResource("TextOnAPathGeometryTemplate");

        }
    }
}
