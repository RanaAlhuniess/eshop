using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace eshop.Entities
{
    public class ProductAttributeVariant : BasicAggregateRoot<Guid>
    {
        public Guid ProductAttributeId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public virtual ProductAttribute ProductAttribute { get; set; }
        public virtual ICollection<ProductVariantValue> VariantAttributes { get; set; }

    }
}
