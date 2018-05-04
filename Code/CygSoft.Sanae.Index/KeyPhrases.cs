using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index
{
    public class KeyPhrases
    {
        private List<string> keyPhrases = new List<string>();

        public KeyPhrases()
        {
            // no key phrases at start up.
        }

        public KeyPhrases(string delimitedKeyPhrases)
        {
            this.keyPhrases = SplitKeyPhrases(delimitedKeyPhrases);
        }

        public KeyPhrases(List<string> keyPhrases)
        {
            this.keyPhrases = keyPhrases.Select(s => s.Trim().ToUpper()).Where(s => !string.IsNullOrEmpty(s)).ToList(); ;
        }

        public bool PhraseExists(string phrase)
        {
            if (keyPhrases.Contains(phrase.Trim().ToUpper()))
                return true;
            return false;
        }

        public bool AllPhrasesExist(string[] phrases)
        {
            foreach (string phrase in phrases)
            {
                if (!PhraseExists(phrase))
                    return false;
            }
            return true;
        }

        public bool Exist
        {
            get
            {
                if (this.keyPhrases.Count == 0)
                    return false;
                else
                {
                    var results = keyPhrases.Where(k => !string.IsNullOrEmpty(k));
                    return (results.Count() > 0);
                }
            }
        }

        public string[] Phrases
        {
            get
            {
                return keyPhrases.Where(k => !string.IsNullOrEmpty(k)).OrderBy(k => k).ToArray();
            }
        }

        public string DelimitKeyPhraseList()
        {
            return string.Join(",", keyPhrases.OrderBy(k => k).ToArray());
        }

        public void AddKeyPhrases(string[] keyPhraseList)
        {
            foreach (string keyphrase in keyPhraseList)
            {
                string trimmed = keyphrase.Trim();
                if (!string.IsNullOrEmpty(trimmed) && !keyPhrases.Contains(trimmed.ToUpper()))
                    keyPhrases.Add(trimmed.ToUpper());
            }
        }

        public void AddKeyPhrases(string keyPhraseDelimitedList)
        {
            AddKeyPhrases(
                SplitKeyPhrases(keyPhraseDelimitedList).ToArray());
        }

        public void RemovePhrases(string[] keyPhraseList)
        {
            foreach (string keyphrase in keyPhraseList)
            {
                string trimmedUpper = keyphrase.Trim().ToUpper();

                if (keyPhrases.Contains(trimmedUpper))
                    keyPhrases.Remove(trimmedUpper);
            }
        }

        public void RemovePhrases(string keyPhraseDelimitedList)
        {
            List<string> phrases = SplitKeyPhrases(keyPhraseDelimitedList);
            RemovePhrases(phrases.ToArray());
        }

        private List<string> SplitKeyPhrases(string keyPhrases)
        {
            string[] phrases = keyPhrases.Split(new char[] { ',' });
            var ph = phrases.Select(p => p.ToUpper().Trim()).Where(p => !string.IsNullOrEmpty(p)).Distinct();

            return ph.OrderBy(k => k).ToList();
        }
    }
}
