using System;
using System.IO;

namespace FocusWatcher
{
    class Logger
    {
        static public string LogFilePath;
        static public void WriteHeader()
        {
            using (StreamWriter file = new StreamWriter(LogFilePath, true))
            {
                file.WriteLine("Heure;Durée (s);Application;Titre");
            }
        }
        static public void Log(string format, params object[] prms)
        {
            string toLog = string.Format(format, prms);
            using (StreamWriter file = new StreamWriter(LogFilePath, true))
            {
                file.WriteLine(toLog);
            }
            Console.WriteLine(toLog);
        }
    }
}