using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SqlKata.Execution
{
    public class MyContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {

        public MyContractResolver()
        {
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var list = type.GetProperties()
                        .Select(p =>
                        {
                            var attr = p.GetCustomAttributes(true).FirstOrDefault(f=> f.GetType() == typeof(ColumnAttribute)) as ColumnAttribute;
                            var name = p.Name;
                            if(attr != null)
                            {
                                name = attr.Name;
                            }
                            return new JsonProperty()
                            {
                                PropertyName = name,
                                PropertyType = p.PropertyType,
                                Readable = true,
                                Writable = true,
                                ValueProvider = base.CreateMemberValueProvider(p)
                            };

                        }).ToList();

            return list;
        }
    }
}
