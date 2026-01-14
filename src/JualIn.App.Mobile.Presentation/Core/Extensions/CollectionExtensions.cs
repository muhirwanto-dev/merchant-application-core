using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;

namespace JualIn.App.Mobile.Presentation.Core.Extensions
{
    public static class CollectionExtensions
    {
        extension<T>(ObservableCollection<T> collection)
        {
            public async Task AddRangeOnMainThread(IEnumerable<T> items)
            {
                Guard.IsNotNull(collection);
                Guard.IsNotNull(items);

                foreach (var item in items)
                {
                    await MainThread.InvokeOnMainThreadAsync(() => collection.Add(item));
                }
            }

            public async Task RemoveRangeOnMainThread(IEnumerable<T> items)
            {
                Guard.IsNotNull(collection);
                Guard.IsNotNull(items);

                foreach (var item in items)
                {
                    await MainThread.InvokeOnMainThreadAsync(() => collection.Remove(item));
                }
            }
        }
    }
}
