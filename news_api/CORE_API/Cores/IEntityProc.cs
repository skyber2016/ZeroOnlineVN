using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;

namespace CORE_API.Cores
{
    public interface IEntityProc<T>
    {
        IEnumerable<T> ExecProcedure(IDataParameter[] obj);
        IEnumerable<TResult> ExecProcedureResult<TResult>(IDataParameter[] prs) where TResult : class, new();
    }
}
