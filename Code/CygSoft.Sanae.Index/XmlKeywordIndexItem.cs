using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CygSoft.Sanae.Index
{
    public abstract class XmlKeywordIndexItem : KeywordIndexItem
    {

        public XmlKeywordIndexItem() : base()
        {
        }

        public XmlKeywordIndexItem(string id, string title, string syntax, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords)
            : base(id, title, dateCreated, dateModified, commaDelimitedKeywords)
        {
        }

        public XmlKeywordIndexItem(string title, string syntax, string commaDelimitedKeywords)
            : base(title, commaDelimitedKeywords)
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
