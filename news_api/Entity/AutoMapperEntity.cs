using AutoMapper;
using System.Linq;
using System.Reflection;

namespace Entity
{
    public class AutoMapperEntityConfig: Profile
    {
        public AutoMapperEntityConfig()
        {
            
            var entities = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.Name.EndsWith("Entity")).ToList();
            var requests = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.Name.EndsWith("Request")).ToList();
            var responses = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.Name.EndsWith("Response")).ToList();
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
