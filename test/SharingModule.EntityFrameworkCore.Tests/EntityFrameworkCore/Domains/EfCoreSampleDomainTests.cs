using SharingModule.Samples;
using Xunit;

namespace SharingModule.EntityFrameworkCore.Domains;

[Collection(SharingModuleTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SharingModuleEntityFrameworkCoreTestModule>
{

}
