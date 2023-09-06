namespace eshop.Services.Dtos.Product
{
    public class CreateProductVariantDto
    {
        public string Code { get; set; }
        public float Price { get; set; }
        public List<Guid> AttributeVariantIds { get; set; }
    }
}
