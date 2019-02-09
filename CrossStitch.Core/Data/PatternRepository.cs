using CrossStitch.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CrossStitch.Core.Data
{
    public class PatternRepository : DatabaseContextRepositoryBase<Pattern>
    {
        protected override IQueryable<Pattern> GetQuery(DatabaseContext ctx)
        {
            return ctx.Patterns
                    .AsNoTracking()
                    .Include(x => x.Threads)
                    .ThenInclude(x => x.ThreadReference);
        }

        protected override Pattern GetSingle(IQueryable<Pattern> source, object key)
        {
            return source.SingleOrDefault(x => x.PatternId.Equals(key));
        }
    }
}