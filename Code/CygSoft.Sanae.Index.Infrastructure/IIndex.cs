using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface IIndex
    {
        void AddKeywords(IIndexItem[] indeces, string delimitedKeywordList);
        IIndexItem[] All();
        string[] AllKeywords(IIndexItem[] indeces);
        bool Contains(IIndexItem item);
        bool Contains(string id);
        string CopyAllKeywords(IIndexItem[] indeces);
        Version CurrentVersion { get; }
        string FilePath { get; }
        string FileTitle { get; }
        IIndexItem[] Find(string commaDelimitedKeywords);
        bool FindAllForEmptySearch { get; set; }
        IIndexItem FindById(string id);
        IIndexItem[] FindByIds(string[] ids);
        string FolderPath { get; }
        event EventHandler IndexModified;
        int ItemCount { get; }
        string[] Keywords { get; }
        string LibraryFolderPath { get; }
        void Remove(string id);
        bool IndexesExistFor(IIndexItem[] indeces, out IIndexItem[] existingIndeces);
        bool ValidateRemoveKeywords(IIndexItem[] indeces, string[] keywords, out IIndexItem[] invalidEmptyItems);
        void RemoveKeywords(IIndexItem[] indeces, string[] keywords);
        void Update(IIndexItem item);
    }
}
