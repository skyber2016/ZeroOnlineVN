using System.Net.Sockets;
using System.Threading.Tasks;

namespace Core
{
    public static class TcpExtensions
    {
        public static string GetSessionId(this TcpClient client)
        {
            try
            {
                return HashHelper.EncryptSHA256Managed(client.Client.RemoteEndPoint.ToString());
            }
            catch (System.Exception)
            {
                return "";
            }
        }
        public static string GetIP(this TcpClient client)
        {
            try
            {
                return client.Client.RemoteEndPoint.ToString();
            }
            catch (System.Exception)
            {
                return "NOIP";
            }
        }
        public static void WaitTask(this Task task)
        {
            task.Wait();
        }
        public static T WaitTask<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
