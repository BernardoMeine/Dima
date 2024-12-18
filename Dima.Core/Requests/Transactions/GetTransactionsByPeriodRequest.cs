using Dima.Core.Common.Utils;

namespace Dima.Core.Requests.Transactions;
public class GetTransactionsByPeriodRequest : PagedRequest
{
    [UtcDateTime]
    public DateTime? StartDate { get; set; }
    [UtcDateTime]
    public DateTime? EndDate { get; set; }
}
