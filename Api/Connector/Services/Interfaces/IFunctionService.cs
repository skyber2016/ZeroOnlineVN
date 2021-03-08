using Forum_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forum_API.Services.Interfaces
{
    public interface IFunctionService : IGeneralService<FunctionEntity>
    {
        Task<IEnumerable<FunctionEntity>> CurrentFunctions();
    }
}
