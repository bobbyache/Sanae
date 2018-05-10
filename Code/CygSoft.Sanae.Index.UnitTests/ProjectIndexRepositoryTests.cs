﻿using CygSoft.Sanae.Index.Infrastructure;
using CygSoft.Sanae.Index.UnitTests.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CygSoft.Sanae.Index.UnitTests
{
    [TestFixture]
    [Category("Tests.UnitTests")]
    [Category("KeywordIndex"), Category("KeywordIndex.XmlKeywordSearchIndexRepository")]
    class ProjectIndexRepositoryTests
    {
        // Replacing the ExpectedExceptionAttribute with Throws.InstanceOf
        // http://jamesnewkirk.typepad.com/posts/2008/06/replacing-expec.html

        [Test]
        public void ProjectIndexRepository_Save_SavesCorrectXml()
        {
            TestXmlProjectIndexItem item = new TestXmlProjectIndexItem(
                "4ecac722-8ec5-441c-8e3e-00b192b30453",
                "Components and Libraries",
                DateTime.Parse("2018/02/28 18:00:00"),
                DateTime.Parse("2018/02/28 19:00:00"),
                "TEST,TEST", 
                new string[] { "2017/05", "SPC/Components/Enquiries/History" },
                "Awe34Dr", 
                "1.0.0.0"
            );
            //item.AddCategoryPath("2017/05");
            //item.AddCategoryPath("SPC/Components/Enquiries/History");

            Index index = new Index(TxtFile.ResolvePath("LoadSingleProjectIndexItem.xml"), "4.1.0.0", new List<IProjectIndexItem> { item });
            TestXmlProjectIndexRepository repository = new TestXmlProjectIndexRepository("Index", (s1) => true, (s1, s2) => true);
            repository.SaveIndex(index);
            Assert.AreEqual(repository.LastSavedXml, TxtFile.ReadText("LoadSingleProjectIndexItem.xml"), "Expected that saved xml matches the expected xml. Xml does not match.");
        }

        [Test]
        public void ProjectIndexRepository_OpeningNonExistingFile_ThrowsFileNotFoundException()
        {
            TestXmlProjectIndexRepository repository = new TestXmlProjectIndexRepository("RootElement", (s1) => true, (s1, s2) => true);
            repository.FudgeIndexExists = false;

            Assert.That(() => repository.OpenIndex("", "2.0.0.0"), Throws.InstanceOf(typeof(FileNotFoundException)),
                "Stub method simulated that an Index file was not found at the path specified. Repository should have thrown a FileNotFoundException.");
        }

        [Test]
        public void ProjectIndexRepository_OpeningIncorrectFormat_ThrowsArgumentException()
        {
            TestXmlProjectIndexRepository repository = new TestXmlProjectIndexRepository("RootElement", (s1) => false, (s1, s2) => true);

            Assert.That(() => repository.OpenIndex("", "2.0.0.0"), Throws.InstanceOf(typeof(InvalidDataException)),
                "Stub method simulated that an Index file format was invalid but a InvalidDataException was not thrown.");
        }

        [Test]
        public void ProjectIndexRepository_OpeningIncorrectVersion_ThrowsVersionException()
        {
            TestXmlProjectIndexRepository repository = new TestXmlProjectIndexRepository("RootElement", (s1) => true, (s1, s2) => false);

            Assert.That(() => repository.OpenIndex("", "2.0.0.0"), Throws.InstanceOf(typeof(InvalidFileIndexVersionException)));
        }

        [Test]
        public void ProjectIndexRepository_CloneIndex_ClonesCorrectly()
        {
            var indexItems = new IProjectIndexItem[]
            {
                new TestProjectIndexItem("Item 1", "green,blue", new string[0], "Awe34Dr", "1.0.0.0"),
                new TestProjectIndexItem("Item 1", "green,red", new string[0], "Awe34Dr", "1.0.0.0"),
                new TestProjectIndexItem("Item 1", "yellow,gray", new string[0], "Awe34Dr", "1.0.0.0")
            };

            var stubKeywordSearchIndex = new Mock<IIndex>();
            stubKeywordSearchIndex.Setup(m => m.All()).Returns(indexItems);
            stubKeywordSearchIndex.Setup(m => m.CurrentVersion).Returns(new Version(2, 0));

            var keywordSearchIndex = new Index("C:File.xml", "2.0.0.0");
 
            TestXmlProjectIndexRepository repository = new TestXmlProjectIndexRepository("RootElement", (s1) => true, (s1, s2) => true);
            IIndex newSearchIndex = repository.CloneIndex(stubKeywordSearchIndex.Object, @"C:\hello_world.txt");

            Assert.That(newSearchIndex, Is.Not.SameAs(stubKeywordSearchIndex.Object));
            Assert.That(newSearchIndex.ItemCount, Is.EqualTo(3));
            Assert.That(newSearchIndex.Contains(indexItems[0]), Is.True);
            Assert.That(newSearchIndex.Contains(indexItems[1]), Is.True);
            Assert.That(newSearchIndex.Contains(indexItems[2]), Is.True);
        }

        class TestXmlProjectIndexRepository : XmlIndexRepository<TestXmlProjectIndexItem>
        {
            public bool FudgeIndexExists = true;
            public string LastSavedXml = "";

            public TestXmlProjectIndexRepository(string rootElement, Func<string, bool> formatChecker, Func<string, string, bool> versionChecker) 
                : base(rootElement, formatChecker, versionChecker)
            {
            }

            protected override List<TestXmlProjectIndexItem> LoadIndexItems(string filePath, string currentVersion)
            {
                return new List<TestXmlProjectIndexItem>();
            }

            protected override bool IndexExists(string filePath)
            {
                return FudgeIndexExists ? true : false;
            }

            protected override void SaveFile(string fileText, string filePath)
            {
                LastSavedXml = fileText;
            }

            protected override string LoadFile(string filePath)
            {
                return TxtFile.ReadText("LoadSingleIndexItem.xml");
            }
        }

        class TestXmlProjectIndexItem : XmlIndexItem
        {
            public TestXmlProjectIndexItem() : base("Title", "", new string[0], "", "")
            {
            }

            public TestXmlProjectIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified, 
                string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion)
            : base(id, title, dateCreated, dateModified, commaDelimitedKeywords, categoryPaths, pluginId, pluginVersion)
            {
            }

            public TestXmlProjectIndexItem(string title, string commaDelimitedKeywords, string[] categoryPaths, string pluginId, string pluginVersion)
            : base(title, commaDelimitedKeywords, categoryPaths, pluginId, pluginVersion)
            {
            }
        }
    }
}
