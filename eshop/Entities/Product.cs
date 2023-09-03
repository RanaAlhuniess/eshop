using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace eshop.Entities;

public class Product : FullAuditedAggregateRoot<Guid>
{
    public Product()
    {
        IsActive = true;
    }

    public Guid? ProductVariantImageId { get; set; }

    [Required]
    [StringLength(255)] 
    public string Number { get; set; }


    public bool IsActive { get; set; }

}