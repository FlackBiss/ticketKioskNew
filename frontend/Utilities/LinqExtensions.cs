using System.Collections;
using System.Windows;

namespace Lastik.Utilities;

public static class LinqExtensions
{
    public static List<T> GetNextChunk<T>(this ICollection<T> source, int skipCount, int takeCount)
        => source
            .Skip(skipCount)
            .Take(takeCount)
            .ToList();

    public static IEnumerable<T> ApplyFilters<T>(this IEnumerable<T> source, IEnumerable<Func<T, bool>> filters) =>
        filters.Aggregate(source, (current, filter) => current.Where(filter));

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source) action(item);
    }

    public static void ForEach<T>(this IList source, Action<T> action)
    {
        foreach (var item in source) action((T)item);
    }

}