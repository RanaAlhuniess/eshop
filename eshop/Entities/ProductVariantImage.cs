using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace eshop.Entities
{
    public class ProductVariantImage : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProductVariantId { get; set; }
        [Required]
        public string Data { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
