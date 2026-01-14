using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using JualIn.App.Mobile.Presentation.Core.ViewModels;

namespace JualIn.App.Mobile.Presentation.Shared.Filtering
{
    public partial class FilterGroupState<TItem> : BaseViewModel
        where TItem : class
    {
        private readonly Lock _lock = new();

        public ICommand SelectedFilterChangeCommand => new Command<string>(RunApplyFilter);

        public IEnumerable<IFilterState<TItem>> FilterStates { get; }

        public IEnumerable<TItem> UnfilteredItems { get; private set; } = [];

        public ObservableCollection<TItem> FilteredItems { get; } = [];

        public bool IsFilterApplied => !_isWaitingForFilterApplied;

        private bool _isWaitingForFilterApplied = false;

        private IFilter<TItem>? _activeFilter = null;

        public FilterGroupState(IEnumerable<IFilterState<TItem>> filterStates)
        {
            Guard.IsGreaterThan(filterStates.Count(), 0, nameof(filterStates));

            _activeFilter = filterStates.FirstOrDefault()?.Model;

            FilterStates = filterStates;
        }

        public void SetUnfiltered(IEnumerable<TItem> items)
        {
            UnfilteredItems = items;

            foreach (var state in FilterStates)
            {
                var filtered = state.Model.Apply(UnfilteredItems);
                state.UpdateState(filtered);
            }
        }

        public void RunApplyFilter()
        {
            if (_activeFilter != null)
            {
                RunApplyFilter(_activeFilter.Key);
            }
        }

        public void RunApplyFilter(string key)
        {
            Task.Run(async () => await ApplyFilterAsync(key));
        }

        public async Task ApplyFilterAsync(string key)
        {
            try
            {
                _activeFilter = FilterStates.First(x => x.Model?.Key == key).Model;

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    lock (_lock)
                    {
                        FilteredItems.Clear();
                    }
                });

                foreach (var item in _activeFilter.Apply(UnfilteredItems))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        lock (_lock)
                        {
                            FilteredItems.Add(item);
                        }
                    });
                }

                // Guard from wrong UI states
                if (FilteredItems.FirstOrDefault() is TItem first)
                {
                    Guard.IsTrue(UnfilteredItems.Any(x => object.ReferenceEquals(x, first)));
                }
            }
            finally
            {
                _isWaitingForFilterApplied = false;
            }
        }

        public void WaitForFilterApplied()
        {
            _isWaitingForFilterApplied = true;
        }
    }
}
