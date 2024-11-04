namespace IntegrationTests;

internal abstract class TestValues
{
    public const string CreateByUserId = "UserCreate";

    public static DateTimeOffset CreateAtTime = new(new DateTime(2024, 01, 01));

    public const string UpdateByUserId = "UserUpdate";

    public static DateTimeOffset UpdateAtTime = new(new DateTime(2024, 01, 02));

    public const string DeleteByUserId = "UserDelete";

    public static DateTimeOffset DeleteAtTime = new(new DateTime(2024, 01, 03));

    public const string TenandId = "TenantTest";
}
