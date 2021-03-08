using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Forum_API.Services
{
    public class SystemMessageService : GeneralService<SystemMessageEntity>, ISystemMessageService
    {
        private IDictionary<string,string> MainMessage { get; set; }
        public string GetMessage(string code)
        {
            if(this.MainMessage == null)
            {
                var data = this.FindBy(x => !x.IsDeleted).ToList();
                this.MainMessage = data.ToDictionary(x=>x.Code, x=> x.Message);
            }
            if (this.MainMessage.ContainsKey(code))
            {
                return this.MainMessage[code];
            }
            return null;
        }

        public string GetMessage(string code, object data)
        {
            var text = this.GetMessage(code) ?? string.Empty;
            if(text == string.Empty)
            {
                return null;
            }
            var properties = data.GetType().GetProperties();
            foreach (var prop in properties)
            {
                text = text.Replace("{" + prop.Name.ToLower() + "}", prop.GetValue(data)?.ToString() ?? string.Empty);
            }
            return text;
        }
    }
}
