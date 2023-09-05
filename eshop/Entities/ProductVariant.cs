using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace eshop.Entities
{
    public class ProductVariant : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(255)]
        public string Code { get; set; }

        [Required]
        public float Price { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<ProductVariantValue> VariantValues { get; set; }
    }
}
