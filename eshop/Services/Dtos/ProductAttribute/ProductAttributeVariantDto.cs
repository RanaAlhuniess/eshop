namespace eshop.Services.Dtos.ProductAttribute
{
    public class ProductAttributeVariantDto
    {
        public Guid? Id { get; set; }
        public Guid ProductAttributeId { get; set; }
        public string Name { get; set; }
    }
}
