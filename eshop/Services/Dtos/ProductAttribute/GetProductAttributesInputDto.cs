using Volo.Abp.Application.Dtos;

namespace eshop.Services.Dtos.ProductAttribute
{
    public class GetProductAttributesInputDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? Sorting { get; set; }
    }
}
