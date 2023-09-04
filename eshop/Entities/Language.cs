using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace eshop.Entities
{
    public class Language : BasicAggregateRoot<Guid>
    {
        [Required]
        [StringLength(25)]
        public string Code { get; set; }
    }
}
