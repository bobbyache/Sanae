using NUnit.Framework;
using CygSoft.Sanae.Index.UnitTests.Helpers;
using System;
using CygSoft.Sanae.Index.Infrastructure;

namespace CygSoft.Sanae.Index.UnitTests
{
    [TestFixture]
    [Category("Tests.UnitTests")]
    [Category("KeywordIndex"), Category("KeywordIndex.KeywordIndexItem")]
    class ProjectIndexItemTests
    {
        [Test]
        public void ProjectIndexItem_OnInitializedWithoutKeywords_ReturnsFalseForAllSearches()
        {
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "", new string[0], "Awe34Dr", "1.0.0.0");

            bool foundSingle = keywordIndexItem.AllKeywordsFound(new string[] { "single" });
            bool emptyStringFound = keywordIndexItem.AllKeywordsFound(new string[] { "" });

            Assert.That(foundSingle, Is.False);
            Assert.That(emptyStringFound, Is.False);
        }

        [Test]
        public void ProjectIndexItem_OnInitializedWithKeywords_ReturnsFalseIfNotFound()
        {
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "test,tested", new string[0], "Awe34Dr", "1.0.0.0");

            bool foundSingle = keywordIndexItem.AllKeywordsFound(new string[] { "testing" });
            bool emptyStringFound = keywordIndexItem.AllKeywordsFound(new string[] { "tester", "testing" });

