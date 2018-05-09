using CygSoft.Sanae.Index.Infrastructure;
using System;
using System.Collections.Generic;

namespace CygSoft.Sanae.Index
{
    /// <summary>
    /// A single code index item that points to a single code snippet resource.
    /// </summary>
    public abstract class IndexItem : IIndexItem
    {
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

        public IndexItem()
        {
            this.title = string.Empty;
            this.KeywordsFromDelimitedList(string.Empty);
            this.identifyingGuid = Guid.Empty;
            this.DateCreated = DateTime.Now;
            this.DateModified = this.DateCreated;
        }

        public IndexItem(string id, string title, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion)
        {
            this.DateCreated = dateCreated;
            this.DateModified = dateModified;
            this.identifyingGuid = new Guid(id);
            this.title = title;
            this.KeywordsFromDelimitedList(commaDelimitedKeywords);
        }

        public IndexItem(string title, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion)
            : base()
        {
            this.title = title;
            this.KeywordsFromDelimitedList(commaDelimitedKeywords);
        }

        private KeyPhrases keyPhrases;
        private List<string> categoryPaths = new List<string>();

        public string FileTitle { get { return this.Id + ".xml"; } }

        public string[] Keywords
        {
            get { return this.keyPhrases.Phrases; }
        }

        public string CommaDelimitedKeywords
        {
            get { return this.keyPhrases.DelimitKeyPhraseList(); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; this.DateModified = DateTime.Now; }
        }

        public string[] CategoryPaths => categoryPaths.ToArray();

        public void SetKeywords(string commaDelimitedKeywords)
        {
            this.KeywordsFromDelimitedList(commaDelimitedKeywords);
            this.DateModified = DateTime.Now;
        }

        public void AddKeywords(string commaDelimitedKeywords)
        {
            this.keyPhrases.AddKeyPhrases(commaDelimitedKeywords);
            this.DateModified = DateTime.Now;
        }

        public bool ValidateRemoveKeywords(string[] keywords)
        {
            // the rule is that a searchable item cannot have an empty keyword list!

            KeyPhrases phrases = new KeyPhrases(this.CommaDelimitedKeywords);
            phrases.RemovePhrases(keywords);

            return phrases.Phrases.Length > 0;
        }

        public void RemoveKeywords(string[] keywords)
        {
            this.keyPhrases.RemovePhrases(keywords);
            this.DateModified = DateTime.Now;
        }

        public bool AllKeywordsFound(string[] keywords)
        {
            return keyPhrases.AllPhrasesExist(keywords);
        }

        protected void KeywordsFromDelimitedList(string commaDelimitedKeywords)
        {
            this.keyPhrases = new KeyPhrases(commaDelimitedKeywords);
        }

        public void AddCategoryPath(string path)
        {
            if (!categoryPaths.Exists(p => p == path))
                categoryPaths.Add(path);
        }

        public void RemoveCategoryPath(string path)
        {
            categoryPaths.Remove(path);
        }
    }
}
