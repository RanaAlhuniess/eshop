using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace eshop.Entities
{
    public class ProductTranslation : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(25)]
        public string LanguageCode { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)] 
        public string Description { get; set; }

        public Product Product { get; set; }
    }
}
