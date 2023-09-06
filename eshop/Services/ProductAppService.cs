using eshop.Entities;
using eshop.Services.Dtos.Product;
using eshop.Services.Helpers;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace eshop.Services;

public class ProductAppService : ApplicationService
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<ProductTranslation, Guid> _productTranslationRepository;

    public ProductAppService(IRepository<Product, Guid> productRepository,
        IRepository<ProductTranslation, Guid> productTranslationRepository)
    {
        _productRepository = productRepository;
        _productTranslationRepository = productTranslationRepository;
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

    private bool ValidateLanguageCode(string languageCode)
    {
        return TranslationHelper.IsLanguageCodeValid(languageCode);
    }
}