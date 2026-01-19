using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Mapping;
using JualIn.App.Mobile.Presentation.Modules.Sales.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.App.Mobile.Presentation.Shared.Filtering;
using JualIn.App.Mobile.Presentation.Shared.Persistence;
using JualIn.App.Mobile.Presentation.UI.Controls.Filtering;
using JualIn.Domain.Catalogs.Entities;
using SingleScope.Navigations.Maui.Models;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels
{
    [QueryProperty(nameof(IsRefreshing), "refresh")]
    public partial class PointOfSaleViewModel : BaseViewModel
    {
        private static readonly CountableFilterViewModel<SaleProductViewModel> _defaultFilter = new(x => true, AppStrings.Common_Filter_All);

        private readonly IMapper _mapper;
        private readonly IReadRepository<Product> _productRepository;
        private readonly ISearchable<Product> _searchable;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SearchSuggestions))]
        [NotifyPropertyChangedRecipients]
        private IEnumerable<SaleProductViewModel> _saleProducts = [];

        [ObservableProperty]
        private FilterGroupState<SaleProductViewModel> _filterGroup;

        public IEnumerable<string> SearchSuggestions => [.. SaleProducts.Select(x => x.Entity.Name)];

        public double OnCartTotalPrice => FilterGroup.FilteredItems.Where(x => x.IsSelected).Sum(x => x.OnCartTotalPrice);

        public double OnCartTotalQuantity => FilterGroup.FilteredItems.Where(x => x.IsSelected).Sum(x => x.OnCartQuantity);

        public bool HasProductSelected => FilterGroup.FilteredItems.Any(x => x.IsSelected);

        public PointOfSaleViewModel(
            IMapper mapper,
            IReadRepository<Product> productRepository,
            ISearchable<Product> searchable
            )
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _searchable = searchable;

            FilterGroup = new([_defaultFilter]);
            IsRefreshing = true;
        }

        protected override void Broadcast<T>(T oldValue, T newValue, string? propertyName)
        {
            base.Broadcast(oldValue, newValue, propertyName);

            try
            {
                if (propertyName == nameof(SaleProducts))
                {
                    FilterGroup.SetUnfiltered(SaleProducts);
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

                if (IsRefreshing)
                {
                    FetchData();
                }
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

        [RelayCommand]
        private async Task SearchAsync(string query)
        {
            try
            {
                var entities = await _searchable.SearchAsync(query);

                SaleProducts = [.. entities.Select(_mapper.Map<Product, SaleProductViewModel>)];
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void SelectProduct(SaleProductViewModel item)
        {
            try
            {
                if (item.IsSelected)
                {
                    return;
                }

                AddItem(item);
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void SubtractItem(SaleProductViewModel item)
        {
            try
            {
                item.OnCartQuantity--;

                OnPropertyChanged(nameof(HasProductSelected));
                OnPropertyChanged(nameof(OnCartTotalQuantity));
                OnPropertyChanged(nameof(OnCartTotalPrice));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void AddItem(SaleProductViewModel item)
        {
            try
            {
                if (item.OnCartQuantity >= item.Entity.Stock.Value)
                {
                    Toast.Make(AppStrings.PointOfSalePage_Msg_ItemStockLimit).Show();

                    return;
                }

                item.OnCartQuantity++;

                OnPropertyChanged(nameof(HasProductSelected));
                OnPropertyChanged(nameof(OnCartTotalQuantity));
                OnPropertyChanged(nameof(OnCartTotalPrice));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task CheckoutAsync()
        {
            try
            {
                var selected = SaleProducts.Where(x => x.IsSelected);

                await _navigation.NavigateToAsync<CheckoutPage>(ShellNavigationParams.Create(("items", selected)));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        protected void FetchData()
        {
            Task.Run(async () =>
            {
                try
                {
                    await LoadProductsAsync();
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

        private async Task LoadProductsAsync()
        {
            var productTable = await _productRepository.GetAllAsync();

            while (IsNavigating)
            {
                await Task.Yield();
            }

            var saleProducts = productTable.Select(_mapper.Map<Product, SaleProductViewModel>);
            var categories = saleProducts.Select(x => x.Entity.Category).Distinct();

            FilterGroup = new([
                _defaultFilter,
                .. categories.Select(x => new CountableFilterViewModel<SaleProductViewModel>(
                    y => y.Entity.Category == x, x))
                ]);
            FilterGroup.WaitForFilterApplied();
            SaleProducts = [.. saleProducts];

            while (!FilterGroup.IsFilterApplied)
            {
                await Task.Yield();
            }

            OnPropertyChanged(nameof(HasProductSelected));
            OnPropertyChanged(nameof(OnCartTotalQuantity));
            OnPropertyChanged(nameof(OnCartTotalPrice));
        }
    }
}
