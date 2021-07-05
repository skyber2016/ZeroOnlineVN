using Ganss.XSS;

namespace API_CORE
{
    public static class HtmlHelper
    {
        public static string Clean(string html)
        {
            var sanitizer = new HtmlSanitizer();
            var sanitized = sanitizer.Sanitize(html);
            return sanitized;
        }
    }
}
