using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace eshop;

[Dependency(ReplaceServices = true)]
public class eshopBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "eshop";
}
