using TradeGenius.WebApi.Application.Crypto.Enums;
using TradeGenius.WebApi.Application.Crypto.Indicators;
using TradeGenius.WebApi.Domain.CryptoCurrency;

namespace TradeGenius.WebApi.Infrastructure.Indicators;
public class RelativeStrengthIndex : IIndicator
{
    public string Name => nameof(RelativeStrengthIndex);
    public string Alias => "RSI";

    private int Period { get; set; } = 14;
    private CandlePriceType PriceType { get; set; } = CandlePriceType.Close;
    private List<Candle> Candles { get; set; } = new();

    private IndicatorConfig _config;

    public enum InputParameter
    {
        Candles
    }

    public enum OutputParameter
    {
        Candle,
        Result
    }

    public enum ConfigParameter
    {
        Period,
        PriceType
    }

    public RelativeStrengthIndex() => _config = new();

    public void SetConfig(IndicatorConfig config) => this._config = config;

    public IndicatorOutput? Calculate(IndicatorInput parameter)
    {
        this.Candles = (List<Candle>)parameter.GetValue(nameof(InputParameter.Candles));
        this.Period = (int)this._config.GetValue(nameof(ConfigParameter.Period));
        this.PriceType = (CandlePriceType)this._config.GetValue(nameof(ConfigParameter.PriceType));

        if (this.Candles.Count() < this.Period)
            return null;

        var model = new IndicatorOutput();

        var averageProfit = 1;
        var averageLoss = 0.8;
        var rs = (averageProfit / Period) / (averageLoss / Period);
        var rsi = 100 - (100 / (1 + rs)); //=> 55.55

        model.AddValue(nameof(OutputParameter.Result), rsi);
        model.AddValue(nameof(OutputParameter.Candle), this.Candles.FirstOrDefault());

        return model;
    }
}
