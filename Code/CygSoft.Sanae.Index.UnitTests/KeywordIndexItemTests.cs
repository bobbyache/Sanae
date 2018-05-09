using NUnit.Framework;
using CygSoft.Sanae.Index.UnitTests.Helpers;
using System;
using CygSoft.Sanae.Index.Infrastructure;

namespace CygSoft.Sanae.Index.UnitTests
{
    [TestFixture]
    [Category("Tests.UnitTests")]
    [Category("KeywordIndex"), Category("KeywordIndex.KeywordIndexItem")]
    class KeywordIndexItemTests
    {
        [Test]
        public void IndexItem_OnInitializedWithoutKeywords_ReturnsFalseForAllSearches()
        {
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "");

            bool foundSingle = keywordIndexItem.AllKeywordsFound(new string[] { "single" });
            bool emptyStringFound = keywordIndexItem.AllKeywordsFound(new string[] { "" });

            Assert.That(foundSingle, Is.False);
            Assert.That(emptyStringFound, Is.False);
        }

        [Test]
        public void IndexItem_OnInitializedWithKeywords_ReturnsFalseIfNotFound()
        {
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "test,tested");

            bool foundSingle = keywordIndexItem.AllKeywordsFound(new string[] { "testing" });
            bool emptyStringFound = keywordIndexItem.AllKeywordsFound(new string[] { "tester", "testing" });

            Assert.That(foundSingle, Is.False);
            Assert.That(emptyStringFound, Is.False);
        }

        [Test]
        public void IndexItem_OnInitializedWithKeywords_ReturnsTrueIfFound()
        {
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "test,tested");

            bool foundSingle = keywordIndexItem.AllKeywordsFound(new string[] { "test" });
            bool foundAll = keywordIndexItem.AllKeywordsFound(new string[] { "test", "tested" });

            Assert.That(foundSingle, Is.True);
            Assert.That(foundAll, Is.True);
        }

        [Test]
        public void IndexItem_OnInitializedWithKeywords_ReturnsFalseIfSomeFound()
        {
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "test,tested");
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "test", "testing" });
            Assert.That(found, Is.False);
        }

        [Test]
        public void IndexItem_AllKeywordsFound_ReturnsTrueIfExistRegardlessOfCase()
        {
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "test,tested");
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "TEST", "tested" });
            Assert.That(found, Is.True);
        }

        [Test]
        public void IndexItem_AddKeywordsAndSubsequentSearch_ReturnsTrueIfExistRegardlessOfCase()
        {
            
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "apple,pear");
            keywordIndexItem.AddKeywords("banana,orange");
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "apple", "BANANA", "Orange", "pear" });
            Assert.That(found, Is.True);
        }

        [Test]
        public void IndexItem_RemoveKeywordsAndSubsequentSearch_ReturnsFalseIfSomeDoNotExist()
        {

            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "apple,pear,banana,orange");
            keywordIndexItem.RemoveKeywords(new string[] { "banana", "orange" });
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "apple", "BANANA", "Orange", "pear" });

            Assert.That(found, Is.False);
        }

        [Test]
        public void IndexItem_SetKeywords_ResetsKeywords()
        {

            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "apple,pear,banana,orange");
            keywordIndexItem.SetKeywords("banana,orange");
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "BANANA", "Orange"});
            bool pearNotFound = keywordIndexItem.AllKeywordsFound(new string[] { "pear" });
            bool appleNotFound = keywordIndexItem.AllKeywordsFound(new string[] { "apple" });

            Assert.That(found, Is.True);
            Assert.That(pearNotFound, Is.False);
            Assert.That(appleNotFound, Is.False);
        }


        [Test]
        public void IndexItem_ValidateRemoveKeywords_IsInvalidIfAllKeywordsRemoved()
        {
            // better to change from ValidateRemoveKeywords() to IsSearchableAfterRemove()
            // perhaps add property "IsSearchable" or "HasKeywords" or both....
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "");
            keywordIndexItem.SetKeywords("banana,orange");
            bool willResultNonSearchhableIndexItem = !keywordIndexItem.ValidateRemoveKeywords(new string[] { "banana", "orange" });

            Assert.IsTrue(willResultNonSearchhableIndexItem);
        }

        [Test]
        public void IndexItem_ValidateRemoveKeywords_IsValidIfSomeKeywordsRemain()
        {
            // better to change from ValidateRemoveKeywords() to IsSearchableAfterRemove()
            // perhaps add property "IsSearchable" or "HasKeywords" or both....
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "");
            keywordIndexItem.SetKeywords("banana,orange");
            bool willResultNonSearchhableIndexItem = !keywordIndexItem.ValidateRemoveKeywords(new string[] { "banana" });
            Assert.IsFalse(willResultNonSearchhableIndexItem);
        }

        [Test]
        public void IndexItem_AfterAddingIndexItems_ContainsOnlyUniqueKeywords()
        {
            // better to change from ValidateRemoveKeywords() to IsSearchableAfterRemove()
            // perhaps add property "IsSearchable" or "HasKeywords" or both....
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "");
            keywordIndexItem.AddKeywords("banana,orange");
            keywordIndexItem.AddKeywords("banana,ORANGE");
            keywordIndexItem.AddKeywords("apple,BANANA");

            int keywordCount = keywordIndexItem.Keywords.Length;
            Assert.That(keywordCount, Is.EqualTo(3));
        }

        [Test]
        public void IndexItem_InitializedWithParameterlessConstructor_ReturnsNewIdString()
        {
            IIndexItem keywordIndexItem = new TestKeywordIndexItem();
            Guid guid = new Guid(keywordIndexItem.Id);
            Assert.That(guid == Guid.Empty, Is.False);
        }

        [Test]
        public void IndexItem_InitializedWithParameteredConstructor_ReturnsPassInIdString()
        {
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Test Title", DateTime.Now, DateTime.Now, "test,testing");
            string id = keywordIndexItem.Id;
            Assert.That(id, Is.EqualTo("4ecac722-8ec5-441c-8e3e-00b192b30453"));
        }

        [Test]
        public void IndexItem_InitializedWithParameteredConstructorNoId_ReturnsNewIdString()
        {
            IIndexItem keywordIndexItem = new TestKeywordIndexItem("Test Title", "test, testing");
            Guid guid = new Guid(keywordIndexItem.Id);
            Assert.That(guid == Guid.Empty, Is.False);
        }
    }
}
