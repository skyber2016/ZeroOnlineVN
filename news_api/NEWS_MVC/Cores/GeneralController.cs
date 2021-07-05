using CORE_API.Helpers;
using NEWS_MVC.Services.Interfaces;
using Entity;
using Unity;

namespace NEWS_MVC.Cores
{
    public class GeneralController<EntityModel> : BaseController where EntityModel : BaseEntity, new()
    {
        [Dependency]
        public IGeneralService<EntityModel> GeneralService { get; set; }
        [Dependency]
        public ILoggerManager Logger { get; set; }
    }
}