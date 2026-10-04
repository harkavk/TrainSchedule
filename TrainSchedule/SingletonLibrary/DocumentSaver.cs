using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SingletonLibrary
{
    public class DocumentSaver
    {
        private static DocumentSaver instance;

        private readonly string path;

        private DocumentSaver()
        {
            path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Documents"
            );

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static DocumentSaver GetInstance()
        {
            if (instance == null)
            {
                instance = new DocumentSaver();
            }

            return instance;
        }

        public void SaveDocument(string name, string content)
        {
            string filePath = Path.Combine(path, name + ".txt");

            File.WriteAllText(filePath, content);
        }

        public string GetDocumentsPath()
        {
            return path;
        }
    }
}
