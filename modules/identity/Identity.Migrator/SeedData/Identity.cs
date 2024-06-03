namespace Light.Identity.Migrator.SeedData
{
    public class DefaultUser
    {
        public const string USER_NAME = "super";
        public const string FIRST_NAME = "S.";
        public const string LAST_NAME = "A.";
        public const string EMAIL = "super@local";
        public const string PASSWORD = "123"; // don't use it on Production

        public static List<string> MASTER_USERS => new()
        {
            USER_NAME
        };
    }

    public class DefaultRole
    {
        public const string NAME = "super";
        public const string DESCRIPTION = "Super admin role";

        public static List<string> MASTER_ROLES => new()
        {
            NAME
        };
    }
}
