using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeGenius.WebApi.Infrastructure.MQTTClient;
public class BrokerHostSettings
{
    public string Host { set; get; }
    public int Port { set; get; }
}
