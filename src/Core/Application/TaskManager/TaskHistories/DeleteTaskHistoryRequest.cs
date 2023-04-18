using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.TaskHistories;

public class DeleteTaskHistoryRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteTaskHistoryRequest(Guid id) => Id = id;
}

public class DeleteTaskHistoryRequestHandler : IRequestHandler<DeleteTaskHistoryRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<TaskHistory> _repository;
    private readonly IStringLocalizer _t;

    public DeleteTaskHistoryRequestHandler(IRepositoryWithEvents<TaskHistory> repository, IStringLocalizer<DeleteTaskHistoryRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(DeleteTaskHistoryRequest request, CancellationToken cancellationToken)
    {
        var taskHistory = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = taskHistory ?? throw new NotFoundException(_t["Task History {0} Not Found."]);

        await _repository.DeleteAsync(taskHistory, cancellationToken);

        return request.Id;
    }
}