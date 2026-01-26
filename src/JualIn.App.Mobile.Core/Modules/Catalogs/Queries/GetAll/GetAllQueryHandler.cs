using JualIn.Domain.Catalogs.Entities;
using Mediator;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Queries.GetAll
{
    public class GetAllQueryHandler(
        IReadRepository<Product> _repository
        ) : IQueryHandler<GetAllQuery, IList<Product>>
    {
        public async ValueTask<IList<Product>> Handle(GetAllQuery query, CancellationToken cancellationToken)
            => await _repository.GetAllAsync(cancellationToken);
    }
}
