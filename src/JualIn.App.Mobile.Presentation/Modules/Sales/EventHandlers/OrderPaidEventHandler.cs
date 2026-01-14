using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Modules.Sales.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Sales.Factories;
using JualIn.App.Mobile.Presentation.Modules.Sales.Messages;
using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Sales.Entities;
using JualIn.Domain.Sales.Events;
using SingleScope.Persistence.Abstraction;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.EventHandlers
{
    public sealed class OrderPaidEventHandler(
        IReportingService _reporting,
        IReadRepository<Order> _orderRepository,
        IOrderTransactionRepository _orderTransactionRepository,
        IMessenger _messenger,
        OrderTransactionFactory _factory
        ) : IDomainEventHandler<OrderPaidEvent>
    {
        public async Task Handle(OrderPaidEvent domainEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _orderRepository.FirstOrDefaultAsync(x => x.OrderId == domainEvent.OrderId, cancellationToken)
                    ?? ThrowHelper.ThrowInvalidOperationException<Order>($"Order with id: {domainEvent.OrderId} not found!");
                var orderTransaction = _factory.Create(entity, domainEvent.PaidAmount, domainEvent.PaymentMethod);

                await _orderTransactionRepository.AddAsync(orderTransaction, cancellationToken);
                await _orderTransactionRepository.SaveAsync(cancellationToken);

                _messenger.Send(new OrderPaidMessage());
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }
    }
}
