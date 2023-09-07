
using eshop.Services.Dtos.ProductAttribute;

namespace eshop.Services.Dtos.Product
{
    public class ProductVariantValueDto
    {
        public Guid Id { get; set; }
        public ProductAttributeVariantDto ProductAttributeVariant { get; set; }

    }
}
