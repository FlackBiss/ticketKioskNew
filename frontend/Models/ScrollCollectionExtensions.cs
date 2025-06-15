using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Schedules.Stores;
using Lastik.Utilities;

namespace Lastik.Models;

public static class ScrollCollectionExtensions
{
    public static bool CheckScrollOverflow(this ScrollChangedEventArgs args)
        => args.ExtentHeightChange <= 0 &&
           args.VerticalOffset != 0 &&
           args.ExtentHeight - args.VerticalOffset - args.ViewportHeight <= 100;

    public static void LoadNextToObservable<T>(
        this ObservableCollection<T> observable,
        ICollection<T> items,int count)
        
    {
        foreach (var model in items
                           .GetNextChunk(observable.Count, count))
        {
            observable.Add(model);
        }
    }
}