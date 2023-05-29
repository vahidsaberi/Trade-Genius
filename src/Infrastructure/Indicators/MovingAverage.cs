using TradeGenius.WebApi.Application.Crypto.Enums;
using TradeGenius.WebApi.Application.Crypto.Interfaces;
using TradeGenius.WebApi.Application.Crypto.Models;
using TradeGenius.WebApi.Domain.CryptoCurrency;

namespace TradeGenius.WebApi.Infrastructure.Indicators;
public class MovingAverage : IIndicator
{
    public string Name => nameof(MovingAverage);
    public string Alias => "MA";

    private int Period { get; set; } = 25;
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
        Rate,
        PriceType
    }

    public MovingAverage() => _config = new();

    public void SetConfig(IndicatorConfig config) => this._config = config;

    public IndicatorOutput? Calculate(IndicatorInput parameter)
    {
        this.Candles = (List<Candle>)parameter.GetValue(nameof(InputParameter.Candles));
        this.Period = (int)this._config.GetValue(nameof(ConfigParameter.Rate));
        this.PriceType = (CandlePriceType)this._config.GetValue(nameof(ConfigParameter.PriceType));

        if (this.Candles.Count() < this.Period)
            return null;

        var model = new IndicatorOutput();

        model.AddValue(nameof(OutputParameter.Result), this.GetAverage(this.Candles.GetRange(0, this.Period).ToList()));
        model.AddValue(nameof(OutputParameter.Candle), this.Candles.FirstOrDefault());

        return model;
    }

    private decimal GetAverage(List<Candle> candles)
    {
        decimal sum = 0;
        decimal avg = 0;

        switch (this.PriceType)
        {
            case CandlePriceType.Open:
                sum = candles.Sum(x => x.OpenPrice);
                break;
            case CandlePriceType.Low:
                sum += candles.Sum(x => x.LowPrice);
                break;
            case CandlePriceType.High:
                sum += candles.Sum(x => x.HighPrice);
                break;
            case CandlePriceType.Close:
                sum += candles.Sum(x => x.ClosePrice);
                break;
        }

        avg = sum / candles.Count;

        return avg;
    }
}
