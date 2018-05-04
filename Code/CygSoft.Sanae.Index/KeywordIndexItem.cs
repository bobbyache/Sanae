using CygSoft.Sanae.Index.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index
{
    /// <summary>
    /// A single code index item that points to a single code snippet resource.
    /// </summary>
    public abstract class KeywordIndexItem : PersistableObject, IKeywordIndexItem
    {
        public KeywordIndexItem()
        {
            this.title = string.Empty;
            this.KeywordsFromDelimitedList(string.Empty);
        }

        public KeywordIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords)
            : base(id, dateCreated, dateModified)
        {
            this.title = title;
            this.KeywordsFromDelimitedList(commaDelimitedKeywords);
        }

        public KeywordIndexItem(string title, string commaDelimitedKeywords)
            : base()
        {
            this.title = title;
            this.KeywordsFromDelimitedList(commaDelimitedKeywords);
        }

        private KeyPhrases keyPhrases;

        public string FileTitle { get { return base.Id + ".xml"; } }

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
    }
}
