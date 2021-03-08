using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Services
{
    public class FunctionService : GeneralService<FunctionEntity>, IFunctionService
    {
        [Dependency]
        public IRoleFunctionService RoleFunctionService { get; set; }
        public async Task<IEnumerable<FunctionEntity>> CurrentFunctions()
        {
            var currentUser = this.UserService.GetCurrentUser();
            IEnumerable<FunctionEntity> functions;
            if (currentUser.IsSystem)
            {
                functions = await this.FindBy(x => !x.IsDeleted).ToListAsync();
            }
            else
            {
                functions = await this.RoleFunctionService.GetFunctionByRoleId(currentUser.RoleId);
            }
            return functions;
        }
    }
}
