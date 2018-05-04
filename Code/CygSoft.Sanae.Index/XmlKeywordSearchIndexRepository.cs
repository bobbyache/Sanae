using CygSoft.Sanae.Index.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CygSoft.Sanae.Index
{
    public abstract class XmlKeywordSearchIndexRepository<IndexItem> : IKeywordSearchIndexRepository where IndexItem : XmlKeywordIndexItem, new()
    {
        public string RootElement { get; private set; }
        protected readonly IIndexFileFunctions FileFunctions;

        public XmlKeywordSearchIndexRepository(string rootElement)
        {
            this.RootElement = rootElement;
            this.FileFunctions = new IndexFileFunctions();
        }

        public XmlKeywordSearchIndexRepository(string rootElement, IIndexFileFunctions indexFileFunctions)
        {
            this.RootElement = rootElement;
            this.FileFunctions = indexFileFunctions;
        }

        public IKeywordSearchIndex OpenIndex(string filePath, Version expectedVersion)
        {
            if (!FileFunctions.Exists(filePath))
                throw new FileNotFoundException("Index file not found.");

            string fileText = FileFunctions.Open(filePath);

            CheckFormat(fileText);
            CheckVersion(fileText, expectedVersion);

            List<IndexItem> items = LoadIndexItems(fileText, expectedVersion);
            IKeywordSearchIndex Index = new KeywordSearchIndex(filePath, expectedVersion, items.Cast<IKeywordIndexItem>().ToList());
            return Index;
        }

        protected virtual void CheckFormat(string fileText)
        {
            if (!FileFunctions.CheckFormat(fileText))
                throw new InvalidDataException("The file format for the target file does not match the format expected or the file is corrupt.");
        }

        protected void CheckVersion(string fileText, Version expectedVersion)
        {
            if (!FileFunctions.CheckVersion(fileText, expectedVersion))
                throw new InvalidFileIndexVersionException("The file version is not compatible with the current application version.");
        }

        public void SaveIndex(IKeywordSearchIndex Index)
        {
            XDocument xmlDocument = XDocument.Load(Index.FilePath, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            XElement xElement = xmlDocument.Element(this.RootElement);

            xElement.Nodes().Remove();

            foreach (IndexItem item in Index.All())
            {
                xElement.Add(item.Serialize());
            }

            FileFunctions.Save(xmlDocument.ToString(), Index.FilePath);
        }

        public IKeywordSearchIndex SaveIndexAs(IKeywordSearchIndex Index, string filePath)
        {
            IKeywordSearchIndex newIndex = CloneIndex(Index, filePath);
            CreateIndex(filePath, Index.CurrentVersion);
            SaveIndex(newIndex);
            return newIndex;
        }

        public IKeywordSearchIndex CloneIndex(IKeywordSearchIndex sourceIndex, string filePath)
        {
            IKeywordSearchIndex newIndex = new KeywordSearchIndex(filePath, sourceIndex.CurrentVersion, sourceIndex.All().ToList());
            return newIndex;
        }

        public IKeywordSearchIndex CreateIndex(string filePath, Version expectedVersion)
        {
            CreateNew(filePath, expectedVersion);
            IKeywordSearchIndex Index = new KeywordSearchIndex(filePath, expectedVersion);
            return Index;
        }

        private void CreateNew(string xmlFile, Version expectedVersion)
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

        protected abstract List<IndexItem> LoadIndexItems(string fileText, Version expectedVersion);
        
        public void ImportItems(string filePath, Version expectedVersion, IKeywordIndexItem[] importItems)
        {
            IndexItem[] imports = importItems.OfType<IndexItem>().ToArray();
            XDocument xDocument = XDocument.Load(filePath);
            string fileText = FileFunctions.Open(filePath);

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
