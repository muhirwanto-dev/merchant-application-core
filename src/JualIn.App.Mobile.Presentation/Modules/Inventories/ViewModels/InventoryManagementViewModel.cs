using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Mapping;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Models;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Views;
using JualIn.App.Mobile.Presentation.Shared.Filtering;
using JualIn.App.Mobile.Presentation.Shared.Persistence;
using JualIn.App.Mobile.Presentation.UI.Controls.Filtering;
using JualIn.Domain.Inventories.Entities;
using JualIn.SharedLib;
using SingleScope.Navigations.Maui.Models;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels
{
    [QueryProperty(nameof(IsRefreshing), "refresh")]
    public partial class InventoryManagementViewModel : BaseViewModel
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISearchable<Inventory> _searchable;

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private IList<InventoryViewModel> _inventories = [];

        [ObservableProperty]
        private IList<string> _searchSuggestions = [];

        public FilterGroupState<InventoryViewModel> FilterGroup { get; }

        public InventoryManagementViewModel(
            IMapper mapper
            )
        {
            _mapper = mapper;

            IsRefreshing = true;
            FilterGroup = new([
                new CountableFilterViewModel<InventoryViewModel>(x => true, InventoryFilter.AllItems) { ShowCounter = true },
                new CountableFilterViewModel<InventoryViewModel>(x => x.IsLowStock, InventoryFilter.LowStock) { ShowCounter = true },
                new CountableFilterViewModel<InventoryViewModel>(x => x.IsExpired, InventoryFilter.Expired) { ShowCounter = true },
            ]);

            RegisterInteractionCommand(nameof(IsNavigating), OpenAddInventoryCommand);
        }

        protected override void Broadcast<T>(T oldValue, T newValue, string? propertyName)
        {
            base.Broadcast(oldValue, newValue, propertyName);

            try
            {
                if (propertyName == nameof(Inventories))
                {
                    SearchSuggestions = [.. Inventories.Select(x => x.Entity.Name)];
                    FilterGroup.SetUnfiltered(Inventories);
                    FilterGroup.RunApplyFilter();
                }
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void Appearing()
        {
            try
            {
                Navigating();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void Disappearing()
        {
            try
            {
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void NavigatedTo()
        {
            try
            {
                Navigated();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task OpenAddInventoryAsync()
        {
            using var _ = StartScopedNavigation();

            try
            {
                await _navigation.NavigateToAsync<InventoryFormPage>();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task OpenInventoryDetailAsync(InventoryViewModel item)
        {
            using var _ = StartScopedNavigation();

            try
            {
                await _navigation.NavigateToAsync<InventoryDetailPage>(ShellNavigationParams.Create(
                    ("inventory", Inventories.First(x => x.Entity.Id == item.Entity.Id))));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private void OpenScanner()
        {
            using var _ = StartScopedNavigation();

            try
            {
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task SearchAsync(string query)
        {
            try
            {
                var inventoryTable = await _searchable.SearchAsync(query);

                FilterGroup.WaitForFilterApplied();
                Inventories = [.. inventoryTable.Select(_mapper.Map<Inventory, InventoryViewModel>)];

                while (!FilterGroup.IsFilterApplied)
                {
                    await Waiting.Moment;
                }
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void Refresh()
        {
            FetchData();
        }

        protected void FetchData()
        {
            Task.Run(async () =>
            {
                try
                {
                    await LoadInventoriesAsync();
                }
                catch (Exception ex)
                {
                    _reporting.ReportProblems(ex);
                }
                finally
                {
                    await MainThread.InvokeOnMainThreadAsync(() => IsRefreshing = false);
                }
            });
        }

        private async Task LoadInventoriesAsync()
        {
            var inventoryTable = await _inventoryRepository.GetAllAsync();

            while (IsNavigating)
            {
                await Waiting.Moment;
            }

            FilterGroup.WaitForFilterApplied();
            Inventories = [.. inventoryTable.Select(_mapper.Map<Inventory, InventoryViewModel>)];

            while (!FilterGroup.IsFilterApplied)
            {
                await Waiting.Moment;
            }
        }
    }
}
