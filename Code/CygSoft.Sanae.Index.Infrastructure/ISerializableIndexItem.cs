using System;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface ISerializableIndexItem
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        string Id { get; }
        string Title { get; set; }

        string Serialize();
    }
}
