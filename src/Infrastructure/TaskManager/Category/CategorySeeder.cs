using Microsoft.Extensions.Logging;
using System.Reflection;
using TradeGenius.WebApi.Application.Common.Interfaces;
using TradeGenius.WebApi.Domain.TaskManager;
using TradeGenius.WebApi.Infrastructure.Persistence.Context;
using TradeGenius.WebApi.Infrastructure.Persistence.Initialization;

namespace TradeGenius.WebApi.Infrastructure.TaskManager;
public class CategorySeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<CategorySeeder> _logger;

    public CategorySeeder(ISerializerService serializerService, ILogger<CategorySeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!_db.Categories.Any())
        {
            _logger.LogInformation("Started to Seed Categories.");

            // Here you can use your own logic to populate the database.
            // As an example, I am using a JSON file to populate the database.
            string categoryData = await File.ReadAllTextAsync(path + "/TaskManager/Category/categories.json", cancellationToken);
            var categories = _serializerService.Deserialize<List<Category>>(categoryData);

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    await _db.Categories.AddAsync(category, cancellationToken);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Categories.");
        }
    }
}
