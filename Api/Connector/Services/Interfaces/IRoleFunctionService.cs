using Forum_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum_API.Services.Interfaces
{
    public interface IRoleFunctionService : IGeneralService<RoleFunctionEntity>
    {
        Task<IEnumerable<FunctionEntity>> GetFunctionByRoleId(long roleId);
    }
}
