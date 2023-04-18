using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class SearchStatusesRequest : PaginationFilter, IRequest<PaginationResponse<StatusDto>>
{
}

public class StatusesBySearchRequestSpec : EntitiesByPaginationFilterSpec<Status, StatusDto>
{
    public StatusesBySearchRequestSpec(SearchStatusesRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchStatussRequestHandler : IRequestHandler<SearchStatusesRequest, PaginationResponse<StatusDto>>
{
    private readonly IReadRepository<Status> _repository;

    public SearchStatussRequestHandler(IReadRepository<Status> repository) => _repository = repository;

    public async Task<PaginationResponse<StatusDto>> Handle(SearchStatusesRequest request, CancellationToken cancellationToken)
    {
        var spec = new StatusesBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}