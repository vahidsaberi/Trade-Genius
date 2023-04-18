using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.TaskHistories;

public class UpdateTaskHistoryRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid StatusId { get; set; }
}

public class UpdateTaskHistoryRequestValidator : CustomValidator<UpdateTaskHistoryRequest>
{
    public UpdateTaskHistoryRequestValidator(IRepository<TaskHistory> repository, IStringLocalizer<UpdateTaskHistoryRequestValidator> T)
    {
        RuleFor(p => p.TaskId)
            .NotEmpty();

        RuleFor(p => p.StatusId)
            .NotEmpty();
    }
}

public class UpdateTaskHistoryRequestHandler : IRequestHandler<UpdateTaskHistoryRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<TaskHistory> _repository;
    private readonly IStringLocalizer _t;

    public UpdateTaskHistoryRequestHandler(IRepositoryWithEvents<TaskHistory> repository, IStringLocalizer<UpdateTaskHistoryRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(UpdateTaskHistoryRequest request, CancellationToken cancellationToken)
    {
        var taskHistory = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = taskHistory
        ?? throw new NotFoundException(_t["Task History {0} Not Found.", request.Id]);

        taskHistory.Update(request.TaskId, request.StatusId);

        await _repository.UpdateAsync(taskHistory, cancellationToken);

        return request.Id;
    }
}