using System;
using System.ComponentModel;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public abstract class PersistableObject
    {
        private Guid identifyingGuid;

        public PersistableObject()
        {
            this.identifyingGuid = Guid.Empty;
            this.DateCreated = DateTime.Now;
            this.DateModified = this.DateCreated;
        }

        public PersistableObject(string guidString, DateTime dateCreated, DateTime dateModified)
        {
            this.DateCreated = dateCreated;
            this.DateModified = dateModified;
            this.identifyingGuid = new Guid(guidString);
        }

        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

        [Browsable(false)]
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
    }
}
