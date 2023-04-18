namespace TradeGenius.WebApi.Application.TaskManager.Tasks;

public class CreateTaskRequest : IRequest<Guid>
{
    public string Title { get; set; } = default!;
    public Guid CategoryId { get; set; }
}

public class CreateTaskRequestValidator : CustomValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator(IReadRepository<Domain.TaskManager.Task> repository, IStringLocalizer<CreateTaskRequestValidator> T)
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (title, ct) => await repository.FirstOrDefaultAsync(new TaskByTitleSpec(title), ct) is null)
                .WithMessage((_, title) => T["Task {0} already Exists.", title]);

        RuleFor
            (p => p.CategoryId)
            .NotEmpty();
    }
}

public class CreateTaskRequestHandler : IRequestHandler<CreateTaskRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Domain.TaskManager.Task> _repository;

    public CreateTaskRequestHandler(IRepositoryWithEvents<Domain.TaskManager.Task> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = new Domain.TaskManager.Task(request.Title, request.CategoryId);

        await _repository.AddAsync(task, cancellationToken);

        return task.Id;
    }
}