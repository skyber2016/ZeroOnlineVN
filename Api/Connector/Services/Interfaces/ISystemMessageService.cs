using Forum_API.Entities;

namespace Forum_API.Services.Interfaces
{
    public interface ISystemMessageService : IGeneralService<SystemMessageEntity>
    {
        public string GetMessage(string code);
        public string GetMessage(string code, object data);
    }
}
