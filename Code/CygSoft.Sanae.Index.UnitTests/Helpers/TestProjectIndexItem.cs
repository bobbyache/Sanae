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
    public class TestProjectIndexItem : ProjectIndexItem
    {
        public TestProjectIndexItem(string title, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion) : 
            base(title, commaDelimitedKeywords, categoryPaths, pluginId, pluginVersion)
        {

        }

        public TestProjectIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion) : 
            base(id, title, dateCreated, dateModified, commaDelimitedKeywords, categoryPaths, pluginId, pluginVersion)
        {

        }
    }
}
