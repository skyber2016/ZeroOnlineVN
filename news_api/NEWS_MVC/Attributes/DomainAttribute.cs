using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NEWS_MVC.Attributes
{
    public class DomainAttribute : ActionFilterAttribute
    {
        private IHttpContextAccessor Accessor { get; set; }
        public DomainAttribute(IHttpContextAccessor accessor)
        {
            this.Accessor = accessor;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                this.SetDomain(controller);
            }
            base.OnActionExecuting(context);

        }

        public void SetDomain(Controller controller)
        {
            controller.ViewBag.Domain = this.GetDomain();
            controller.ViewBag.ID_Domain = this.GetDomain("id");
            controller.ViewBag.Download_Domain = this.GetDomain("download");
            controller.ViewBag.WWW_Domain = this.GetDomain("www");
            controller.ViewBag.Intro_Domain = this.GetDomain("intro");
        }
        private string GetDomain(string subdomain = null)
        {
            var context = this.Accessor.HttpContext;
            var schema = context.Request.Scheme;
            var domain = "zeroonlinevn.com";
            if (subdomain == null)
            {
                return $"{schema}://{domain}";
            }
            return $"{schema}://{subdomain}.{domain}";

        }
    }
}
