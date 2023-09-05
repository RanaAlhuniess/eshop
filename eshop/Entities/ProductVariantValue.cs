using Volo.Abp.Domain.Entities;

namespace eshop.Entities
{
    public class ProductVariantValue : BasicAggregateRoot<Guid>
    {
        public Guid ProductVariantId { get; set; }
        public Guid ProductAttributeVariantId { get; set; }

        public virtual ProductVariant ProductVariant { get; set; }
        public virtual ProductAttributeVariant ProductAttributeVariant { get; set; }
    }
}
