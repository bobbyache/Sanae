using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index
{
    public class SerializableIndexItem : ProjectIndexItem
    {
        public SerializableIndexItem(string title, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion) : base(title, commaDelimitedKeywords, categoryPaths, pluginId, pluginVersion)
        {
        }

        public SerializableIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion) : base(id, title, dateCreated, dateModified, commaDelimitedKeywords, categoryPaths, pluginId, pluginVersion)
        {
        }
    }
}
