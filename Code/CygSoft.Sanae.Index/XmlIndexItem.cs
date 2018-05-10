using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CygSoft.Sanae.Index
{
    public abstract class XmlIndexItem : IndexItem
    {
        public XmlIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion)
            : base(id, title, dateCreated, dateModified, commaDelimitedKeywords, categoryPaths, pluginId, pluginVersion)
        {
        }

        public XmlIndexItem(string title, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion)
            : base(title, commaDelimitedKeywords, categoryPaths, pluginId, pluginVersion)
        {
        }

        public virtual void Deserialize(XElement element)
        {
            base.Id = (string)element.Attribute("ID");
            this.Title = (string)element.Element("Title");
            this.DateCreated = (DateTime)element.Element("DateCreated");
            this.DateModified = (DateTime)element.Element("DateModified");
            this.KeywordsFromDelimitedList((string)element.Element("Keywords"));
        }

        public virtual XElement Serialize()
        {
            XElement element = new XElement("IndexItem",
                    new XAttribute("ID", this.Id),
                    new XElement("Title", this.Title),
                    new XElement("DateCreated", this.DateCreated),
                    new XElement("DateModified", this.DateModified),
                    new XElement("Keywords", this.CommaDelimitedKeywords)
                    );
            return element;
        }
    }
}
