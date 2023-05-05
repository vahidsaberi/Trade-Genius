namespace TradeGenius.WebApi.Domain.CryptoCurrency;

public class Candle : ManageParameter
{
    public Coin Coin { get; set; }
    public DateTime OpenTime { get; set; }
    public DateTime CloseTime { get; set; }
    public decimal OpenPrice { get; set; }
    public decimal LowPrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal ClosePrice { get; set; }
    public DateTime EventTime { get; set; }
}
