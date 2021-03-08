using Forum_API.Common;
using Forum_API.Configurations;
using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Unity;

namespace Forum_API.Services
{
    public class MailService : GeneralService<MailQueueEntity>, IMailService
    {
        [Dependency]
        public IOptions<AppSettings> Config { get; set; }

        public string GetTemplate(MailTemplateCode templateCode, object data)
        {
            var pathToFile = Config.Value.TemplateMailPath + templateCode.ToString() + ".html";
            if (!File.Exists(pathToFile))
            {
                throw new Exception($"Cannot found template mail {pathToFile}");
            }
            var text = File.ReadAllText(pathToFile);
            var properties = data.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var param = prop.GetValue(data);
                text = text.Replace(prop.Name, param?.ToString() ?? string.Empty);
            }
            return text;
        }
    }
}
