using Forum_API.Common;
using Forum_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum_API.Services.Interfaces
{
    public interface ISystemConfigService : IGeneralService<SystemConfigEntity>
    {
        Task<T> GetConfig<T>(ConfigCode code);
        Task<T> GetConfig<T>(string code);
        Task<long> GetDefaultRole();
        Task<bool> Hooking(ConfigCode code, IDictionary<string, string> param);
    }
}
