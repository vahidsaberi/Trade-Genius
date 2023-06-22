namespace TradeGenius.WebApi.Infrastructure.Persistence.Configuration;

internal static class SchemaNames
{
    // TODO: figure out how to capitalize these only for Oracle
    public static string Base = nameof(Base); // "BASE";
    public static string Auditing = nameof(Auditing); // "AUDITING";
    public static string Identity = nameof(Identity); // "IDENTITY";
    public static string MultiTenancy = nameof(MultiTenancy); // "MULTITENANCY";
    public static string DutyManager = nameof(DutyManager); // "DUTYMANAGER";
    public static string Shop = nameof(Shop); // "SHOP";
    public static string Message = nameof(Message); // "MESSAGE";
}