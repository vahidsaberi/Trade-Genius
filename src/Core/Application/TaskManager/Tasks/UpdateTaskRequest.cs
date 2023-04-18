namespace TradeGenius.WebApi.Application.TaskManager.Tasks;

public class UpdateTaskRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public Guid CategoryId { get; set; }
}

public class UpdateTaskRequestValidator : CustomValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator(IRepository<Domain.TaskManager.Task> repository, IStringLocalizer<UpdateTaskRequestValidator> T)
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (task, title, ct) =>
                    await repository.FirstOrDefaultAsync(new TaskByTitleSpec(title), ct)
                        is not Domain.TaskManager.Task existingTask || existingTask.Id == task.Id)
                .WithMessage((_, title) => T["Task {0} already Exists.", title]);

        RuleFor
            (p => p.CategoryId)
            .NotEmpty();

    }
}

public class UpdateTaskRequestHandler : IRequestHandler<UpdateTaskRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.TaskManager.Task> _repository;
    private readonly IStringLocalizer _t;

    public UpdateTaskRequestHandler(IRepositoryWithEvents<Domain.TaskManager.Task> repository, IStringLocalizer<UpdateTaskRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = task
        ?? throw new NotFoundException(_t["Task {0} Not Found.", request.Id]);

        task.Update(request.Title, request.CategoryId);

        await _repository.UpdateAsync(task, cancellationToken);

        return request.Id;
    }
}