using Ardalis.Specification;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using MediatR;
using Microsoft.Extensions.Logging;
using TradeGenius.WebApi.Application.Common.Interfaces;
using TradeGenius.WebApi.Application.Common.Persistence;
using TradeGenius.WebApi.Application.TaskManager.Categories;
using TradeGenius.WebApi.Domain.TaskManager;
using TradeGenius.WebApi.Shared.Notifications;

namespace TradeGenius.WebApi.Infrastructure.TaskManager;
public class CategoryGeneratorJob : ICategoryGeneratorJob
{
    private readonly ILogger<CategoryGeneratorJob> _logger;
    private readonly ISender _mediator;
    private readonly IReadRepository<Category> _repository;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public CategoryGeneratorJob(
        ILogger<CategoryGeneratorJob> logger,
        ISender mediator,
        IReadRepository<Category> repository,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationSender notifications,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _mediator = mediator;
        _repository = repository;
        _progressBar = progressBar;
        _performingContext = performingContext;
        _notifications = notifications;
        _currentUser = currentUser;
        _progress = _progressBar.Create();
    }
    private async System.Threading.Tasks.Task NotifyAsync(string message, int progress, CancellationToken cancellationToken)
    {
        _progress.SetValue(progress);
        await _notifications.SendToUserAsync(
            new JobNotification()
            {
                JobId = _performingContext.BackgroundJob.Id,
                Message = message,
                Progress = progress
            },
            _currentUser.GetUserId().ToString(),
            cancellationToken);
    }

    [Queue("notdefault")]
    public async System.Threading.Tasks.Task GenerateAsync(int nSeed, CancellationToken cancellationToken)
    {
        await NotifyAsync("Your job processing has started", 0, cancellationToken);

        foreach (int index in Enumerable.Range(1, nSeed))
        {
            await _mediator.Send(
                new CreateCategoryRequest
                {
                    Name = $"Category Random - {DefaultIdType.NewGuid()}",
                    Description = "Funny description"
                },
                cancellationToken);

            await NotifyAsync("Progress: ", nSeed > 0 ? index * 100 / nSeed : 0, cancellationToken);
        }

        await NotifyAsync("Job successfully completed", 0, cancellationToken);
    }

    [Queue("notdefault")]
    [AutomaticRetry(Attempts = 5)]
    public async System.Threading.Tasks.Task CleanAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initializing Job with Id: {jobId}", _performingContext.BackgroundJob.Id);

        var items = await _repository.ListAsync(new RandomCategoriesSpec(), cancellationToken);

        _logger.LogInformation("Categories Random: {CategoriesCount} ", items.Count.ToString());

        foreach (var item in items)
        {
            await _mediator.Send(new DeleteCategoryRequest(item.Id), cancellationToken);
        }

        _logger.LogInformation("All random Categories deleted.");
    }
}

public class RandomCategoriesSpec : Specification<Category>
{
    public RandomCategoriesSpec() =>
        Query.Where(b => !string.IsNullOrEmpty(b.Name) && b.Name.Contains("Category Random"));
}