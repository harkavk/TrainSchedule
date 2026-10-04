using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SingletonLibrary
{
    public class Logger
    {
        private static Logger instance;

        private readonly string filePath;

        private Logger()
        {
            filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "log.txt"
            );
        }

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }

            return instance;
        }

        public void Log(string action, string description)
        {
            LogEntry entry = new LogEntry(action, description);

            SaveToFile(entry);
        }

        public void SaveToFile(LogEntry entry)
        {
            File.AppendAllText(
                filePath,
                entry.ToString() + Environment.NewLine
            );
        }

        public string GetLogFilePath()
        {
            return filePath;
        }
    }
}
