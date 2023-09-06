namespace eshop.Services.Dtos.ProductAttribute
{
    public class CreateProductAttributeDto
    {
        public string Name { get; set; }
        public List<CreateProductAttributeVariantDto> Variants { get; set; }

    }
}
