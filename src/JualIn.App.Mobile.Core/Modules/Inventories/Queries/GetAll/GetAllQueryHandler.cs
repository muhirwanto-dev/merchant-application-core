using JualIn.Domain.Inventories.Entities;
using Mediator;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Queries.GetAll
{
    public class GetAllQueryHandler(
        IReadRepository<Inventory> _repository
        ) : IQueryHandler<GetAllQuery, IList<Inventory>>
    {
        public async ValueTask<IList<Inventory>> Handle(GetAllQuery query, CancellationToken cancellationToken)
            => await _repository.GetAllAsync(cancellationToken);
    }
}
