using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Catalogs.Events;
using JualIn.Domain.Common.Entities;
using JualIn.Domain.Common.ValueObjects;

namespace JualIn.Domain.Catalogs.Entities
{
    [Table(nameof(Product))]
    public class Product : RecoverableEntity
    {
        [StringLength(256)]
        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string? Description { get; set; }

        public byte[]? Image { get; set; }

        [Required]
        public double Price { get; set; } = 0.0;

        [Required]
        public double CapitalPrice { get; set; } = 0.0;

        public double Margin => Price - CapitalPrice;

        public Stock<int> Stock { get; set; } = new(0);

        public IList<ProductComponent> Components { get; set; } = [];

        public void Sell(int quantity)
        {
            Stock.Decrease(quantity);
            UpdatedAt = DateTime.UtcNow;

            if (Stock == 0)
            {
                AddDomainEvent(new ProductOutOfStockEvent(Id));
            }
        }
    }
}
