using CrossStitch.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CrossStitch.Core.Data
{
    public class ThreadReferenceRepository : DatabaseContextRepositoryBase<ThreadReference>
    {
        protected override IQueryable<ThreadReference> GetQuery(DatabaseContext ctx)
        {
            return ctx.ThreadReferences.AsNoTracking().OrderBy(o => o.Id);
        }
    }
}