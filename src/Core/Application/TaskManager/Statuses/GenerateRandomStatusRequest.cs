namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public class GenerateRandomStatusRequest : IRequest<string>
{
    public int NSeed { get; set; }
}

public class GenerateRandomStatusRequestHandler : IRequestHandler<GenerateRandomStatusRequest, string>
{
    private readonly IJobService _jobService;

    public GenerateRandomStatusRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(GenerateRandomStatusRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Enqueue<IStatusGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
        return Task.FromResult(jobId);
    }
}