namespace TradeGenius.WebApi.Application.Crypto.Interfaces;

public interface ISignalCreatorService : ITransientService
{
    Task CalculateStrategi(CancellationToken cancellationToken = default);
}
