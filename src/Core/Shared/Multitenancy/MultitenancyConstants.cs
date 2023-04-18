namespace TradeGenius.WebApi.Shared.Multitenancy;

public class MultitenancyConstants
{
    public static class Root
    {
        public const string Id = "root";
        public const string Name = "Root";
        public const string EmailAddress = "admin@root.com";
    }

    public const string DefaultPassword = "Bo!2bjaq";

    public const string TenantIdName = "tenant";
}