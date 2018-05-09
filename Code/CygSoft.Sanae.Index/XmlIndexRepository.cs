using CygSoft.Sanae.Index.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace CygSoft.Sanae.Index
{
    public abstract class XmlIndexRepository<IndexItem> : IIndexRepository where IndexItem : XmlIndexItem, new()
    {
        public string RootElement { get; private set; }
        private Func<string, bool> formatChecker;
        private Func<string, string, bool> versionChecker;

        public XmlIndexRepository(string rootElement, Func<string, bool> formatChecker, Func<string, string, bool> versionChecker)
        {
            this.RootElement = rootElement;
            this.formatChecker = formatChecker;
            this.versionChecker = versionChecker;
        }

        protected virtual bool IndexExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public IIndex OpenIndex(string filePath, string expectedVersion)
        {
            if (!IndexExists(filePath))
                throw new FileNotFoundException("Index file not found.");

            string fileText = "";

            CheckFormat(fileText);
            CheckVersion(fileText, expectedVersion);

            List<IndexItem> items = LoadIndexItems(fileText, expectedVersion);
            IIndex Index = new Index(filePath, expectedVersion, items.Cast<IIndexItem>().ToList());
            return Index;
        }

        protected virtual void CheckFormat(string fileText)
        {
            if (!formatChecker(fileText))
                throw new InvalidDataException("The file format for the target file does not match the format expected or the file is corrupt.");
        }

        protected void CheckVersion(string fileText, string expectedVersion)
        {
            if (!versionChecker(fileText, expectedVersion))
                throw new InvalidFileIndexVersionException("The file version is not compatible with the current application version.");
        }

        public void SaveIndex(IIndex Index)
        {
            XDocument xmlDocument = XDocument.Parse(LoadFile(Index.FilePath));
            XElement xElement = xmlDocument.Element(this.RootElement);

            xElement.Nodes().Remove();

            foreach (IndexItem item in Index.All())
            {
                xElement.Add(item.Serialize());
            }

            SaveFile(xmlDocument.ToString(), Index.FilePath);
        }

        protected virtual string LoadFile(string filePath)
        {
            string fileText = null;

            if (IndexExists(filePath))
            {
                using (var file = new FileStream(filePath, FileMode.Open))
                using (var reader = new StreamReader(file))
                {
                    fileText = reader.ReadToEnd();
                }
            }

            return fileText;
        }

        protected virtual void SaveFile(string fileText, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Truncate, FileAccess.Write))
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(fileText);
                streamWriter.Flush();
            }
        }

        //protected virtual XDocument LoadIndexDocument(IIndex Index)
        //{
        //    return XDocument.Load(Index.FilePath, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
        //}

        public IIndex SaveIndexAs(IIndex Index, string filePath)
        {
            IIndex newIndex = CloneIndex(Index, filePath);
            CreateIndex(filePath, Index.CurrentVersion.ToString());
            SaveIndex(newIndex);
            return newIndex;
        }

        public IIndex CloneIndex(IIndex sourceIndex, string filePath)
        {
            IIndex newIndex = new Index(filePath, sourceIndex.CurrentVersion.ToString(), sourceIndex.All().ToList());
            return newIndex;
        }

        public IIndex CreateIndex(string filePath, string expectedVersion)
        {
            CreateNew(filePath, expectedVersion);
            IIndex Index = new Index(filePath, expectedVersion);
            return Index;
        }

        private void CreateNew(string xmlFile, string expectedVersion)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = xmlDocument.CreateElement(this.RootElement);
            XmlAttribute version = xmlDocument.CreateAttribute("Version");
            version.Value = expectedVersion.ToString();

            root.Attributes.Append(version);
            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);
            xmlDocument.AppendChild(root);
            xmlDocument.Save(xmlFile);
        }

        protected abstract List<IndexItem> LoadIndexItems(string fileText, string expectedVersion);
        
        public void ImportItems(string filePath, string expectedVersion, IIndexItem[] importItems)
        {
            IndexItem[] imports = importItems.OfType<IndexItem>().ToArray();
            XDocument xDocument = XDocument.Load(filePath);
            string fileText = ""; // FileFunctions.Open(filePath);

            CheckVersion(fileText, expectedVersion);

            // ensure ids do not already exist.
            List<IndexItem> existingItems = LoadIndexItems(fileText, expectedVersion);

            bool exist = existingItems.Join(importItems, ex => ex.Id, im => im.Id,
                (ex, im) => ex.Id).Count() > 0;

            if (exist)
                throw new ApplicationException("Matching index IDs already exist on the target index.");

            foreach (var importItem in imports)
            {
                xDocument.Root.Add(importItem.Serialize());
            }

            xDocument.Save(filePath);
        }
    }
}
