using System.ComponentModel;

namespace TradeGenius.WebApi.Application.TaskManager.Statuses;

public interface IStatusGeneratorJob : IScopedService
{
    [DisplayName("Generate Random Status example job on Queue notDefault")]
    Task GenerateAsync(int nSeed, CancellationToken cancellationToken);

    [DisplayName("removes all random statuses created example job on Queue notDefault")]
    Task CleanAsync(CancellationToken cancellationToken);
}
