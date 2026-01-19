using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Views;
using JualIn.Domain.Catalogs.Entities;
using JualIn.Domain.Sales.Entities;
using SingleScope.Navigations.Maui.Models;
using SingleScope.Persistence.Abstraction;
using SingleScope.Persistence.Specification;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels
{
    public partial class OrderHistoryViewModel : BaseViewModel
    {
        private readonly IReadRepository<Order> _orderRepository;

        public ObservableCollection<Order> Orders { get; } = [];

        public OrderHistoryViewModel(
            IReadRepository<Order> orderRepository
            )
        {
            _orderRepository = orderRepository;

            RegisterInteractionCommand(nameof(IsNavigating), OpenOrderDetailCommand);

            IsRefreshing = true;
        }

        protected void FetchData()
        {
            Task.Run(async () =>
            {
                try
                {
                    await LoadOrdersAsync();
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

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task OpenOrderDetailAsync(Product item)
        {
            using var _ = StartScopedNavigation();

            try
            {
                await _navigation.NavigateToAsync<ProductDetailPage>(ShellNavigationParams.Create(("product", item)));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        private async Task LoadOrdersAsync()
        {
            var stream = _orderRepository.StreamAllAsync(new IncludeSpecification<Order>([x => x.Items, x => x.Transactions]));

            while (IsNavigating)
            {
                await Task.Yield();
            }

            await MainThread.InvokeOnMainThreadAsync(Orders.Clear);
            await foreach (var entity in stream)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Orders.Add(entity);
                });
            }
        }
    }
}
