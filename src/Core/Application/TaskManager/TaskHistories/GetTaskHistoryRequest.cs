using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.TaskHistories;

public class GetTaskHistoryRequest : IRequest<TaskHistoryDto>
{
    public Guid Id { get; set; }

    public GetTaskHistoryRequest(Guid id) => Id = id;
}

public class TaskHistoryByIdSpec : Specification<TaskHistory, TaskHistoryDto>, ISingleResultSpecification
{
    public TaskHistoryByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}

public class GetTaskHistoryRequestHandler : IRequestHandler<GetTaskHistoryRequest, TaskHistoryDto>
{
    private readonly IRepository<TaskHistory> _repository;
    private readonly IStringLocalizer _t;

    public GetTaskHistoryRequestHandler(IRepository<TaskHistory> repository, IStringLocalizer<GetTaskHistoryRequestHandler> localizer) => (_repository, _t) = (repository, localizer);

    public async Task<TaskHistoryDto> Handle(GetTaskHistoryRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<TaskHistory, TaskHistoryDto>)new TaskHistoryByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Task History {0} Not Found.", request.Id]);
}