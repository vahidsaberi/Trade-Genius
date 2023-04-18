using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.TaskHistories;
internal class TaskHistoriesByTaskSpec : Specification<TaskHistory>
{
    public TaskHistoriesByTaskSpec(Guid taskId) =>
        Query.Where(p => p.TaskId == taskId);
}
