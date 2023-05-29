using TradeGenius.WebApi.Application.Crypto.Models;

namespace TradeGenius.WebApi.Application.Crypto.Interfaces;

public interface IIndicator : ITransientService
{
    string Name
    {
        get;
    }

    string Alias
    {
        get;
    }

    void SetConfig(IndicatorConfig config);

    IndicatorOutput? Calculate(IndicatorInput parameter);
}
