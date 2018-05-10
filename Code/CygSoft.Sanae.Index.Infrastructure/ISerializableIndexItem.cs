using System;
using System.Xml.Linq;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface ISerializableIndexItem
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        string Id { get; }
        string Title { get; set; }

        XElement Serialize();
    }
}
