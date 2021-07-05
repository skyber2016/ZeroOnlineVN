using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICORE.Attributes
{
    public class ProcedureAttribute : Attribute
    {
        public string Name { get; set; }
        public ProcedureAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
