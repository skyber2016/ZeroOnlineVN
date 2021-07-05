using AutoMapper;
using System.Linq;
using System.Reflection;
using NEWS_API.Entities;
using NEWS_API.Security;

namespace NEWS_API.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // Add as many of these lines as you need to map your objects
            var entities = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.Name.EndsWith("Entity"));
            var requests = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.Name.EndsWith("Request"));
            var responses = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.Name.EndsWith("Response"));
            foreach (var ent in entities)
            {
                foreach (var request in requests)
                {
                    CreateMap(request, ent);
                }
                foreach (var response in responses)
                {
                    CreateMap(ent, response);
                }
                foreach(var entity in entities)
                {
                    CreateMap(entity, ent);
                }    
            }
        }
    }
}
