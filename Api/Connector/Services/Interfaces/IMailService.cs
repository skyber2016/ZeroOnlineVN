using Forum_API.Common;
using Forum_API.Entities;

namespace Forum_API.Services.Interfaces
{
    public interface IMailService : IGeneralService<MailQueueEntity>
    {
        string GetTemplate(MailTemplateCode templateCode, object data);
    }
}
