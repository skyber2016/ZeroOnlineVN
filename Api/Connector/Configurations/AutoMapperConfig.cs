using AutoMapper;
using Forum_API.DTO.Base;
using Forum_API.Entities;
using Forum_API.Security;
using System.Linq;
using System.Reflection;

namespace Forum_API.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<UserEntity, UserPrincipal>();
            CreateMap<UserPrincipal, BaseUserGetResponse>();
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
            }
        }
    }
}
