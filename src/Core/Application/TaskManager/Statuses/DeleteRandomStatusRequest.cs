namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class DeleteRandomStatusRequest : IRequest<string>
{
}

public class DeleteRandomStatusRequestHandler : IRequestHandler<DeleteRandomStatusRequest, string>
{
    private readonly IJobService _jobService;

    public DeleteRandomStatusRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(DeleteRandomStatusRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Schedule<IStatusGeneratorJob>(x => x.CleanAsync(default), TimeSpan.FromSeconds(5));
        return Task.FromResult(jobId);
    }
}