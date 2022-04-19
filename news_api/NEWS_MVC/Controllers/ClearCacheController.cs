using Microsoft.AspNetCore.Mvc;
using NEWS_MVC.Cores;
using NEWS_MVC.Helpers;

namespace NEWS_MVC.Controllers
{
    public class ClearCacheController : BaseController
    {
        [HttpGet]
        [Route("clear-cache")]
        public IActionResult ClearCache()
        {
            MemoryCacheHelper.Clear();
            return Ok(new {message = "Xoa cache thanh cong"});
        }
    }
}
