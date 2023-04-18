using System.ComponentModel;

namespace TradeGenius.WebApi.Application.TaskManager.Categories;

public interface ICategoryGeneratorJob : IScopedService
{
    [DisplayName("Generate Random Category example job on Queue notDefault")]
    Task GenerateAsync(int nSeed, CancellationToken cancellationToken);

    [DisplayName("removes all random Categories created example job on Queue notDefault")]
    Task CleanAsync(CancellationToken cancellationToken);
}
