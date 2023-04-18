using TradeGenius.WebApi.Application.TaskManager.Tasks;
using TradeGenius.WebApi.Domain.TaskManager;

namespace TradeGenius.WebApi.Application.TaskManager.Categories;

public class DeleteCategoryRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteCategoryRequest(Guid id) => Id = id;
}

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Category> _categoryRepo;
    private readonly IReadRepository<Domain.TaskManager.Task> _taskRepo;
    private readonly IStringLocalizer _t;

    public DeleteCategoryRequestHandler(IRepositoryWithEvents<Category> categoryRepo, IReadRepository<Domain.TaskManager.Task> taskRepo, IStringLocalizer<DeleteCategoryRequestHandler> localizer) =>
        (_categoryRepo, _taskRepo, _t) = (categoryRepo, taskRepo, localizer);

    public async Task<Guid> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        if (await _taskRepo.AnyAsync(new TasksByCategorySpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_t["Category cannot be deleted as it's being used."]);
        }

        var category = await _categoryRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = category ?? throw new NotFoundException(_t["Category {0} Not Found."]);

        await _categoryRepo.DeleteAsync(category, cancellationToken);

        return request.Id;
    }
}