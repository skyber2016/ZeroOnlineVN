using NEWS_API.Entities;
using Entity;
using System.Linq;
using Unity;

namespace NEWS_API
{
    public static class IQueryableExtension
    {
        private static IUnityContainer Container { get; set; }
        public static void Configure(IUnityContainer container)
        {
            Container = container;
        }
        public static IQueryable<T> Pagination<T>(this IQueryable<T> query,int pageNumber, int pageSize) where T: BaseEntity,new()
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

    }
}
