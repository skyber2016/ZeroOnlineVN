using Entity.DTO.Posts.Responses;

namespace NEWS_MVC.Services.Interfaces
{
    public interface IPostService
    {
        PostRenderResponse GetPosts();
    }
}
