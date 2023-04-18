namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class StatusDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}