using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;

namespace JualIn.SharedLib.Extensions
{
    public static class CollectionExtensions
    {
        extension<T>(ObservableCollection<T> collection)
        {
            public void AddRange(IEnumerable<T> items)
            {
                Guard.IsNotNull(collection);
                Guard.IsNotNull(items);

                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }

            public void RemoveRange(IEnumerable<T> items)
            {
                Guard.IsNotNull(collection);
                Guard.IsNotNull(items);

                foreach (var item in items)
                {
                    collection.Remove(item);
                }
            }
        }

        extension<T>(IList<T> list)
        {
            public void AddRange(IEnumerable<T> items)
            {
                Guard.IsNotNull(list);
                Guard.IsNotNull(items);

                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        extension<T>(IEnumerable<T> source)
        {
            public void Do(Action<T> action)
            {
                foreach (var s in source)
                {
                    action(s);
                }
            }
        }
    }
}
