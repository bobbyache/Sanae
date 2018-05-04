using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface ISimpleIndexItem
    {
        string Id { get; }
        string Title { get; set; }
    }
}
