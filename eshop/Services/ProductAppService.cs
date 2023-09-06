using eshop.Entities;
using eshop.Services.Dtos.Product;
using eshop.Services.Helpers;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace eshop.Services;

public class ProductAppService : ApplicationService
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<ProductVariant, Guid> _productVariantRepository;
    private readonly IRepository<ProductVariantValue, Guid> _productVariantValueRepository;

    public ProductAppService(IRepository<Product, Guid> productRepository,
        IRepository<ProductVariant, Guid> productVariantRepository,
        IRepository<ProductVariantValue, Guid> productVariantValueRepository)
    {
        _productRepository = productRepository;
        _productVariantRepository = productVariantRepository;
        _productVariantValueRepository = productVariantValueRepository;
    }

    public async Task<Guid> CreateProductWithTranslationsAsync(ProductDto productDto)
    {
        try
        {
            var invalidLanguageCodes = productDto.Translations
                .Where(translationDto => !ValidateLanguageCode(translationDto.LanguageCode))
                .Select(translationDto => translationDto.LanguageCode)
                .ToList();

            if (invalidLanguageCodes.Any())
                throw new UserFriendlyException($"Invalid language codes: {string.Join(", ", invalidLanguageCodes)}");

            var product = new Product
            {
                Number = productDto.Number,
                IsActive = productDto.IsActive
            };
            product.Translations ??= new List<ProductTranslation>();
            foreach (var translationDto in productDto.Translations)
            {
                var translation = new ProductTranslation
                {
                    LanguageCode = translationDto.LanguageCode,
                    Name = translationDto.Name,
                    Description = translationDto.Description
                };

                product.Translations.Add(translation);
            }

            await _productRepository.InsertAsync(product, true);

            return product.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task CreateProductVariantAsync(Guid id, List<CreateProductVariantDto> input)
    {
        try
        {

            var product = await _productRepository.GetAsync(id);

            if (product == null) throw new EntityNotFoundException(typeof(Product), id);

            foreach (var variantDto in input)
            {
                var productVariant = new ProductVariant
                {
                    ProductId = product.Id,
                    Code = variantDto.Code,
                    Price = variantDto.Price
                };
                await _productVariantRepository.InsertAsync(productVariant);

                productVariant.VariantValues ??= new List<ProductVariantValue>();
                var variantValuesToInsert = new List<ProductVariantValue>();
                foreach (var attributeVariantId in variantDto.AttributeVariantIds)
                {
                    
                    var existingValue = variantValuesToInsert
                        .FirstOrDefault(v => v.ProductAttributeVariantId == attributeVariantId);
                    if (existingValue == null)
                    {
                        var productVariantValue = new ProductVariantValue
                        {
                            ProductVariant = productVariant,
                            ProductAttributeVariantId = attributeVariantId
                        };

                        variantValuesToInsert.Add(productVariantValue);
                    }

                }
                await _productVariantValueRepository.InsertManyAsync(variantValuesToInsert);

            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }


    private bool ValidateLanguageCode(string languageCode)
    {
        return TranslationHelper.IsLanguageCodeValid(languageCode);
    }
}