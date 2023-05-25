using Microsoft.Extensions.Logging;
using TradeGenius.WebApi.Application.Crypto.Interfaces;

namespace TradeGenius.WebApi.Infrastructure.Indicators;

public class SignalCreatorService : ISignalCreatorService
{
    private readonly ILogger<SignalCreatorService> _logger;

    public SignalCreatorService(ILogger<SignalCreatorService> logger) =>
        (_logger) = (logger);

    public async Task CalculateStrategi(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
