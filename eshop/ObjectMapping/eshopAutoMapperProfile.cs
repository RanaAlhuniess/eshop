using AutoMapper;
using eshop.Entities;
using eshop.Services.Dtos.Product;
using eshop.Services.Dtos.ProductAttribute;

namespace eshop.ObjectMapping;

public class eshopAutoMapperProfile : Profile
{
    public eshopAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */
        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttributeVariant, ProductAttributeVariantDto>();
        CreateMap<Product, ProductDto>();
        CreateMap<ProductTranslation, ProductTranslationDto>();
        CreateMap<ProductVariant, ProductVariantDto>();
        CreateMap<ProductVariantValue, ProductVariantValueDto>();

    }
}