            Assert.That(foundSingle, Is.False);
            Assert.That(emptyStringFound, Is.False);
        }

        [Test]
        public void ProjectIndexItem_OnInitializedWithKeywords_ReturnsTrueIfFound()
        {
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "test,tested", new string[0], "Awe34Dr", "1.0.0.0");

            bool foundSingle = keywordIndexItem.AllKeywordsFound(new string[] { "test" });
            bool foundAll = keywordIndexItem.AllKeywordsFound(new string[] { "test", "tested" });

            Assert.That(foundSingle, Is.True);
            Assert.That(foundAll, Is.True);
        }

        [Test]
        public void ProjectIndexItem_OnInitializedWithKeywords_ReturnsFalseIfSomeFound()
        {
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "test,tested", new string[0], "Awe34Dr", "1.0.0.0");
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "test", "testing" });
            Assert.That(found, Is.False);
        }

        [Test]
        public void ProjectIndexItem_AllKeywordsFound_ReturnsTrueIfExistRegardlessOfCase()
        {
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "test,tested", new string[0], "Awe34Dr", "1.0.0.0");
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "TEST", "tested" });
            Assert.That(found, Is.True);
        }

        [Test]
        public void ProjectIndexItem_AddKeywordsAndSubsequentSearch_ReturnsTrueIfExistRegardlessOfCase()
        {
            
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "apple,pear", new string[0], "Awe34Dr", "1.0.0.0");
            keywordIndexItem.AddKeywords("banana,orange");
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "apple", "BANANA", "Orange", "pear" });
            Assert.That(found, Is.True);
        }

        [Test]
        public void ProjectIndexItem_RemoveKeywordsAndSubsequentSearch_ReturnsFalseIfSomeDoNotExist()
        {

            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "apple,pear,banana,orange", new string[0], "Awe34Dr", "1.0.0.0");
            keywordIndexItem.RemoveKeywords(new string[] { "banana", "orange" });
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "apple", "BANANA", "Orange", "pear" });

            Assert.That(found, Is.False);
        }

        [Test]
        public void ProjectIndexItem_SetKeywords_ResetsKeywords()
        {

            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "apple,pear,banana,orange", new string[0], "Awe34Dr", "1.0.0.0");
            keywordIndexItem.SetKeywords("banana,orange");
            bool found = keywordIndexItem.AllKeywordsFound(new string[] { "BANANA", "Orange"});
            bool pearNotFound = keywordIndexItem.AllKeywordsFound(new string[] { "pear" });
            bool appleNotFound = keywordIndexItem.AllKeywordsFound(new string[] { "apple" });

            Assert.That(found, Is.True);
            Assert.That(pearNotFound, Is.False);
            Assert.That(appleNotFound, Is.False);
        }


        [Test]
        public void ProjectIndexItem_ValidateRemoveKeywords_IsInvalidIfAllKeywordsRemoved()
        {
            // better to change from ValidateRemoveKeywords() to IsSearchableAfterRemove()
            // perhaps add property "IsSearchable" or "HasKeywords" or both....
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "", new string[0], "Awe34Dr", "1.0.0.0");
            keywordIndexItem.SetKeywords("banana,orange");
            bool willResultNonSearchhableIndexItem = !keywordIndexItem.ValidateRemoveKeywords(new string[] { "banana", "orange" });

            Assert.IsTrue(willResultNonSearchhableIndexItem);
        }

        [Test]
        public void ProjectIndexItem_ValidateRemoveKeywords_IsValidIfSomeKeywordsRemain()
        {
            // better to change from ValidateRemoveKeywords() to IsSearchableAfterRemove()
            // perhaps add property "IsSearchable" or "HasKeywords" or both....
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "", new string[0], "Awe34Dr", "1.0.0.0");
            keywordIndexItem.SetKeywords("banana,orange");
            bool willResultNonSearchhableIndexItem = !keywordIndexItem.ValidateRemoveKeywords(new string[] { "banana" });
            Assert.IsFalse(willResultNonSearchhableIndexItem);
        }

        [Test]
        public void ProjectIndexItem_AfterAddingIndexItems_ContainsOnlyUniqueKeywords()
        {
            // better to change from ValidateRemoveKeywords() to IsSearchableAfterRemove()
            // perhaps add property "IsSearchable" or "HasKeywords" or both....
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Title", DateTime.Now, DateTime.Now, "", new string[0], "Awe34Dr", "1.0.0.0");
            keywordIndexItem.AddKeywords("banana,orange");
            keywordIndexItem.AddKeywords("banana,ORANGE");
            keywordIndexItem.AddKeywords("apple,BANANA");

            int keywordCount = keywordIndexItem.Keywords.Length;
            Assert.That(keywordCount, Is.EqualTo(3));
        }

        [Test]
        public void ProjectIndexItem_InitializedWithParameterlessConstructor_ReturnsNewIdString()
        {
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("Title", "", new string[0], "", "");
            Guid guid = new Guid(keywordIndexItem.Id);
            Assert.That(guid == Guid.Empty, Is.False);
        }

        [Test]
        public void ProjectIndexItem_InitializedWithParameteredConstructor_ReturnsPassInIdString()
        {
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Test Title", DateTime.Now, DateTime.Now, "test,testing", new string[0], "Awe34Dr", "1.0.0.0");
            string id = keywordIndexItem.Id;
            Assert.That(id, Is.EqualTo("4ecac722-8ec5-441c-8e3e-00b192b30453"));
        }

        [Test]
        public void ProjectIndexItem_InitializedWithParameteredConstructorNoId_ReturnsNewIdString()
        {
            IProjectIndexItem keywordIndexItem = new TestProjectIndexItem("Test Title", "test, testing", new string[0], "Awe34Dr", "1.0.0.0");
            Guid guid = new Guid(keywordIndexItem.Id);
            Assert.That(guid == Guid.Empty, Is.False);
        }

        [Test]
        public void ProjectIndexItem_AddCategoryPaths_AddsPaths_Successfully()
        {
            IProjectIndexItem indexItem = new TestProjectIndexItem("Test Title", "test, testing", new string[0], "Awe34Dr", "1.0.0.0");
            indexItem.AddCategoryPath("Project/Module/WorkItem1");
            indexItem.AddCategoryPath("Project/Module/WorkItem2");

            Assert.AreEqual(2, indexItem.CategoryPaths.Length);
        }

        [Test]
        public void ProjectIndexItem_AddDuplicateCategoryPath_DoesNotDuplicate()
        {
            IProjectIndexItem indexItem = new TestProjectIndexItem("Test Title", "test, testing", new string[0], "Awe34Dr", "1.0.0.0");
            indexItem.AddCategoryPath("Project/Module/WorkItem");
            indexItem.AddCategoryPath("Project/Module/WorkItem");

            Assert.AreEqual(1, indexItem.CategoryPaths.Length);
        }

        [Test]
        public void ProjectIndexItem_RemoveCategoryPath_RemovesCategoryPath_Successfully()
        {
            IProjectIndexItem indexItem = new TestProjectIndexItem("Test Title", "test, testing", new string[0], "Awe34Dr", "1.0.0.0");
            indexItem.AddCategoryPath("Project/Module/WorkItem1");
            indexItem.AddCategoryPath("Project/Module/WorkItem2");

            indexItem.RemoveCategoryPath("Project/Module/WorkItem1");

            Assert.AreEqual(1, indexItem.CategoryPaths.Length);
            Assert.AreEqual("Project/Module/WorkItem2", indexItem.CategoryPaths[0]);
        }

        [Test]
        public void ProjectIndexItem_RemoveNonExistentCategoryPath_DoesNothingWithNoException()
        {
            IProjectIndexItem indexItem = new TestProjectIndexItem("Test Title", "test, testing", new string[0], "Awe34Dr", "1.0.0.0");
            indexItem.AddCategoryPath("Project/Module/WorkItem1");
            indexItem.AddCategoryPath("Project/Module/WorkItem2");
            indexItem.RemoveCategoryPath("Project/Module/WorkItem3");

            Assert.AreEqual(2, indexItem.CategoryPaths.Length);
        }

        [Test]
        public void ProjectIndexItem_SetsPluginProperties_When_Initialized()
        {
            IProjectIndexItem indexItem1 = new TestProjectIndexItem("Test Title", "test, testing", new string[0], "Awe34Dr", "1.0.0.0");

            IProjectIndexItem indexItem2 = new TestProjectIndexItem("2d4421df-2b88-470f-b9e8-55af9ccb760d", 
                "Test Title", DateTime.Now, DateTime.Now, "test, testing", new string[0], "Awe34Br", "2.0.0.0");

            Assert.AreEqual("Awe34Dr", indexItem1.PluginId);
            Assert.AreEqual("1.0.0.0", indexItem1.PluginVersion);

            Assert.AreEqual("Awe34Br", indexItem2.PluginId);
            Assert.AreEqual("2.0.0.0", indexItem2.PluginVersion);
        }
    }
}
