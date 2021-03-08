using Microsoft.AspNetCore.Mvc.Filters;

namespace Forum_API.Cores
{
    public class FunctionAttribute : ActionFilterAttribute
    {
        public int FunctionId { get; set; }
        public string Name { get; set; }
        public FunctionAttribute(int functionId, string name = null)
        {
            this.FunctionId = functionId;
            this.Name = name;
        }
    }
}
