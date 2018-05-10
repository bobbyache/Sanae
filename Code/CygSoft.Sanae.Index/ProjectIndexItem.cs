using CygSoft.Sanae.Index.Infrastructure;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CygSoft.Sanae.Index
{
    /// <summary>
    /// A single code index item that points to a single code snippet resource.
    /// </summary>
    public abstract class ProjectIndexItem : SerializableIndexItem, IProjectIndexItem
    {
        public ProjectIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords, 
            string[] categoryPaths, string pluginId, string pluginVersion) : base(id, title, dateCreated, dateModified)
        {
            this.KeywordsFromDelimitedList(commaDelimitedKeywords);
            this.PluginId = pluginId;
            this.PluginVersion = pluginVersion;
        }

        public ProjectIndexItem(string title, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion)
            : base(title)
        {
            this.KeywordsFromDelimitedList(commaDelimitedKeywords);
            this.PluginId = pluginId;
            this.PluginVersion = pluginVersion;
        }

        public string PluginId { get; private set; }
        public string PluginVersion { get; private set; }

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

        public override XElement Serialize()
        {
            return base.Serialize();
        }
    }
}
