using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.TaskHistoryHistories;

public class CreateTaskHistoryRequest : IRequest<Guid>
{
    public Guid TaskId { get; set; }
    public Guid StatusId { get; set; }
}

public class CreateTaskHistoryRequestValidator : CustomValidator<CreateTaskHistoryRequest>
{
    public CreateTaskHistoryRequestValidator(IReadRepository<TaskHistory> repository, IStringLocalizer<CreateTaskHistoryRequestValidator> T)
    {
        RuleFor(p => p.TaskId)
            .NotEmpty();

        RuleFor(p => p.StatusId)
            .NotEmpty();
    }
}

public class CreateTaskHistoryRequestHandler : IRequestHandler<CreateTaskHistoryRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<TaskHistory> _repository;

    public CreateTaskHistoryRequestHandler(IRepositoryWithEvents<TaskHistory> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateTaskHistoryRequest request, CancellationToken cancellationToken)
    {
        var taskHistory = new TaskHistory(request.TaskId, request.StatusId);

        await _repository.AddAsync(taskHistory, cancellationToken);

        return taskHistory.Id;
    }
}