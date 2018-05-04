using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface IKeywordSearchIndex
    {
        void AddKeywords(IKeywordIndexItem[] indeces, string delimitedKeywordList);
        IKeywordIndexItem[] All();
        string[] AllKeywords(IKeywordIndexItem[] indeces);
        bool Contains(IKeywordIndexItem item);
        bool Contains(string id);
        string CopyAllKeywords(IKeywordIndexItem[] indeces);
        Version CurrentVersion { get; }
        string FilePath { get; }
        string FileTitle { get; }
        IKeywordIndexItem[] Find(string commaDelimitedKeywords);
        bool FindAllForEmptySearch { get; set; }
        IKeywordIndexItem FindById(string id);
        IKeywordIndexItem[] FindByIds(string[] ids);
        string FolderPath { get; }
        event EventHandler IndexModified;
        int ItemCount { get; }
        string[] Keywords { get; }
        string LibraryFolderPath { get; }
        void Remove(string id);
        bool IndexesExistFor(IKeywordIndexItem[] indeces, out IKeywordIndexItem[] existingIndeces);
        bool ValidateRemoveKeywords(IKeywordIndexItem[] indeces, string[] keywords, out IKeywordIndexItem[] invalidEmptyItems);
        void RemoveKeywords(IKeywordIndexItem[] indeces, string[] keywords);
        void Update(IKeywordIndexItem item);
    }
}
