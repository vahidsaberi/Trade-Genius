using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.TaskHistories;

public class SearchTaskHistoriesRequest : PaginationFilter, IRequest<PaginationResponse<TaskHistoryDto>>
{
}

public class TaskHistoriesBySearchRequestSpec : EntitiesByPaginationFilterSpec<TaskHistory, TaskHistoryDto>
{
    public TaskHistoriesBySearchRequestSpec(SearchTaskHistoriesRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Id, !request.HasOrderBy());
}

public class SearchTaskHistoriesRequestHandler : IRequestHandler<SearchTaskHistoriesRequest, PaginationResponse<TaskHistoryDto>>
{
    private readonly IReadRepository<TaskHistory> _repository;

    public SearchTaskHistoriesRequestHandler(IReadRepository<TaskHistory> repository) => _repository = repository;

    public async Task<PaginationResponse<TaskHistoryDto>> Handle(SearchTaskHistoriesRequest request, CancellationToken cancellationToken)
    {
        var spec = new TaskHistoriesBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}