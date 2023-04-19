using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeGenius.WebApi.Application.Common.Mailing;

namespace TradeGenius.WebApi.Application.Common.Brokering;
public interface IBrokerService : ITransientService
{
    Task GetDataAsync(CancellationToken ct);
}
