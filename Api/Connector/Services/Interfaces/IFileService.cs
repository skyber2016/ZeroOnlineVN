using Forum_API.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Forum_API.Services.Interfaces
{
    public interface IFileService : IGeneralService<FileEntity>
    {
        Task<string> Push(IFormFile file);
    }
}
