using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.TaskHistories;
public class TaskHistoriesByStatusSpec : Specification<TaskHistory>
{
    public TaskHistoriesByStatusSpec(Guid statusId) =>
        Query.Where(p => p.StatusId == statusId);
}
