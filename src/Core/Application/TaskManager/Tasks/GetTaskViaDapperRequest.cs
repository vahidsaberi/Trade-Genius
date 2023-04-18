namespace TradeGenius.WebApi.Application.TaskManager.Tasks;
public class GetTaskViaDapperRequest : IRequest<TaskDto>
{
    public Guid Id { get; set; }

    public GetTaskViaDapperRequest(Guid id) => Id = id;
}

public class GetTaskViaDapperRequestHandler : IRequestHandler<GetTaskViaDapperRequest, TaskDto>
{
    private readonly IDapperRepository _repository;
    private readonly IStringLocalizer _t;

    public GetTaskViaDapperRequestHandler(IDapperRepository repository, IStringLocalizer<GetTaskViaDapperRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<TaskDto> Handle(GetTaskViaDapperRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.QueryFirstOrDefaultAsync<Domain.TaskManager.Task>(
            $"SELECT * FROM Catalog.\"Tasks\" WHERE \"Id\"  = '{request.Id}' AND \"TenantId\" = '@tenant'", cancellationToken: cancellationToken);

        _ = task ?? throw new NotFoundException(_t["Task {0} Not Found.", request.Id]);

        // Using mapster here throws a nullreference exception because of the "BrandName" property
        // in TaskDto and the task not having a Brand assigned.
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            CreateDateTime = task.CreateDateTime,
            CategoryId = task.CategoryId,
            CategoryName = string.Empty
        };
    }
}
