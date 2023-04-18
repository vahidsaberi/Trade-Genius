using TradeGenius.WebApi.Application.TaskManager.TaskHistories;
using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class DeleteStatusRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteStatusRequest(Guid id) => Id = id;
}

public class DeleteStatusRequestHandler : IRequestHandler<DeleteStatusRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Status> _statusRepo;
    private readonly IReadRepository<TaskHistory> _taskHistoryRepo;
    private readonly IStringLocalizer _t;

    public DeleteStatusRequestHandler(IRepositoryWithEvents<Status> statusRepo, IReadRepository<TaskHistory> taskHistoryRepo, IStringLocalizer<DeleteStatusRequestHandler> localizer) =>
        (_statusRepo, _taskHistoryRepo, _t) = (statusRepo, taskHistoryRepo, localizer);

    public async Task<Guid> Handle(DeleteStatusRequest request, CancellationToken cancellationToken)
    {
        if (await _taskHistoryRepo.AnyAsync(new TaskHistoriesByStatusSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_t["Status cannot be deleted as it's being used."]);
        }

        var status = await _statusRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = status ?? throw new NotFoundException(_t["Status {0} Not Found."]);

        await _statusRepo.DeleteAsync(status, cancellationToken);

        return request.Id;
    }
}