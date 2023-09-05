namespace eshop.Services.Dtos
{
    public class ProductDto
    {
        public string Number { get; set; }
        public int? ProductVariantImageId { get; set; }
        public bool IsActive { get; set; }
        public List<ProductTranslationDto> Translations { get; set; }
    }
}
