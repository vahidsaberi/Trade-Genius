namespace TradeGenius.WebApi.Application.TaskManager.Tasks;
public class TasksByCategorySpec : Specification<Domain.TaskManager.Task>
{
    public TasksByCategorySpec(Guid categoryId) =>
        Query.Where(p => p.CategoryId == categoryId);
}
