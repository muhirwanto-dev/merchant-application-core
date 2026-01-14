using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Mapping;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Views;
using JualIn.Domain.Catalogs.Entities;
using JualIn.SharedLib;
using JualIn.SharedLib.Extensions;
using SingleScope.Navigations.Maui.Models;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels
{
    [QueryProperty(nameof(IsRefreshing), "refresh")]
    public partial class ProductManagementViewModel : BaseViewModel
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SearchSuggestions))]
        private IList<Product> _products = [];

        public ObservableCollection<ProductViewModel> ProductSummaries { get; } = [];

        public IList<string> SearchSuggestions => [.. Products.Select(x => x.Name)];

        public ProductManagementViewModel(
            IMapper mapper,
            IProductRepository productRepository
            )
        {
            _mapper = mapper;
            _productRepository = productRepository;

            RegisterInteractionCommand(nameof(IsNavigating), AddProductCommand);
            RegisterInteractionCommand(nameof(IsNavigating), OpenProductDetailCommand);
            RegisterInteractionCommand(nameof(IsNavigating), OpenScannerCommand);

            IsRefreshing = true;
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
        private void Search(string query)
        {
            try
            {
                var filtered = string.IsNullOrWhiteSpace(query)
                    ? Products
                    : Products.Where(x => x.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase));

                ProductSummaries.Clear();
                ProductSummaries.AddRange(filtered.Select(_mapper.Map<Product, ProductViewModel>));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task AddProductAsync()
        {
            using var _ = StartScopedNavigation();

            try
            {
                await _navigation.NavigateToAsync<ProductFormPage>();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task OpenProductDetailAsync(ProductViewModel item)
        {
            using var _ = StartScopedNavigation();

            try
            {
                await _navigation.NavigateToAsync<ProductDetailPage>(ShellNavigationParams.Create(
                    ("product", _mapper.Map<Product, ProductViewModel>(item.Entity))));
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

        private async Task LoadProductsAsync()
        {
            var productTable = await _productRepository.GetProductsWithComponentAsync();

            while (IsNavigating)
            {
                await Waiting.Moment;
            }

            Products = productTable;

            await MainThread.InvokeOnMainThreadAsync(ProductSummaries.Clear);
            await ProductSummaries.AddRangeOnMainThread(Products.Select(_mapper.Map<Product, ProductViewModel>));
        }
    }
}
