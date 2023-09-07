using eshop.Entities;

namespace eshop.Services.Dtos.Product
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public float Price { get; set; }
        public List<ProductVariantValueDto> VariantValues { get; set; }
    }
}
