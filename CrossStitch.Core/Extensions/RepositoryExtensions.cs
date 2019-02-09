using CrossStitch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossStitch.Core.Extensions
{
    public static class RepositoryExtensions
    {
        public static T[] Query<T>(this IRepository<T> repository, ISearchCriteria<T> criteria)
        {
            return repository.Query(a => criteria.Apply(a));
        }
    }
}
