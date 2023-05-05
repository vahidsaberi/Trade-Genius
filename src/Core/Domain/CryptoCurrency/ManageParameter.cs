namespace TradeGenius.WebApi.Domain.CryptoCurrency;

public class ManageParameter
{
    private Dictionary<string, object> Parameters { get; set; } = new();

    public object GetValue(string key) => Parameters[key];

    public void AddValue(string key, object value) => this.Parameters.Add(key, value);

    public void UpdateValue(string key, object value) => Parameters[key] = value;
}
