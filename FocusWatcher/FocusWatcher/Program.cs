using System.Configuration;
using System.Threading;

namespace FocusWatcher
{
    class Program
    {
        static private int sleep;
        static void Main(string[] args)
        {
            InitLog();
            FocusMonitor focusMonitor = new FocusMonitor() { LongDurationIgnore = GetLongDurationIgnore() };
            while (true)
            {
                Thread.Sleep(sleep);
            }
        }
        static void InitLog()
        {
            AppSettingsReader cfg = new AppSettingsReader();
            Logger.LogFilePath = cfg.GetValue("LogPath", typeof(string)) as string;
            sleep = (int)cfg.GetValue("Sleep", typeof(int));
        }
        static int GetLongDurationIgnore()
        {
            AppSettingsReader cfg = new AppSettingsReader();
            return (int)cfg.GetValue("LongDurationIgnore", typeof(int));
        }
    }
}
