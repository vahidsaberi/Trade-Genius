using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class UpdateStatusRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class UpdateStatusRequestValidator : CustomValidator<UpdateStatusRequest>
{
    public UpdateStatusRequestValidator(IRepository<Status> repository, IStringLocalizer<UpdateStatusRequestValidator> T) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (status, name, ct) =>
                    await repository.FirstOrDefaultAsync(new StatusByNameSpec(name), ct)
                        is not Status existingStatus || existingStatus.Id == status.Id)
                .WithMessage((_, name) => T["Status {0} already Exists.", name]);
}

public class UpdateStatusRequestHandler : IRequestHandler<UpdateStatusRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Status> _repository;
    private readonly IStringLocalizer _t;

    public UpdateStatusRequestHandler(IRepositoryWithEvents<Status> repository, IStringLocalizer<UpdateStatusRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        var status = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = status
        ?? throw new NotFoundException(_t["Status {0} Not Found.", request.Id]);

        status.Update(request.Name, request.Description);

        await _repository.UpdateAsync(status, cancellationToken);

        return request.Id;
    }
}