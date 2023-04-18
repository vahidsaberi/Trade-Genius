using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class StatusByNameSpec : Specification<Status>, ISingleResultSpecification
{
    public StatusByNameSpec(string name) =>
        Query.Where(b => b.Name == name);
}