using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.UnitTests.Helpers
{
    /// <summary>
    /// Required for testing. KeywordIndexItem is abstract.
    /// </summary>
    public class TestKeywordIndexItem : IndexItem
    {
        public TestKeywordIndexItem() : base()
        {
        }

        public TestKeywordIndexItem(string title, string commaDelimitedKeywords) : 
            base(title, commaDelimitedKeywords)
        {

        }

        public TestKeywordIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords) : 
            base(id, title, dateCreated, dateModified, commaDelimitedKeywords)
        {

        }
    }
}
