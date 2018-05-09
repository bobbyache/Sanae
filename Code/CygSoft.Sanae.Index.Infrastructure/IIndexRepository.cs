using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface IIndexRepository
    {
        IIndex OpenIndex(string filePath, string expectedVersion);
        void SaveIndex(IIndex Index);
        IIndex SaveIndexAs(IIndex Index, string filePath);
        IIndex CloneIndex(IIndex sourceIndex, string filePath);
        IIndex CreateIndex(string filePath, string expectedVersion);

        void ImportItems(string filePath, string expectedVersion, IIndexItem[] items);
    }
}
