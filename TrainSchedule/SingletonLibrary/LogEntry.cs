using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonLibrary
{
    public class LogEntry
    {
        public DateTime DateTime { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        public LogEntry(string action, string description)
        {
            DateTime = System.DateTime.Now;
            Action = action;
            Description = description;
        }

        public override string ToString()
        {
            return $"[{DateTime:dd.MM.yyyy HH:mm:ss}] {Action} - {Description}";
        }
    }
}
