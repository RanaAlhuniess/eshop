using eshop.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace eshop.Data;

public class eshopDbContext : AbpDbContext<eshopDbContext>
{
    public eshopDbContext(DbContextOptions<eshopDbContext> options)
        : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<ProductTranslation> ProductTranslations { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    public DbSet<ProductAttributeVariant> ProductAttributeVariants { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductVariantValue> ProductVariantValues { get; set; }
    public DbSet<ProductVariantImage> ProductVariantImages { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own entities here */
        builder.Entity<Product>(b =>
        {
            b.ToTable("Products");
            b.ConfigureAudited();
        });
        builder.Entity<Language>(b =>
        {
            b.ToTable("Languages");
        });
        builder.Entity<ProductTranslation>(b =>
        {
            b.ToTable("ProductTranslations")
                .HasIndex(pt => new { pt.ProductId,pt.LanguageCode })
                .IsUnique();
            b.ConfigureAudited();
        });
        builder.Entity<ProductAttribute>(b =>
        {
            b.ToTable("ProductAttributes");
        });
        builder.Entity<ProductAttributeVariant>(b =>
        {
            b.ToTable("ProductAttributeVariants");
        });
        builder.Entity<ProductVariant>(b =>
        {
            b.ToTable("ProductVariants");
        });
        builder.Entity<ProductVariantValue>(b =>
        {
            b.ToTable("ProductVariantValues");
        });
        builder.Entity<ProductVariantImage>(b =>
        {
            b.ToTable("ProductVariantImages");
        });
    }
}
