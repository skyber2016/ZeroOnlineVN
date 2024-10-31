using GameServer;
class Program
{
    static int Main(string[] args)
    {
        string targetServer = "157.66.27.215"; // Địa chỉ server đích
        int targetLoginPort = 9959; // Cổng login đích
        int targetGamePort = 5816; // Cổng proxy

        var loginThread = Task.Run(() => LoginThread.Instance.ListenAsync(targetServer, targetLoginPort));
        var gameThread = Task.Run(() => GameThread.Instance.ListenAsync(targetServer, targetGamePort));
        Task.WaitAny( gameThread, loginThread);
        return 0;
    }
}
