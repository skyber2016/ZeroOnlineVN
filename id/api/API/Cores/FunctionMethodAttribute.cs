using System;

namespace API.Cores
{
    public class FunctionMethodAttribute : Attribute
    {
        public string Name { get; set; }
        public FunctionMethodAttribute(string name = null)
        {
            this.Name = name;
        }
    }
}
