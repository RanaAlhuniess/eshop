using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace eshop.Entities;

public class Product : FullAuditedAggregateRoot<Guid>
{
    public Product()
    {
        IsActive = true;
        Variants = new List<ProductVariant>();
    }

    public Guid? ProductVariantImageId { get; set; }

    [Required]
    [StringLength(255)] 
    public string Number { get; set; }


    public bool IsActive { get; set; }
    public virtual ICollection<ProductTranslation> Translations { get; set; }
    public virtual ICollection<ProductVariant> Variants { get; set; }
}