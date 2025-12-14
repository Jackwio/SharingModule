using SharingModule.Samples;
using Xunit;

namespace SharingModule.EntityFrameworkCore.Applications;

[Collection(SharingModuleTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SharingModuleEntityFrameworkCoreTestModule>
{

}
