using SharingModule.ShareLinks;
using Xunit;

namespace SharingModule.EntityFrameworkCore.ShareLinks;

[Collection(SharingModuleTestConsts.CollectionDefinitionName)]
public class EfCoreShareLinkManagerTests : ShareLinkManagerTests<SharingModuleEntityFrameworkCoreTestModule>
{
}
