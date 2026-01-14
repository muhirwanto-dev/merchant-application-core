using CommunityToolkit.Mvvm.ComponentModel;
using JualIn.App.Mobile.Presentation.Shared.Filtering;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Filtering
{
    public abstract partial class CountableFilterViewModel(string key) : ObservableObject
    {
        [ObservableProperty]
        private string _key = key;

        [ObservableProperty]
        private bool _showCounter;

        [ObservableProperty]
        private int _counter = 0;

        [ObservableProperty]
        private bool _isSelected = false;
    }

    public class CountableFilterViewModel<T>(Func<T, bool> predicate, string key) : CountableFilterViewModel(key)
        , IFilterState<T>
    {
        public IFilter<T> Model { get; } = new CountableFilter<T>(predicate, key);

        void IFilterState<T>.UpdateState(IEnumerable<T> filtered)
        {
            Counter = filtered.Count();
        }
    }
}
