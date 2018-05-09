using System;

namespace CygSoft.Sanae.Index.Infrastructure
{
    public interface IIndexItem
    {
        string Id { get; }
        string Title { get; set; }

        string FileTitle { get; }
        string[] Keywords { get; }
        string[] CategoryPaths { get; }
        string CommaDelimitedKeywords { get; }
        DateTime DateModified { get; }
        DateTime DateCreated { get; }

        void AddCategoryPath(string path);
        void RemoveCategoryPath(string path);

        void AddKeywords(string commaDelimitedKeywords);
        bool AllKeywordsFound(string[] keywords);
        bool ValidateRemoveKeywords(string[] keywords);
        void RemoveKeywords(string[] keywords);
        void SetKeywords(string commaDelimitedKeywords);
        
    }
}
