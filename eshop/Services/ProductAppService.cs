using eshop.Entities;
using eshop.Services.Dtos.Product;
using eshop.Services.Dtos.ProductAttribute;
using eshop.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

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

    public async Task<PagedResultDto<ProductDto>> GetListAsync(GetProductAttributesInputDto input)
    {
        try
        {
            var query = await _productRepository
                .WithDetailsAsync(pa => pa.Translations);
            
            query = query.OrderBy(attribute => attribute.Number);

            var totalCount = await AsyncExecuter.CountAsync(query);
            var attributes = await AsyncExecuter.ToListAsync(query.PageBy(input));
            return new PagedResultDto<ProductDto>(totalCount, ObjectMapper.Map<List<Product>,
                List<ProductDto>>(attributes));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<Guid> CreateAsync(ProductDto productDto)
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

    public async Task<ProductDto> GetProductWithVariantsAsync(Guid productId, string languageCode = "en")
    {
        try
        {
            var productWithVariants = await (await _productRepository.GetQueryableAsync())
                    .Include(p => p.Translations)
                    .Include(p=>p.Variants)
                    .ThenInclude(v => v.VariantValues)
                    .ThenInclude(v => v.ProductAttributeVariant)
                    .ThenInclude(v => v.ProductAttribute)
                    .FirstOrDefaultAsync(p => p.Id == productId);

            if (productWithVariants == null)
            {
                throw new EntityNotFoundException(typeof(Product), productId);
            }

            var selectedTranslation = productWithVariants?.Translations.FirstOrDefault(t => t.LanguageCode == languageCode) ??
                                      productWithVariants?.Translations.FirstOrDefault(t => t.LanguageCode == "en");

            if (selectedTranslation != null)
                productWithVariants.Translations = new List<ProductTranslation> { selectedTranslation };

            return ObjectMapper.Map<Product,
                ProductDto>(productWithVariants);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while fetching product with variants.", ex);
        }
    }
    private bool ValidateLanguageCode(string languageCode)
    {
        return TranslationHelper.IsLanguageCodeValid(languageCode);
    }
}