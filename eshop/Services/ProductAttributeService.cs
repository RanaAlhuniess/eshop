using eshop.Entities;
using eshop.Services.Dtos.ProductAttribute;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace eshop.Services;

public class ProductAttributeService : ApplicationService

{
    private readonly IRepository<ProductAttribute, Guid> _productAttributeRepository;

    private readonly IRepository<ProductAttributeVariant, Guid> _productAttributeVariantRepository;

    public ProductAttributeService(IRepository<ProductAttribute, Guid> productAttributeRepository, IRepository<ProductAttributeVariant, Guid> productAttributeVariantRepository)
    {
        _productAttributeRepository = productAttributeRepository;
        _productAttributeVariantRepository = productAttributeVariantRepository;
    }

    public async Task<PagedResultDto<ProductAttributeDto>> GetListAsync(GetProductAttributesInputDto input)
    {
        try
        {
            var query = await _productAttributeRepository
                .WithDetailsAsync(pa => pa.Variants);

            if (!string.IsNullOrEmpty(input.Filter))
                query = query.Where(attribute => attribute.Name.Contains(input.Filter));

            query = ApplySorting(query, input.Sorting);

            var totalCount = await AsyncExecuter.CountAsync(query);
            var attributes = await AsyncExecuter.ToListAsync(query.PageBy(input));
            return new PagedResultDto<ProductAttributeDto>(totalCount, ObjectMapper.Map<List<ProductAttribute>,
                List<ProductAttributeDto>>(attributes));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private IQueryable<ProductAttribute> ApplySorting(IQueryable<ProductAttribute> query, string sorting)
    {
        if (string.IsNullOrWhiteSpace(sorting) || sorting.Equals("name", StringComparison.OrdinalIgnoreCase))
            query = query.OrderBy(attribute => attribute.Name);
        else if (sorting.Equals("name desc", StringComparison.OrdinalIgnoreCase))
            query = query.OrderByDescending(attribute => attribute.Name);

        return query;
    }

    public async Task<Guid> CreateAsync(ProductAttributeDto productAttributeDto)
    {
        try
        {
            if (!productAttributeDto.Variants.Any())
                throw new BusinessException("You cannot create an Attribute without associated AttributeVariants.");
            var productAttribute = new ProductAttribute
            {
                Name = productAttributeDto.Name
            };

            productAttribute.Variants ??= new List<ProductAttributeVariant>();

            foreach (var variantDto in productAttributeDto.Variants)
            {
                var variant = new ProductAttributeVariant
                {
                    Name = variantDto.Name
                };

                productAttribute.Variants.Add(variant);
            }

            await _productAttributeRepository.InsertAsync(productAttribute, true);

            return productAttribute.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var productAttribute = await _productAttributeRepository.GetAsync(id);

        if (productAttribute == null) throw new EntityNotFoundException(typeof(ProductAttribute), id);

        await _productAttributeRepository.DeleteAsync(id);
    }

    public async Task UpdateAsync(Guid id, ProductAttributeDto productAttributeDto)
    {
        var productAttribute = await _productAttributeRepository.GetAsync(id);

        if (productAttribute == null)
        {
            throw new EntityNotFoundException(typeof(ProductAttribute), id);
        }

        if (!productAttributeDto.Variants.Any())
        {
            throw new BusinessException("You cannot update an Attribute without associated AttributeVariants.");
        }

        productAttribute.Name = productAttributeDto.Name;

        await _productAttributeRepository.UpdateAsync(productAttribute, true);
    }

    public async Task UpdateAttributeAndValuesAsync(Guid id,ProductAttributeDto productAttributeDto)
    {
        using var uow = UnitOfWorkManager.Begin();
        try
        {
            var productAttribute = await (await _productAttributeRepository
                    .WithDetailsAsync(pa => pa.Variants))
                .FirstOrDefaultAsync(pa => pa.Id == id);

            if (productAttribute == null)
            {
                throw new EntityNotFoundException(typeof(ProductAttribute), id);
            }

            productAttribute.Name = productAttributeDto.Name;

            var variantsToRemove = productAttribute.Variants
                .Where(v => productAttributeDto.Variants.All(dto => dto.Id != v.Id))
                .ToList();

            await _productAttributeVariantRepository.DeleteManyAsync(variantsToRemove);

            var variantsToInsert = new List<ProductAttributeVariant>();
            foreach (var newValue in productAttributeDto.Variants)
            {
                if (newValue.Id == null)
                {
                    var newVariant = new ProductAttributeVariant
                    {
                        ProductAttributeId = productAttribute.Id,
                        Name = newValue.Name
                    };

                    variantsToInsert.Add(newVariant);
                }
            }
            if (variantsToInsert.Any())
            {
                await _productAttributeVariantRepository.InsertManyAsync(variantsToInsert);
            }
           
            await CurrentUnitOfWork.SaveChangesAsync();

            await uow.CompleteAsync(); 
        }
        catch
        {
            uow.Dispose();
            throw;
        }
    }

}