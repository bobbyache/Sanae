using CygSoft.Sanae.Index.Infrastructure;
using System;

namespace CygSoft.Sanae.Index
{
    public abstract class SerializableIndexItem : ISerializableIndexItem
    {
        private string title;
        private Guid identifyingGuid;

        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

        public string Id
        {
            get
            {
                if (identifyingGuid == Guid.Empty)
                    identifyingGuid = Guid.NewGuid();
                return identifyingGuid.ToString();
            }
            protected set
            {
                identifyingGuid = new Guid(value);
            }
        }

        public string Title
        {
            get { return title; }
            set { title = value; this.DateModified = DateTime.Now; }
        }

        public SerializableIndexItem(string title)
        {
            this.title = title;
        }

        public SerializableIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified)
        {
            if (dateModified < dateCreated)
                throw new ApplicationException("Modified date cannot be smaller than the create date.");

            this.DateCreated = dateCreated;
            this.DateModified = dateModified;
            this.identifyingGuid = new Guid(id);
            this.title = title;
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
