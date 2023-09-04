using eshop.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace eshop.DataSeed
{
    public class LanguageDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Language, Guid> _languageRepository;
        private readonly ICurrentTenant _currentTenant;

        public LanguageDataSeedContributor(IRepository<Language, Guid> languageRepository, ICurrentTenant currentTenant)
        {
            _languageRepository = languageRepository;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                if (await _languageRepository.GetCountAsync() > 0)
                {
                    return;
                }
                await _languageRepository.InsertAsync(
                    new Language
                    {
                        Code = "en"
                    },
                    autoSave: true
                );

                await _languageRepository.InsertAsync(
                    new Language
                    {
                        Code = "fr"
                    },
                    autoSave: true
                );

                await _languageRepository.InsertAsync(
                    new Language
                    {
                        Code = "ar"
                    },
                    autoSave: true
                );
            }
        }
    }
}
