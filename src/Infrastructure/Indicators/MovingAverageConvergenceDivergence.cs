using TradeGenius.WebApi.Application.Crypto.Enums;
using TradeGenius.WebApi.Application.Crypto.Interfaces;
using TradeGenius.WebApi.Application.Crypto.Models;
using TradeGenius.WebApi.Domain.CryptoCurrency;

namespace TradeGenius.WebApi.Infrastructure.Indicators;
public class MovingAverageConvergenceDivergence : IIndicator
{
    public string Name => nameof(MovingAverageConvergenceDivergence);
    public string Alias => "MACD";

    private int Fast { get; set; } = 12;
    public CandlePriceType FastPriceType { get; set; } = CandlePriceType.Close;
    private int Slow { get; set; } = 26;
    public CandlePriceType SlowPriceType { get; set; } = CandlePriceType.Close;
    private int Signal { get; set; } = 9;
    private List<Candle> Candles { get; set; } = new();
    private Queue<IndicatorOutput> MACDs { get; set; } = new();

    private IndicatorConfig _config;

    public enum InputParameter
    {
        Candles,
        MACDs
    }

    public enum OutputParameter
    {
        Candle,
        MACDLine,
        MACDPerscent,
        SignalLine,
        Histogram,
        FastEMA,
        SlowEMA,
        SignalEMA
    }

    public enum ConfigParameter
    {
        Fast,
        FastPriceType,
        Slow,
        SlowPriceType,
        Signal
    }

    public enum MACD_EMA_Type
    {
        Fast,
        Slow,
        Signal
    }

    public MovingAverageConvergenceDivergence() => _config = new();

    public void SetConfig(IndicatorConfig config) => this._config = config;

    public IndicatorOutput? Calculate(IndicatorInput parameter)
    {
        this.Candles = (List<Candle>)parameter.GetValue(InputParameter.Candles.ToString());

        var lastCandle = this.Candles.FirstOrDefault();
        var macd = (IndicatorOutput)lastCandle.GetValue(this.Alias);
        if (macd != null)
            return macd;

        this.MACDs = new Queue<IndicatorOutput>();

        //Get befor MACDs
        var cdls = this.Candles.GetRange(1, this.Signal + 1).ToList();
        if (cdls != null && cdls.Count > 0)
        {
            cdls = cdls.Reverse<Candle>().ToList();
            foreach (var previousCandle in cdls)
            {
                macd = (IndicatorOutput)previousCandle.GetValue(this.Alias);
                if (macd != null)
                    this.MACDs.Enqueue(macd);
            }
        }

        this.Fast = (int)this._config.GetValue(ConfigParameter.Fast.ToString());
        this.Slow = (int)this._config.GetValue(ConfigParameter.Slow.ToString());
        this.Signal = (int)this._config.GetValue(ConfigParameter.Signal.ToString());
        this.FastPriceType = (CandlePriceType)this._config.GetValue(ConfigParameter.FastPriceType.ToString());
        this.SlowPriceType = (CandlePriceType)this._config.GetValue(ConfigParameter.SlowPriceType.ToString());

        var max = new List<int>() { this.Fast, this.Slow, this.Signal }.Max();
        if (this.Candles.Count() < max)
            return null;

        if (this.MACDs.Count == 0)
        {
            var cnt = this.Candles.Count - (max + 1);

            for (int i = cnt; i >= 1; i--)
            {
                var items = this.Candles.GetRange(i, this.Candles.Count - i).ToList();
                var result = new IndicatorOutput();
                if (i == cnt)
                    result = this.Start(items);
                else
                    result = this.GetMacd(items);

                items[0].AddValue(this.Alias, result);

                this.MACDs.Enqueue(result);
                if (this.MACDs.Count > this.Signal + 1)
                    this.MACDs.Dequeue();
            }
        }

        var model = this.GetMacd(this.Candles);

        lastCandle.AddValue(this.Alias, model);

        return model;
    }

