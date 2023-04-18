using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class GetStatusRequest : IRequest<StatusDto>
{
    public Guid Id { get; set; }

    public GetStatusRequest(Guid id) => Id = id;
}

public class StatusByIdSpec : Specification<Status, StatusDto>, ISingleResultSpecification
{
    public StatusByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}

public class GetStatusRequestHandler : IRequestHandler<GetStatusRequest, StatusDto>
{
    private readonly IRepository<Status> _repository;
    private readonly IStringLocalizer _t;

    public GetStatusRequestHandler(IRepository<Status> repository, IStringLocalizer<GetStatusRequestHandler> localizer) => (_repository, _t) = (repository, localizer);

    public async Task<StatusDto> Handle(GetStatusRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<Status, StatusDto>)new StatusByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Status {0} Not Found.", request.Id]);
}