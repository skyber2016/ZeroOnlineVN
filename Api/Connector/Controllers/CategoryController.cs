using Forum_API.Cores;
using Forum_API.Cores.Exceptions;
using Forum_API.DTO.Category.Requests;
using Forum_API.DTO.Category.Responses;
using Forum_API.DTO.Topic.Responses;
using Forum_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forum_API.Controllers
{
    public class CategoryController : GeneralController<CategoryEntity, CategoryGetResponse, CategoryCreateRequest, CategoryCreateResponse, CategoryUpdateRequest, CategoryUpdateResponse>
    {
    }
}
