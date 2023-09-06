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


    public ProductAppService(IRepository<Product, Guid> productRepository,
        IRepository<ProductVariant, Guid> productVariantRepository)
    {
        _productRepository = productRepository;
        _productVariantRepository = productVariantRepository;
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
            productVariant.VariantValues ??= new List<ProductVariantValue>();
            foreach (var attributeVariantId in variantDto.AttributeVariantIds)
            {
                //TODO: Should check if we check if each attributeVariantId corresponds to an existing attribute variant that belongs to the same product
                //var attributeVariant = await _attributeVariantRepository
                //    .FirstOrDefaultAsync(av => av.Id == attributeVariantId && av.AttributeId == product.AttributeId);

                var productVariantValue = new ProductVariantValue
                {
                    ProductVariant = productVariant,
                    ProductAttributeVariantId = attributeVariantId
                };

                productVariant.VariantValues.Add(productVariantValue);
            }

            await _productVariantRepository.InsertAsync(productVariant);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

     
    }


    private bool ValidateLanguageCode(string languageCode)
    {
        return TranslationHelper.IsLanguageCodeValid(languageCode);
    }
}