using Xunit;

namespace SharingModule.EntityFrameworkCore;

[CollectionDefinition(SharingModuleTestConsts.CollectionDefinitionName)]
public class SharingModuleEntityFrameworkCoreCollection : ICollectionFixture<SharingModuleEntityFrameworkCoreFixture>
{

}
