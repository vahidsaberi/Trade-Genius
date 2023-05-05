namespace TradeGenius.WebApi.Domain.CryptoCurrency;
public class Coin
{
    public string Base { get; set; }
    public string Quote { get; set; }

    public string Symbol => $"{this.Base}{this.Quote}";
}
