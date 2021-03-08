using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Services
{
    public class RoleFunctionService : GeneralService<RoleFunctionEntity>,  IRoleFunctionService
    {
        public async Task<IEnumerable<FunctionEntity>> GetFunctionByRoleId(long roleId)
        {
            var result = await this.FindBy(x => x.RoleId == roleId).Select(x=>x.Function).ToListAsync();
            return result;
        }
    }
}
