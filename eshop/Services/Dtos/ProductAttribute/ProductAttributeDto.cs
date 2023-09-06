
using eshop.Entities;

namespace eshop.Services.Dtos.ProductAttribute
{
    public class ProductAttributeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ProductAttributeVariantDto> Variants { get; set; }
    }
}
