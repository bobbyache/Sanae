using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface IIndex
    {
        void AddKeywords(IProjectIndexItem[] indeces, string delimitedKeywordList);
        IProjectIndexItem[] All();
        string[] AllKeywords(IProjectIndexItem[] indeces);
        bool Contains(IProjectIndexItem item);
        bool Contains(string id);
        string CopyAllKeywords(IProjectIndexItem[] indeces);
        Version CurrentVersion { get; }
        string FilePath { get; }
        string FileTitle { get; }
        IProjectIndexItem[] Find(string commaDelimitedKeywords);
        bool FindAllForEmptySearch { get; set; }
        IProjectIndexItem FindById(string id);
        IProjectIndexItem[] FindByIds(string[] ids);
        string FolderPath { get; }
        event EventHandler IndexModified;
        int ItemCount { get; }
        string[] Keywords { get; }
        string LibraryFolderPath { get; }
        void Remove(string id);
        bool IndexesExistFor(IProjectIndexItem[] indeces, out IProjectIndexItem[] existingIndeces);
        bool ValidateRemoveKeywords(IProjectIndexItem[] indeces, string[] keywords, out IProjectIndexItem[] invalidEmptyItems);
        void RemoveKeywords(IProjectIndexItem[] indeces, string[] keywords);
        void Update(IProjectIndexItem item);
    }
}
