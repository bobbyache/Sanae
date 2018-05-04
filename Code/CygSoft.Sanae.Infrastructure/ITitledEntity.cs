using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Infrastructure
{
    public interface ITitledEntity
    {
        string Id { get; }
        string Title { get; set; }
    }
}
