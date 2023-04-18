using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class CreateStatusRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class CreateStatusRequestValidator : CustomValidator<CreateStatusRequest>
{
    public CreateStatusRequestValidator(IReadRepository<Status> repository, IStringLocalizer<CreateStatusRequestValidator> T) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.FirstOrDefaultAsync(new StatusByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["Status {0} already Exists.", name]);
}

public class CreateStatusRequestHandler : IRequestHandler<CreateStatusRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Status> _repository;

    public CreateStatusRequestHandler(IRepositoryWithEvents<Status> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateStatusRequest request, CancellationToken cancellationToken)
    {
        var status = new Status(request.Name, request.Description);

        await _repository.AddAsync(status, cancellationToken);

        return status.Id;
    }
}