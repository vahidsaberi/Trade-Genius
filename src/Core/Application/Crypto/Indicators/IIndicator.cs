namespace TradeGenius.WebApi.Application.Crypto.Indicators;

public interface IIndicator
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
