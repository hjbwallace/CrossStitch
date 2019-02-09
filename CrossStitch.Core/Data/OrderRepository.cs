using CrossStitch.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CrossStitch.Core.Data
{
    public class OrderRepository : DatabaseContextRepositoryBase<Order>
    {
        protected override IQueryable<Order> GetQuery(DatabaseContext ctx)
        {
            return ctx.Orders
                .AsNoTracking()
                .Include(x => x.Threads)
                .ThenInclude(x => x.ThreadReference);
        }

        protected override Order GetSingle(IQueryable<Order> source, object key)
        {
            return source.SingleOrDefault(x => x.OrderId.Equals(key));
        }
    }
}