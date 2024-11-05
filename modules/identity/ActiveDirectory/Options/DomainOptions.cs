namespace Light.ActiveDirectory.Options
{
    public class DomainOptions
    {
        public string Name { get; set; } = "domain.com";

        public bool Enable => !string.IsNullOrEmpty(Name);
    }
}
