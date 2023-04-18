using Microsoft.Extensions.Logging;
using System.Reflection;
using TradeGenius.WebApi.Application.Common.Interfaces;
using TradeGenius.WebApi.Domain.TaskManager;
using TradeGenius.WebApi.Infrastructure.Persistence.Context;
using TradeGenius.WebApi.Infrastructure.Persistence.Initialization;

namespace TradeGenius.WebApi.Infrastructure.TaskManager;
public class StatusSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<StatusSeeder> _logger;

    public StatusSeeder(ISerializerService serializerService, ILogger<StatusSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!_db.Statuses.Any())
        {
            _logger.LogInformation("Started to Seed Statuses.");

            // Here you can use your own logic to populate the database.
            // As an example, I am using a JSON file to populate the database.
            string statusData = await File.ReadAllTextAsync(path + "/TaskManager/Status/statuses.json", cancellationToken);
            var statuses = _serializerService.Deserialize<List<Status>>(statusData);

            if (statuses != null)
            {
                foreach (var status in statuses)
                {
                    await _db.Statuses.AddAsync(status, cancellationToken);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Statuses.");
        }
    }
}
