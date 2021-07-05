using Microsoft.AspNetCore.Mvc.Filters;

namespace NEWS_API.Attributes
{
    public class CacheableAttribute : ActionFilterAttribute
    {
        public int? TimeCache { get; set; } //second
        public CacheableAttribute(int? secondsTime)
        {
            this.TimeCache = secondsTime;
        }
        public CacheableAttribute()
        {
        }
    }
}
