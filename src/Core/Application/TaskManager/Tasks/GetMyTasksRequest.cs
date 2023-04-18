using TradeGenius.WebApi.Application.TaskManager.Categories;
using TradeGenius.WebApi.Application.TaskManager.Statuses;
using TradeGenius.WebApi.Application.TaskManager.TaskHistories;
using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Tasks;
public class GetMyTasksRequest : IRequest<List<TaskDto>>
{
    public string UserId { get; set; }

    public GetMyTasksRequest(string userId) => UserId = userId;
}

public class GetMyTasksRequestHandler : IRequestHandler<GetMyTasksRequest, List<TaskDto>>
{
    private readonly IDapperRepository _repository;
    private readonly IReadRepository<Category> _categoryRepo;
    private readonly IReadRepository<Status> _statusRepo;
    private readonly IReadRepository<TaskHistory> _taskHistoryRepo;
    private readonly IStringLocalizer _t;

    public GetMyTasksRequestHandler(IDapperRepository repository, IReadRepository<Category> categoryRepo, IReadRepository<Status> statusRepo, IReadRepository<TaskHistory> taskHistoryRepo, IStringLocalizer<GetTaskViaDapperRequestHandler> localizer) =>
        (_repository, _categoryRepo, _statusRepo, _taskHistoryRepo, _t) = (repository, categoryRepo, statusRepo, taskHistoryRepo, localizer);

    public async Task<List<TaskDto>> Handle(GetMyTasksRequest request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.QueryWithTenantAsync<Domain.TaskManager.Task>(
            $"SELECT * FROM Catalog.\"Tasks\" WHERE \"CreatedBy\"  = '{request.UserId}' AND \"TenantId\" = '@tenant'", cancellationToken: cancellationToken);

        _ = tasks ?? throw new NotFoundException(_t["Tasks created by {0} Not Found.", request.UserId]);

        // Using mapster here throws a nullreference exception because of the "BrandName" property
        // in TaskDto and the task not having a Brand assigned.

        var result = new List<TaskDto>();

        var categoryIds = tasks.Select(_ => _.CategoryId).ToList();
        var specCategories = new CategoriesSpecification(new GetCategoriesRequest(categoryIds));
        var categories = await _categoryRepo.ListAsync(specCategories, cancellationToken);

        var taskIds = tasks.Select(_ => _.Id).ToList();
        var specTaskHistories = new TaskHistoriesSpecification(new GetTaskHistoriesRequest(taskIds));
        var taskHistories = await _taskHistoryRepo.ListAsync(specTaskHistories, cancellationToken);

        var taskHistoriesItems = taskHistories.GroupBy(_ => _.TaskId).Select(_ => _.OrderByDescending(x => x.UpdateDateTime).FirstOrDefault()).ToList();
        //var statusIds = taskHistoriesItems.Select(_ => _.StatusId).ToList();
        //var specStatuses = new StatusesSpecification(new GetStatusesRequest(statusIds));
        //var statuses = await _statusRepo.ListAsync(specStatuses, cancellationToken);

        foreach (var task in tasks)
        {
            var history = taskHistoriesItems.FirstOrDefault(_ => _.TaskId == task.Id);
            result.Add(new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                CreateDateTime = task.CreateDateTime,
                CategoryId = task.CategoryId,
                CategoryName = categories.FirstOrDefault(_ => _.Id == task.CategoryId).Name,
                //StatusId = statuses.FirstOrDefault(_ => _.Id == history.StatusId).Id,
                //StatusName = statuses.FirstOrDefault(_ => _.Id == history.StatusId).Name
                StatusId = history.StatusId,
                StatusName = history.StatusName
            });
        }

        return result;
    }
}

#region Category Filter
public class GetCategoriesRequest : BaseFilter, IRequest<Category>
{
    public List<Guid> CategoryIds { get; private set; }

    public GetCategoriesRequest(List<Guid> categoryIds) => CategoryIds = categoryIds;
}

public class CategoriesSpecification : EntitiesByBaseFilterSpec<Category, CategoryDto>
{
    public CategoriesSpecification(GetCategoriesRequest request)
        : base(request) =>
        Query
            .Where(p => request.CategoryIds.Contains(p.Id), request.CategoryIds.Count > 0);
}
#endregion

#region Task History Filter
public class GetTaskHistoriesRequest : BaseFilter, IRequest<TaskHistory>
{
    public List<Guid> TaskIds { get; private set; }

    public GetTaskHistoriesRequest(List<Guid> taskIds) => TaskIds = taskIds;
}

public class TaskHistoriesSpecification : EntitiesByBaseFilterSpec<TaskHistory, TaskHistoryDto>
{
    public TaskHistoriesSpecification(GetTaskHistoriesRequest request)
        : base(request) =>
        Query
            .Where(p => request.TaskIds.Contains(p.TaskId), request.TaskIds.Count > 0);
}
#endregion

//#region Status Filter
//public class GetStatusesRequest : BaseFilter, IRequest<Status>
//{
//    public List<Guid> StatusIds { get; private set; }

//    public GetStatusesRequest(List<Guid> statusIds) => StatusIds = statusIds;
//}

//public class StatusesSpecification : EntitiesByBaseFilterSpec<Status, StatusDto>
//{
//    public StatusesSpecification(GetStatusesRequest request)
//        : base(request) =>
//        Query
//            .Where(p => request.StatusIds.Contains(p.Id), request.StatusIds.Count > 0);
//}
//#endregion