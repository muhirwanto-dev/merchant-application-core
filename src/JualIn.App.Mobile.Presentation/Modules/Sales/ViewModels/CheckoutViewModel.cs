using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HorusStudio.Maui.MaterialDesignControls;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Mapping;
using JualIn.App.Mobile.Presentation.Modules.Payments.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Payments.Factories;
using JualIn.App.Mobile.Presentation.Modules.Sales.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Sales.Messages;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Payments.ValueObjects;
using JualIn.Domain.Sales.Entities;
using JualIn.SharedLib.Extensions;
using Mediator;
using Microsoft.Extensions.Logging;
using Plugin.Maui.BottomSheet.Navigation;
using SingleScope.Maui.Dialogs;
using SingleScope.Maui.Dialogs.Models;
using SingleScope.Navigations.Maui.Models;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels
{
    public partial class CheckoutViewModel : BaseViewModel, INavigationAware, IQueryAttributable,
        IRecipient<OrderConfirmedMessage>, IRecipient<OrderPaidMessage>
    {
        private readonly ILogger<CheckoutViewModel> _logger;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IMediator _mediator;
        private readonly IMaterialSnackbar _snackbar;
        private readonly IOrderUnitOfWork _unitOfWork;
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly PaymentFactory _paymentFactory;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ChangeAmount))]
        private Order _currentOrder = default!;

        [ObservableProperty]
        private string _selectedPaymentMethod = PaymentMethod.Cash.Value;

        [ObservableProperty]
        private bool _cashPaymentModalOpened = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ChangeAmount))]
        private double _paidAmount = 0;

        public double ChangeAmount => PaidAmount - CurrentOrder.GrandTotal;

        public ObservableCollection<SaleProductViewModel> OrderItems { get; } = [];

        public IList<string> PaymentMethods { get; } = [.. PaymentMethod.List.Select(x => x.Value)];

        public CheckoutViewModel(
            ILogger<CheckoutViewModel> logger,
            IMapper mapper,
            IMessenger messenger,
            IPaymentService paymentService,
            IMediator mediator,
            IMaterialSnackbar snackbar,
            IOrderUnitOfWork unitOfWork,
            IDomainEventDispatcher dispatcher,
            PaymentFactory paymentFactory
            )
        {
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
            _mediator = mediator;
            _snackbar = snackbar;
            _unitOfWork = unitOfWork;
            _dispatcher = dispatcher;
            _paymentFactory = paymentFactory;

            messenger.RegisterAll(this);
        }

        void INavigationAware.OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
        {
            CashPaymentModalOpened = false;
        }

        void INavigationAware.OnNavigatedTo(IBottomSheetNavigationParameters parameters)
        {
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("items", out var items) && items is IEnumerable<SaleProductViewModel> lines)
            {
                OrderItems.Clear();
                OrderItems.AddRange(lines);
                CurrentOrder = Order.Create(OrderItems.Select(_mapper.Map<SaleProductViewModel, OrderItem>));
            }
        }

        void IRecipient<OrderConfirmedMessage>.Receive(OrderConfirmedMessage message)
        {
            _logger.LogInformation("Order created with payment method: {PaymentMethod}", message.PaymentMethod);

            switch (message.PaymentMethod.Name)
            {
                case nameof(PaymentMethod.Cash):
                    CashPaymentModalOpened = true;
                    break;
            }
        }

        void IRecipient<OrderPaidMessage>.Receive(OrderPaidMessage message)
        {
            _snackbar.Show(AppStrings.CheckoutPage_Msg_PaymentSuccess);
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
        private void SelectPaymentMethod(string item)
        {
            try
            {
                SelectedPaymentMethod = item;
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task DeleteItemAsync(SaleProductViewModel item)
        {
            try
            {
                bool ok = await _dialogService.ShowAsync(Confirmation.Untitled(AppStrings.CheckoutPage_Msg_ItemRemoveConfirmation));
                if (!ok)
                {
                    return;
                }

                OrderItems.Remove(item);
                CurrentOrder.RemoveItem(item.Entity.Id);

                OnPropertyChanged(nameof(CurrentOrder));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void DecrementItem(SaleProductViewModel item)
        {
            try
            {
                if (item.OnCartQuantity <= 0)
                {
                    return;
                }

                item.OnCartQuantity--;
                CurrentOrder.AddItemQuantity(item.Entity.Id, -1);

                OnPropertyChanged(nameof(CurrentOrder));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void IncrementItem(SaleProductViewModel item)
        {
            try
            {
                if (item.OnCartQuantity >= item.Entity.Stock.Value)
                {
                    Toast.Make(AppStrings.CheckoutPage_Msg_ItemStockLimit).Show();

                    return;
                }

                item.OnCartQuantity++;
                CurrentOrder.AddItemQuantity(item.Entity.Id, +1);

                OnPropertyChanged(nameof(CurrentOrder));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task SetDiscountAsync()
        {
            try
            {
                var discountStr = await _dialogService.ShowAsync(Dialog.Prompt(
                    title: AppStrings.CheckoutPage_Lbl_Discount,
                    message: string.Empty,
                    initialValue: $"{CurrentOrder.TotalDiscount}"));

                if (string.IsNullOrWhiteSpace(discountStr))
                {
                    return;
                }

                var discount = double.TryParse(discountStr, out var d)
                    ? d
                    : 0.0;

                CurrentOrder.ApplyDiscount(discount);
                OnPropertyChanged(nameof(CurrentOrder));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task GoToPaymentAsync(string paymentMethodStr)
        {
            try
            {
                var hasZeroItems = OrderItems.Any(x => x.OnCartQuantity == 0);
                if (hasZeroItems)
                {
                    bool accepted = await _dialogService.ShowAsync(Confirmation.Untitled(AppStrings.CheckoutPage_Msg_ZeroQuantityConfirmation));
                    if (!accepted)
                    {
                        return;
                    }
                }

                await _unitOfWork.CreateOrderAsync(CurrentOrder);

                CurrentOrder.Confirm(PaymentMethod.FromValue(paymentMethodStr));

                await _dispatcher.DispatchAsync(CurrentOrder.ConsumeEvents());
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task CashPaymentAsync()
        {
            try
            {
                var paymentMethod = PaymentMethod.Cash;
                IPayment payment = _paymentFactory.CreatePayment(paymentMethod);

                _paymentService.SetPayment(payment);

                var result = await _paymentService.ProcessPaymentAsync(PaidAmount);
                if (result.IsError)
                {
                    _logger.LogError("Payment failed with error: {Error}", string.Join(", ", result.Errors.Select(e => e.Description)));

                    await _dialogService.ShowAsync(Alert.Error(AppStrings.CheckoutPage_Msg_PaymentFailed));

                    return;
                }

                CurrentOrder.Pay(PaidAmount, paymentMethod);

                await _dispatcher.DispatchAsync(CurrentOrder.ConsumeEvents());
                await _navigation.BackAsync(ShellNavigationParams.Create(("refresh", true)));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
            finally
            {
                CashPaymentModalOpened = false;
            }
        }
    }
}