    public IndicatorOutput Start(List<Candle> candles)
    {
        var model = new IndicatorOutput();

        model.AddValue(nameof(OutputParameter.Candle), candles.FirstOrDefault());

        var fastItems = candles.GetRange(0, this.Fast).ToList();
        var emaFast = fastItems.Sum(x => this.GetPrice(x, this.FastPriceType)) / this.Fast;
        model.AddValue(nameof(OutputParameter.FastEMA), emaFast);

        var slowItems = candles.GetRange(0, this.Slow).ToList();
        var emaSlow = slowItems.Sum(x => this.GetPrice(x, this.SlowPriceType)) / this.Slow;
        model.AddValue(nameof(OutputParameter.SlowEMA), emaSlow);

        model.AddValue(nameof(OutputParameter.MACDLine), emaFast - emaSlow);
        model.AddValue(nameof(OutputParameter.MACDPerscent), 100 * ((emaFast / emaSlow) - 1));

        model.AddValue(nameof(OutputParameter.SignalLine), 0);
        model.AddValue(nameof(OutputParameter.Histogram), 0);

        return model;
    }

    public IndicatorOutput GetMacd(List<Candle> candles)
    {
        var lastCandle = candles.FirstOrDefault();

        var model = new IndicatorOutput();

        model.AddValue(nameof(OutputParameter.Candle), lastCandle);

        var fastEMA = this.EMA(lastCandle, MACD_EMA_Type.Fast);
        model.AddValue(nameof(OutputParameter.FastEMA), fastEMA);

        var slowEMA = this.EMA(lastCandle, MACD_EMA_Type.Slow);
        model.AddValue(nameof(OutputParameter.SlowEMA), slowEMA);

        var macdLine = fastEMA - slowEMA;
        model.AddValue(nameof(OutputParameter.MACDLine), macdLine);
        model.AddValue(nameof(OutputParameter.MACDPerscent), 100 * ((fastEMA / slowEMA) - 1));

        var signalLine = this.EMA(lastCandle, MACD_EMA_Type.Signal);
        model.AddValue(nameof(OutputParameter.SignalLine), signalLine);

        model.AddValue(nameof(OutputParameter.Histogram), macdLine - signalLine);

        return model;
    }

    public decimal EMA(Candle lastCandle, MACD_EMA_Type eMAType)
    {
        if (!this.MACDs.Any())
            return 0;

        var lastMacd = this.MACDs.LastOrDefault();
        if (lastMacd == null)
            return 0;

        var period = 0;
        decimal previousEMA = 0;
        decimal lastValue = 0;

        switch (eMAType)
        {
            case MACD_EMA_Type.Fast:
                period = this.Fast;
                previousEMA = (decimal)lastMacd.GetValue(nameof(OutputParameter.FastEMA));
                lastValue = this.GetPrice(lastCandle, this.FastPriceType);
                break;
            case MACD_EMA_Type.Slow:
                period = this.Slow;
                previousEMA = (decimal)lastMacd.GetValue(nameof(OutputParameter.SlowEMA));
                lastValue = this.GetPrice(lastCandle, this.SlowPriceType);
                break;
            case MACD_EMA_Type.Signal:
                if (this.MACDs.Count < this.Signal + 1)
                    return 0;

                var temp = this.MACDs.ElementAt(this.MACDs.Count - 1);
                var signalLine = (decimal)temp.GetValue(nameof(OutputParameter.SignalLine));
                if (signalLine == 0)
                {
                    signalLine = this.MACDs.ToList().GetRange(0, this.Signal).Sum(x => (decimal)x.GetValue(nameof(OutputParameter.MACDLine))) / this.Signal;
                    temp.UpdateValue(nameof(OutputParameter.SignalLine), signalLine);
                }

                period = this.Signal;
                previousEMA = (decimal)this.MACDs.ElementAt(this.MACDs.Count - 1).GetValue(nameof(OutputParameter.SignalLine));
                lastValue = (decimal)lastMacd.GetValue(nameof(OutputParameter.MACDLine));
                break;
        }

        var multiplier = 2 / (decimal)(period + 1);
        var ema = previousEMA + multiplier * (lastValue - previousEMA);

        return ema;
    }

    private decimal GetPrice(Candle candle, CandlePriceType priceType)
    {
        switch (priceType)
        {
            case CandlePriceType.Open:
                return candle.OpenPrice;
            case CandlePriceType.Low:
                return candle.LowPrice;
            case CandlePriceType.High:
                return candle.HighPrice;
            case CandlePriceType.Close:
                return candle.ClosePrice;
        }

        return 0;
    }
}
