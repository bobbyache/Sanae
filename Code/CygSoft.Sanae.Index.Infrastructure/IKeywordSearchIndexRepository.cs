using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface IKeywordSearchIndexRepository
    {
        IKeywordSearchIndex OpenIndex(string filePath, Version expectedVersion);
        void SaveIndex(IKeywordSearchIndex Index);
        IKeywordSearchIndex SaveIndexAs(IKeywordSearchIndex Index, string filePath);
        IKeywordSearchIndex CloneIndex(IKeywordSearchIndex sourceIndex, string filePath);
        IKeywordSearchIndex CreateIndex(string filePath, Version expectedVersion);

        void ImportItems(string filePath, Version expectedVersion, IKeywordIndexItem[] items);
    }
}
