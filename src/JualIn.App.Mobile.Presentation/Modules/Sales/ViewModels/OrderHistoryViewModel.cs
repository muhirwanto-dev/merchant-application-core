using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using SingleScope.Persistence.Specification;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels
{
    public partial class OrderHistoryViewModel : BaseViewModel
    {
        private readonly IReportingService<OrderHistoryViewModel> _reporting;

        public ObservableCollection<OrderDto> Orders { get; } = [];

        public OrderHistoryViewModel(IReportingService<OrderHistoryViewModel> reporting)
        {
            _reporting = reporting;

            RegisterInteractionCommand(nameof(IsNavigating), OpenOrderDetailCommand);

            IsRefreshing = true;
        }

        protected override void FetchData()
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
        private async Task OpenOrderDetailAsync(ProductDto item)
        {
            using var _ = StartScopedNavigation();

            try
            {
                //await _navigation.NavigateToAsync<ProductDetailPage>(new Dictionary<string, object>
                //{
                //    ["product"] = Products.First(x => x.Id == item.Id),
                //});
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        private async Task LoadOrdersAsync()
        {
            var repository = SingleScopeServiceProvider.Current.GetRequiredService<IReadRepository<OrderDto>>();
            var stream = repository.StreamAllAsync(new IncludeSpecification<OrderDto>([x => x.Items, x => x.Transactions]));

            while (IsNavigating)
            {
                await Waiting.Moment;
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
