using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.TaskHistories;

public class TaskHistoryDto : IDto
{
    public Guid Id { get; set; }

    public Guid TaskId { get; private set; }
    public string TaskTitle { get; set; } = default!;

    public Guid StatusId { get; private set; }
    public string StatusName { get; set; } = default!;

    public DateTime UpdateDateTime { get; private set; }
}