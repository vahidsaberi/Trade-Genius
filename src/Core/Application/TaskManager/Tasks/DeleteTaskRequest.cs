using TradeGenius.WebApi.Application.TaskManager.TaskHistories;
using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Tasks;

public class DeleteTaskRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteTaskRequest(Guid id) => Id = id;
}

public class DeleteTaskRequestHandler : IRequestHandler<DeleteTaskRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.TaskManager.Task> _taskRepo;
    private readonly IReadRepository<TaskHistory> _taskHistoryRepo;
    private readonly IStringLocalizer _t;

    public DeleteTaskRequestHandler(IRepositoryWithEvents<Domain.TaskManager.Task> taskRepo, IReadRepository<TaskHistory> taskHistoryRepo, IStringLocalizer<DeleteTaskRequestHandler> localizer) =>
        (_taskRepo, _taskHistoryRepo, _t) = (taskRepo, taskHistoryRepo, localizer);

    public async Task<Guid> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        if (await _taskHistoryRepo.AnyAsync(new TaskHistoriesByTaskSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_t["Task cannot be deleted as it's being used."]);
        }

        var task = await _taskRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = task ?? throw new NotFoundException(_t["Task {0} Not Found."]);

        await _taskRepo.DeleteAsync(task, cancellationToken);

        return request.Id;
    }
}