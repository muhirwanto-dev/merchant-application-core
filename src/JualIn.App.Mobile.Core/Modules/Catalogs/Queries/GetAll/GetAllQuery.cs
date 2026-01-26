using JualIn.Domain.Catalogs.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Queries.GetAll
{
    public record GetAllQuery : IQuery<IList<Product>>;
}
