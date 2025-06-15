using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace Lastik.Views.Components;

public class MiddleAnchoringCanvas:Canvas
{
    protected override Size ArrangeOverride(Size arrangeSize)
    {
        foreach (UIElement child in Children)
        {
            child.Arrange(new Rect(
                new Point(
                    GetLeft(child)-child.DesiredSize.Width/2,
                    GetTop(child) - child.DesiredSize.Height/2),
                child.DesiredSize));
        }

        return arrangeSize;
    }
}