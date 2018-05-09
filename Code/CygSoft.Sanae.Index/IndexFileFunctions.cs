using System;
using System.IO;
using System.Xml.Linq;

namespace CygSoft.Sanae.Index
{

    public class IndexFileFunctions
    {
        public bool CheckVersion(string fileText, Version expectedVersion)
        {
            if (!string.IsNullOrEmpty(fileText))
                {
                XDocument xDocument = XDocument.Parse(fileText);
                XAttribute xVersion = xDocument.Root.Attribute("Version");

                if (xVersion != null)
                {
                    Version actualVersion;
                    bool success = Version.TryParse(xVersion.Value, out actualVersion);
                    if (success && actualVersion.ToString() == expectedVersion.ToString())
                        return true;
                }
            }
            return false;
        }

        public bool CheckFormat(string fileText)
        {
            if (!string.IsNullOrEmpty(fileText))
            {
                XDocument xDocument = XDocument.Parse(fileText);
                if (xDocument.Root.Name.ToString().StartsWith("CodeCat_"))
                    return true;
            }
            return false;
        }

        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string Open(string filePath)
        {
            string fileText = null;

            if (Exists(filePath))
            {
                using (var file = new FileStream(filePath, FileMode.Open))
                using (var reader = new StreamReader(file))
                {
                    fileText = reader.ReadToEnd();
                }
            }

            return fileText;
        }

        public void Save(string fileText, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Truncate, FileAccess.Write))
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(fileText);
                streamWriter.Flush();
            }
        }
    }
}
