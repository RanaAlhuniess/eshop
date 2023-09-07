using Volo.Abp.Domain.Entities;

namespace eshop.Entities
{
    public class ProductVariantValue : BasicAggregateRoot<Guid>
    {
        public Guid ProductVariantId { get; set; }
        public Guid ProductAttributeVariantId { get; set; }

        public  ProductVariant ProductVariant { get; set; }
        public  ProductAttributeVariant ProductAttributeVariant { get; set; }
    }
}
