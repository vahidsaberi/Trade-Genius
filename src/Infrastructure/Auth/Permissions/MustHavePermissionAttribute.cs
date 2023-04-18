using TradeGenius.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace TradeGenius.WebApi.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = TradeGeniusPermission.NameFor(action, resource);
}