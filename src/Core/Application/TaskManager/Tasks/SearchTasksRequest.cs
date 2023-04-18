namespace TradeGenius.WebApi.Application.TaskManager.Tasks;

public class SearchTasksRequest : PaginationFilter, IRequest<PaginationResponse<TaskDto>>
{
}

public class TasksBySearchRequestSpec : EntitiesByPaginationFilterSpec<Domain.TaskManager.Task, TaskDto>
{
    public TasksBySearchRequestSpec(SearchTasksRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Id, !request.HasOrderBy());
}

public class SearchTasksRequestHandler : IRequestHandler<SearchTasksRequest, PaginationResponse<TaskDto>>
{
    private readonly IReadRepository<Domain.TaskManager.Task> _repository;

    public SearchTasksRequestHandler(IReadRepository<Domain.TaskManager.Task> repository) => _repository = repository;

    public async Task<PaginationResponse<TaskDto>> Handle(SearchTasksRequest request, CancellationToken cancellationToken)
    {
        var spec = new TasksBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}