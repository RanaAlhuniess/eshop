using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace eshop.Entities
{
    public class ProductAttribute : BasicAggregateRoot<Guid>
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual ICollection<ProductAttributeVariant> Variants{ get; set; }

        public ProductAttribute()
        {
            Variants = new List<ProductAttributeVariant>();
        }
    }

}
