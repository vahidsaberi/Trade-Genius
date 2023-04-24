using System.Collections.ObjectModel;

namespace TradeGenius.WebApi.Shared.Authorization;

public static class TradeGeniusAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class TradeGeniusResource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);

    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);

    public const string Categories = nameof(Categories);
    public const string Statuses = nameof(Statuses);
    public const string Tasks = nameof(Tasks);
    public const string TaskHistories = nameof(TaskHistories);

    public const string PubSub = nameof(PubSub);
}

public static class TradeGeniusPermissions
{
    private static readonly TradeGeniusPermission[] _all = new TradeGeniusPermission[]
    {
        new("View Dashboard", TradeGeniusAction.View, TradeGeniusResource.Dashboard),
        new("View Hangfire", TradeGeniusAction.View, TradeGeniusResource.Hangfire),
        new("View Users", TradeGeniusAction.View, TradeGeniusResource.Users),
        new("Search Users", TradeGeniusAction.Search, TradeGeniusResource.Users),
        new("Create Users", TradeGeniusAction.Create, TradeGeniusResource.Users),
        new("Update Users", TradeGeniusAction.Update, TradeGeniusResource.Users),
        new("Delete Users", TradeGeniusAction.Delete, TradeGeniusResource.Users),
        new("Export Users", TradeGeniusAction.Export, TradeGeniusResource.Users),
        new("View UserRoles", TradeGeniusAction.View, TradeGeniusResource.UserRoles),
        new("Update UserRoles", TradeGeniusAction.Update, TradeGeniusResource.UserRoles),
        new("View Roles", TradeGeniusAction.View, TradeGeniusResource.Roles),
        new("Create Roles", TradeGeniusAction.Create, TradeGeniusResource.Roles),
        new("Update Roles", TradeGeniusAction.Update, TradeGeniusResource.Roles),
        new("Delete Roles", TradeGeniusAction.Delete, TradeGeniusResource.Roles),
        new("View RoleClaims", TradeGeniusAction.View, TradeGeniusResource.RoleClaims),
        new("Update RoleClaims", TradeGeniusAction.Update, TradeGeniusResource.RoleClaims),
        new("View Products", TradeGeniusAction.View, TradeGeniusResource.Products, IsBasic: true),
        new("Search Products", TradeGeniusAction.Search, TradeGeniusResource.Products, IsBasic: true),
        new("Create Products", TradeGeniusAction.Create, TradeGeniusResource.Products),
        new("Update Products", TradeGeniusAction.Update, TradeGeniusResource.Products),
        new("Delete Products", TradeGeniusAction.Delete, TradeGeniusResource.Products),
        new("Export Products", TradeGeniusAction.Export, TradeGeniusResource.Products),
        new("View Brands", TradeGeniusAction.View, TradeGeniusResource.Brands, IsBasic: true),
        new("Search Brands", TradeGeniusAction.Search, TradeGeniusResource.Brands, IsBasic: true),
        new("Create Brands", TradeGeniusAction.Create, TradeGeniusResource.Brands),
        new("Update Brands", TradeGeniusAction.Update, TradeGeniusResource.Brands),
        new("Delete Brands", TradeGeniusAction.Delete, TradeGeniusResource.Brands),
        new("Generate Brands", TradeGeniusAction.Generate, TradeGeniusResource.Brands),
        new("Clean Brands", TradeGeniusAction.Clean, TradeGeniusResource.Brands),
        new("View Tenants", TradeGeniusAction.View, TradeGeniusResource.Tenants, IsRoot: true),
        new("Create Tenants", TradeGeniusAction.Create, TradeGeniusResource.Tenants, IsRoot: true),
        new("Update Tenants", TradeGeniusAction.Update, TradeGeniusResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", TradeGeniusAction.UpgradeSubscription, TradeGeniusResource.Tenants, IsRoot: true),

        new("View Categories", TradeGeniusAction.View, TradeGeniusResource.Categories, IsBasic: true),
        new("Search Categories", TradeGeniusAction.Search, TradeGeniusResource.Categories, IsBasic: true),
        new("Create Categories", TradeGeniusAction.Create, TradeGeniusResource.Categories),
        new("Update Categories", TradeGeniusAction.Update, TradeGeniusResource.Categories),
        new("Delete Categories", TradeGeniusAction.Delete, TradeGeniusResource.Categories),
        new("Generate Categories", TradeGeniusAction.Generate, TradeGeniusResource.Categories),
        new("Clean Categories", TradeGeniusAction.Clean, TradeGeniusResource.Categories),

        new("View Statuses", TradeGeniusAction.View, TradeGeniusResource.Statuses, IsBasic: true),
        new("Search Statuses", TradeGeniusAction.Search, TradeGeniusResource.Statuses, IsBasic: true),
        new("Create Statuses", TradeGeniusAction.Create, TradeGeniusResource.Statuses),
        new("Update Statuses", TradeGeniusAction.Update, TradeGeniusResource.Statuses),
        new("Delete Statuses", TradeGeniusAction.Delete, TradeGeniusResource.Statuses),
        new("Generate Statuses", TradeGeniusAction.Generate, TradeGeniusResource.Statuses),
        new("Clean Statuses", TradeGeniusAction.Clean, TradeGeniusResource.Statuses),

        new("View Tasks", TradeGeniusAction.View, TradeGeniusResource.Tasks, IsBasic: true),
        new("Search Tasks", TradeGeniusAction.Search, TradeGeniusResource.Tasks, IsBasic: true),
        new("Create Tasks", TradeGeniusAction.Create, TradeGeniusResource.Tasks),
        new("Update Tasks", TradeGeniusAction.Update, TradeGeniusResource.Tasks),
        new("Delete Tasks", TradeGeniusAction.Delete, TradeGeniusResource.Tasks),
        new("Generate Tasks", TradeGeniusAction.Generate, TradeGeniusResource.Tasks),
        new("Clean Tasks", TradeGeniusAction.Clean, TradeGeniusResource.Tasks),

        new("View TaskHistories", TradeGeniusAction.View, TradeGeniusResource.TaskHistories, IsBasic: true),
        new("Search TaskHistories", TradeGeniusAction.Search, TradeGeniusResource.TaskHistories, IsBasic: true),
        new("Create TaskHistories", TradeGeniusAction.Create, TradeGeniusResource.TaskHistories),
        new("Update TaskHistories", TradeGeniusAction.Update, TradeGeniusResource.TaskHistories),
        new("Delete TaskHistories", TradeGeniusAction.Delete, TradeGeniusResource.TaskHistories),
        new("Generate TaskHistories", TradeGeniusAction.Generate, TradeGeniusResource.TaskHistories),
        new("Clean TaskHistories", TradeGeniusAction.Clean, TradeGeniusResource.TaskHistories),

        new("Create PubSub", TradeGeniusAction.Create, TradeGeniusResource.PubSub)
    };

    public static IReadOnlyList<TradeGeniusPermission> All { get; } = new ReadOnlyCollection<TradeGeniusPermission>(_all);
    public static IReadOnlyList<TradeGeniusPermission> Root { get; } = new ReadOnlyCollection<TradeGeniusPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<TradeGeniusPermission> Admin { get; } = new ReadOnlyCollection<TradeGeniusPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<TradeGeniusPermission> Basic { get; } = new ReadOnlyCollection<TradeGeniusPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record TradeGeniusPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
