using CygSoft.Sanae.Index.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CygSoft.Sanae.Index
{
    /// <summary>
    /// A lookup of index items that point to code files.
    /// </summary>
    public class KeywordSearchIndex : IKeywordSearchIndex
    {
        /// <summary>
        /// When the code index has been modified in any way, this event will fire which will
        /// enable the application to respond by for instance, saving the index to the index file.
        /// </summary>
        public event EventHandler IndexModified;

        private List<IKeywordIndexItem> IndexItems;
        private KeyPhrases keyPhrases = new KeyPhrases();

        public bool FindAllForEmptySearch { get; set; }

        public Version CurrentVersion { get; private set; }

        public string LibraryFolderPath { get; private set; }

        internal KeywordSearchIndex(string filePath, Version currentVersion, List<IKeywordIndexItem> IndexItems)
        {
            this.FilePath = filePath;

            this.FindAllForEmptySearch = true;
            this.IndexItems = IndexItems;
            this.CurrentVersion = currentVersion;
            CreateKeywordIndex();
        }

        internal KeywordSearchIndex(string filePath, Version currentVersion)
        {
            this.FilePath = filePath;
            this.CurrentVersion = currentVersion;
            this.IndexItems = new List<IKeywordIndexItem>();
        }

        public IKeywordIndexItem[] All()
        {
            return IndexItems.ToArray();
        }

        public string FilePath { get; private set; }
        public string FileTitle { get { return Path.GetFileName(this.FilePath); } }
        public string FolderPath { get { return Path.GetDirectoryName(this.FilePath); } }

        public string[] Keywords { get { return this.keyPhrases.Phrases; } }

        public IKeywordIndexItem[] FindByIds(string[] ids)
        {
            var result = from ix in IndexItems
                         join ia in ids on ix.Id equals ia
                         select ix;

            return result.ToArray();
        }

        public IKeywordIndexItem FindById(string id)
        {
            if (this.IndexItems.Any(r => r.Id == id))
                return this.IndexItems.Where(r => r.Id == id).SingleOrDefault();
            return null;
        }

        public IKeywordIndexItem[] Find(string commaDelimitedKeywords)
        {
            List<IKeywordIndexItem> foundItemList = new List<IKeywordIndexItem>();

            if (!string.IsNullOrWhiteSpace(commaDelimitedKeywords))
            {

                KeyPhrases keys = new KeyPhrases(commaDelimitedKeywords);

                foreach (IKeywordIndexItem item in IndexItems)
                {
                    if (item.AllKeywordsFound(keys.Phrases))
                        foundItemList.Add(item);
                }
            }
            else
            {
                if (this.FindAllForEmptySearch)
                {
                    foundItemList.AddRange(this.All());
                }
            }
            return foundItemList.ToArray();
        }

        public int ItemCount { get { return this.IndexItems.Count; } }

        public bool Contains(IKeywordIndexItem item)
        {
            return this.IndexItems.Any(r => r.Id == item.Id);
        }

        public bool Contains(string id)
        {
            return this.IndexItems.Any(r => r.Id == id);
        }

        /// <summary>
        /// A single item has been  changed. This will be used when a code index items is added.
        /// </summary>
        /// <param name="item"></param>
        public void Update(IKeywordIndexItem item)
        {
            if (!Contains(item))
                Add(item);
            else
                CreateKeywordIndex();

            IndexModified?.Invoke(this, new EventArgs());
        }

        public string[] AllKeywords(IKeywordIndexItem[] indeces)
        {
            KeyPhrases phrases = new KeyPhrases();
            foreach (IKeywordIndexItem index in indeces)
            {
                phrases.AddKeyPhrases(index.Keywords);
            }
            return phrases.Phrases;
        }

        public string CopyAllKeywords(IKeywordIndexItem[] indeces)
        {
            KeyPhrases phrases = new KeyPhrases();
            foreach (IKeywordIndexItem index in indeces)
            {
                phrases.AddKeyPhrases(index.Keywords);
            }
            return phrases.DelimitKeyPhraseList();
        }

        public void AddKeywords(IKeywordIndexItem[] indeces, string delimitedKeywordList)
        {
            foreach (IKeywordIndexItem index in indeces)
            {
                index.AddKeywords(delimitedKeywordList);
            }
            IndexModified?.Invoke(this, new EventArgs());
        }

        public void RemoveKeywords(IKeywordIndexItem[] indeces, string[] keywords)
        {
            // the rule is that a searchable item cannot have an empty keyword list!

            foreach (IKeywordIndexItem index in indeces)
            {
                index.RemoveKeywords(keywords);
            }
            IndexModified?.Invoke(this, new EventArgs());
        }

        public bool IndexesExistFor(IKeywordIndexItem[] indeces, out IKeywordIndexItem[] existingIndeces)
        {
            IEnumerable<IKeywordIndexItem> foundItems = indeces.Join(this.IndexItems, a => a.Id, b => b.Id, 
                (a, b) => b);

            existingIndeces = foundItems.ToArray();

            return existingIndeces.Count() > 0;
        }

        public bool ValidateRemoveKeywords(IKeywordIndexItem[] indeces, string[] keywords, out IKeywordIndexItem[] invalidIndeces)
        {
            bool valid = true;

            List<IKeywordIndexItem> invalidItems = new List<IKeywordIndexItem>();
            foreach (IKeywordIndexItem index in indeces)
            {
                bool validated = index.ValidateRemoveKeywords(keywords);
                if (!validated)
                {
                    invalidItems.Add(index);
                    valid = false;
                }
            }
            invalidIndeces = invalidItems.ToArray();

            return valid;
        }

        private void Add(IKeywordIndexItem item)
        {
            if (Contains(item))
                throw new ApplicationException("This id already exists within the index. Cannot add a duplicate id to the index.");

            this.IndexItems.Add(item);
            CreateKeywordIndex();

        }

        public void Remove(string id)
        {
            IKeywordIndexItem item = this.IndexItems.Where(cd => cd.Id == id).SingleOrDefault();
            this.IndexItems.Remove(item);
            CreateKeywordIndex();

            IndexModified?.Invoke(this, new EventArgs());
        }

        private void CreateKeywordIndex()
        {
            keyPhrases = new KeyPhrases();
            foreach (IKeywordIndexItem IndexItem in IndexItems)
            {
                keyPhrases.AddKeyPhrases(IndexItem.Keywords);
            }
        }
    }
}
