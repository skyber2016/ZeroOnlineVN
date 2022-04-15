using System.IO;
using System.Reflection;

namespace NEWS_MVC.Helpers
{
    public static class AssemblyHelper
    {
        public static long GetLastModify()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var fileInfo = new FileInfo(location);
            return fileInfo.LastWriteTime.Ticks;
        }
    }
}
