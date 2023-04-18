using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Tasks;

public class TaskDto : IDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreateDateTime { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public Guid? StatusId { get; set; }
    public string? StatusName { get; set; } = default!;
}