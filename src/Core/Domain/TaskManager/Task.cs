namespace TradeGenius.WebApi.Domain.TaskManager;
public class Task : AuditableEntity, IAggregateRoot
{
    public string Title { get; private set; }
    public DateTime CreateDateTime { get; private set; }
    public Guid CategoryId { get; private set; }
    public virtual Category Category { get; private set; } = default!;


    public Task()
    {
        // Only needed for working with dapper (See GetTaskViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    public Task(string title, Guid categoryId)
    {
        Title = title;
        CreateDateTime = DateTime.Now;
        CategoryId = categoryId;
    }

    public Task Update(string? title, Guid? categoryId)
    {
        if (title is not null && Title?.Equals(title) is not true) Title = title;
        if (categoryId.HasValue && categoryId.Value != Guid.Empty && !CategoryId.Equals(categoryId.Value)) CategoryId = categoryId.Value;
        return this;
    }
}
