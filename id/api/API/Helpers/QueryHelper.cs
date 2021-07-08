using SqlKata;
using SqlKata.Execution;
using System.Collections.Generic;

namespace API
{
    public static class QueryHelper
    {
        public static IEnumerable<T> ToList<T>(this SqlKata.Query query)
        {
            return query.Get<T>();
        }
    }
}
