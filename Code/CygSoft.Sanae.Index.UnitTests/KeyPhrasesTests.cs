using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygSoft.Sanae.Index.UnitTests
{
    [TestFixture]
    [Category("Tests.UnitTests")]
    [Category("KeywordIndex"), Category("KeywordIndex.KeyPhrases")]
    public class KeyPhrasesTests
    {
        [Test]
        public void KeyPhrases_Ensure_No_Exception_WithEmptyAdd()
        {
            KeyPhrases keyPhrases = new KeyPhrases();
            keyPhrases.AddKeyPhrases(",,");

            Assert.AreEqual(0, keyPhrases.Phrases.Length);
            Assert.IsTrue(!keyPhrases.PhraseExists(""));

            keyPhrases.AddKeyPhrases(" , ");
            Assert.AreEqual(0, keyPhrases.Phrases.Length);
            Assert.IsTrue(!keyPhrases.PhraseExists(""));

        }

        [Test]
        public void KeyPhrases_Ensure_No_Empty_Keyword_Appended()
        {
            KeyPhrases keyPhrases = new KeyPhrases();
            keyPhrases.AddKeyPhrases("yellow,black,");

            Assert.AreEqual(2, keyPhrases.Phrases.Length);
            Assert.AreEqual("BLACK", keyPhrases.Phrases[0]);
            Assert.AreEqual("YELLOW", keyPhrases.Phrases[1]);

            Assert.IsTrue(keyPhrases.PhraseExists("yellow"));
            Assert.IsTrue(keyPhrases.PhraseExists("black"));
            Assert.IsTrue(!keyPhrases.PhraseExists(""));
        }

        [Test]
        public void KeyPhrases_Ensure_No_Empty_Keyword_In_Array()
        {
            KeyPhrases keyPhrases = new KeyPhrases();
            keyPhrases.AddKeyPhrases(new string[] { "yellow", "black", "" });

            Assert.AreEqual(2, keyPhrases.Phrases.Length);
            Assert.AreEqual("BLACK", keyPhrases.Phrases[0]);
            Assert.AreEqual("YELLOW", keyPhrases.Phrases[1]);

            Assert.IsTrue(keyPhrases.PhraseExists("yellow"));
            Assert.IsTrue(keyPhrases.PhraseExists("black"));
            Assert.IsTrue(!keyPhrases.PhraseExists(""));
        }

        [Test]
        public void KeyPhrases_Ensure_No_Empty_Keyword_SavedFromParameter_List()
        {
            KeyPhrases keyPhrases = new KeyPhrases(new List<string> { "yellow", "black", "" });

            Assert.AreEqual(2, keyPhrases.Phrases.Length);
            Assert.AreEqual("BLACK", keyPhrases.Phrases[0]);
            Assert.AreEqual("YELLOW", keyPhrases.Phrases[1]);

            Assert.IsTrue(keyPhrases.PhraseExists("yellow"));
            Assert.IsTrue(keyPhrases.PhraseExists("black"));
            Assert.IsTrue(!keyPhrases.PhraseExists(""));
        }

        [Test]
        public void KeyPhrases_AddingKeywordsWithSpaces_HasSpacesCorrectlyStripped()
        {
            KeyPhrases keyPhrases = new KeyPhrases(new List<string> { " yellow ", "black ", " red" });
            Assert.That(keyPhrases.PhraseExists("yellow"), Is.True);
            Assert.That(keyPhrases.PhraseExists("black"), Is.True);
            Assert.That(keyPhrases.PhraseExists("red"), Is.True);
        }

        [Test]
        public void KeyPhrases_KeyPhrases_SortedAlphabetically()
        {
            KeyPhrases keyPhrases = new KeyPhrases(new List<string> { "yellow", "green", "black", "red" });
            Assert.That(keyPhrases.DelimitKeyPhraseList(), Is.EqualTo("BLACK,GREEN,RED,YELLOW"));
        }
    }
}
