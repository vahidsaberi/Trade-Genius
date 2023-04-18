namespace TradeGenius.WebApi.Domain.TaskManager;
public class TaskHistory : AuditableEntity, IAggregateRoot
{
    public Guid TaskId { get; private set; }
    public virtual Task Task { get; private set; } = default!;

    public Guid StatusId { get; private set; }
    public virtual Status Status { get; private set; } = default!;

    public DateTime UpdateDateTime { get; private set; }

    public TaskHistory()
    {
        // Only needed for working with dapper (See GetTaskHistoryViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    public TaskHistory(Guid taskId, Guid statusId)
    {
        TaskId = taskId;
        StatusId = statusId;
        UpdateDateTime = DateTime.Now;
    }

    public TaskHistory Update(Guid? taskId, Guid? statusId)
    {
        if (taskId.HasValue && taskId.Value != Guid.Empty && !TaskId.Equals(taskId.Value)) TaskId = taskId.Value;
        if (statusId.HasValue && statusId.Value != Guid.Empty && !StatusId.Equals(statusId.Value)) StatusId = statusId.Value;
        return this;
    }
}
