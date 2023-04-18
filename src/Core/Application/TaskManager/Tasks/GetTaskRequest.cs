namespace TradeGenius.WebApi.Application.TaskManager.Tasks;

public class GetTaskRequest : IRequest<TaskDto>
{
    public Guid Id { get; set; }

    public GetTaskRequest(Guid id) => Id = id;
}

public class TaskByIdSpec : Specification<Domain.TaskManager.Task, TaskDto>, ISingleResultSpecification
{
    public TaskByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}

public class GetTaskRequestHandler : IRequestHandler<GetTaskRequest, TaskDto>
{
    private readonly IRepository<Domain.TaskManager.Task> _repository;
    private readonly IStringLocalizer _t;

    public GetTaskRequestHandler(IRepository<Domain.TaskManager.Task> repository, IStringLocalizer<GetTaskRequestHandler> localizer) => (_repository, _t) = (repository, localizer);

    public async Task<TaskDto> Handle(GetTaskRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<Domain.TaskManager.Task, TaskDto>)new TaskByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Task {0} Not Found.", request.Id]);
}