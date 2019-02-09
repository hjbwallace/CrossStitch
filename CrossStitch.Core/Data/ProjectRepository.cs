using CrossStitch.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CrossStitch.Core.Data
{
    public class ProjectRepository : DatabaseContextRepositoryBase<Project>
    {
        protected override IQueryable<Project> GetQuery(DatabaseContext ctx)
        {
            return ctx.Projects
                    .Include(project => project.PatternProjects)
                    .ThenInclude(patternProject => patternProject.Pattern)
                    .ThenInclude(pattern => pattern.Threads)
                    .ThenInclude(thread => thread.ThreadReference)
                    .OrderByDescending(x => x.CreatedDate)
                    .AsNoTracking();
        }

        protected override Project GetSingle(IQueryable<Project> source, object key)
        {
            return source.SingleOrDefault(x => x.ProjectId.Equals(key));
        }

        protected override IQueryable<Project> GetSingleQuery(DatabaseContext ctx)
        {
            return ctx.Projects
                    .AsNoTracking()
                    .Include(x => x.PatternProjects)
                    .ThenInclude(x => x.Pattern);
        }
    }
}