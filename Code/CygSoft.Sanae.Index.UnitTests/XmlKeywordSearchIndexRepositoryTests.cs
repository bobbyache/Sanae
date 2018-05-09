using CygSoft.Sanae.Index.Infrastructure;
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
    class XmlKeywordSearchIndexRepositoryTests
    {
        // Replacing the ExpectedExceptionAttribute with Throws.InstanceOf
        // http://jamesnewkirk.typepad.com/posts/2008/06/replacing-expec.html

        [Test]
        public void XmlIndexRepository_OpeningNonExistingFile_ThrowsFileNotFoundException()
        {
            StubIndexFileFunctions stubFile = new StubIndexFileFunctions();
            stubFile.IsExistingFile = false;

            TestXmlKeywordSearchIndexRepository repository = new TestXmlKeywordSearchIndexRepository("RootElement", stubFile);

            Assert.That(() => repository.OpenIndex("", new Version(2,0)), Throws.InstanceOf(typeof(FileNotFoundException)),
                "Stub method simulated that an Index file was not found at the path specified. Repository should have thrown a FileNotFoundException.");
        }

        [Test]
        public void XmlIndexRepository_OpeningIncorrectFormat_ThrowsArgumentException()
        {
            StubIndexFileFunctions stubFile = new StubIndexFileFunctions();
            stubFile.IsCorrectFormat = false;

            TestXmlKeywordSearchIndexRepository repository = new TestXmlKeywordSearchIndexRepository("RootElement", stubFile);

            Assert.That(() => repository.OpenIndex("", new Version(2, 0)), Throws.InstanceOf(typeof(InvalidDataException)),
                "Stub method simulated that an Index file format was invalid but a InvalidDataException was not thrown.");
        }

        [Test]
        public void XmlIndexRepository_OpeningIncorrectVersion_ThrowsVersionException()
        {
            StubIndexFileFunctions stubFile = new StubIndexFileFunctions();
            stubFile.IsCorrectVersion = false;

            TestXmlKeywordSearchIndexRepository repository = new TestXmlKeywordSearchIndexRepository("RootElement", stubFile);

            Assert.That(() => repository.OpenIndex("", new Version(2, 0)), Throws.InstanceOf(typeof(InvalidFileIndexVersionException)));
        }

        [Test]
        public void XmlIndexRepository_CloneIndex_ClonesCorrectly()
        {
            var indexItems = new IIndexItem[]
            {
                new TestKeywordIndexItem("Item 1", "green,blue"),
                new TestKeywordIndexItem("Item 1", "green,red"),
                new TestKeywordIndexItem("Item 1", "yellow,gray")
            };

            var stubKeywordSearchIndex = new Mock<IIndex>();
            stubKeywordSearchIndex.Setup(m => m.All()).Returns(indexItems);
            stubKeywordSearchIndex.Setup(m => m.CurrentVersion).Returns(new Version(2, 0));

            var keywordSearchIndex = new Index("C:File.xml", new Version(2, 0));
 
            TestXmlKeywordSearchIndexRepository repository = new TestXmlKeywordSearchIndexRepository("RootElement", new StubIndexFileFunctions());
            IIndex newSearchIndex = repository.CloneIndex(stubKeywordSearchIndex.Object, @"C:\hello_world.txt");

            Assert.That(newSearchIndex, Is.Not.SameAs(stubKeywordSearchIndex.Object));
            Assert.That(newSearchIndex.ItemCount, Is.EqualTo(3));
            Assert.That(newSearchIndex.Contains(indexItems[0]), Is.True);
            Assert.That(newSearchIndex.Contains(indexItems[1]), Is.True);
            Assert.That(newSearchIndex.Contains(indexItems[2]), Is.True);
        }

        class StubIndexFileFunctions : IIndexFileFunctions
        {
            public bool IsCorrectFormat = true;
            public bool IsCorrectVersion = true;
            public bool IsExistingFile = true;

            public bool FileExists {  get { return IsExistingFile; } } 

            public bool Exists(string filePath)
            {
                return IsExistingFile;
            }
            public string Open(string filePath)
            {
                return string.Empty;
            }
            public void Save(string fileText, string filePath) { }

            public bool CheckFormat(string fileText)
            {
                return IsCorrectFormat;
            }

            public bool CheckVersion(string fileText, Version expectedVersion)
            {
                return IsCorrectVersion;
            }
        }

        class TestXmlKeywordSearchIndexRepository : XmlKeywordSearchIndexRepository<TestXmlKeywordIndexItem>
        {
            public TestXmlKeywordSearchIndexRepository(string rootElement, StubIndexFileFunctions indexFileFunctions) : base(rootElement, indexFileFunctions)
            {
            }

            protected override List<TestXmlKeywordIndexItem> LoadIndexItems(string filePath, Version currentVersion)
            {
                return new List<TestXmlKeywordIndexItem>();
            }
        }

        class TestXmlKeywordIndexItem : XmlIndexItem
        {
            public TestXmlKeywordIndexItem() : base()
            {
            }

            public TestXmlKeywordIndexItem(string id, string title, string syntax, DateTime dateCreated, DateTime dateModified, string commaDelimitedKeywords)
            : base(id, title, syntax, dateCreated, dateModified, commaDelimitedKeywords)
            {
            }

            public TestXmlKeywordIndexItem(string title, string syntax, string commaDelimitedKeywords)
            : base(title, syntax, commaDelimitedKeywords)
            {
            }
        }
    }
}
