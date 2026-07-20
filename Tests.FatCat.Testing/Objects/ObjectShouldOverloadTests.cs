using FatCat.Testing.Enums;
using FatCat.Testing.Numbers;
using FatCat.Testing.Objects;
using FatCat.Testing.Strings;
using Tests.FatCat.Testing.Enums;

namespace Tests.FatCat.Testing.Objects;

public class ObjectShouldOverloadTests : BaseTest
{
	[Fact]
	public void StringSubjectBindsToStringComparer()
	{
		Assert.IsType<NullableStringComparer>("text".Should());
	}

	[Fact]
	public void IntSubjectBindsToNumericComparer()
	{
		Assert.IsType<NumericComparer<int>>(5.Should());
	}

	[Fact]
	public void EnumSubjectBindsToEnumComparer()
	{
		Assert.IsType<EnumComparer<TestStatus>>(TestStatus.Active.Should());
	}

	[Fact]
	public void DtoSubjectBindsToObjectComparer()
	{
		Assert.IsType<ObjectComparer>(new SampleObject().Should());
	}

	[Fact]
	public void NullReferenceSubjectBindsToObjectComparer()
	{
		object subject = null;

		Assert.IsType<ObjectComparer>(subject.Should());
	}
}
