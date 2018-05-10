using CygSoft.Sanae.Index.Infrastructure;
using CygSoft.Sanae.Index.UnitTests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CygSoft.Sanae.Index.UnitTests
{
    [TestFixture]
    [Category("Tests.UnitTests")]
    [Category("SerializableIndexItem")]

    class SerializableIndexItemTests
    {
        private class TestSerializableIndexItem : SerializableIndexItem
        {
            public TestSerializableIndexItem(string title) :
                base(title)
            {

            }

            public TestSerializableIndexItem(string id, string title, DateTime dateCreated, DateTime dateModified) :
                base(id, title, dateCreated, dateModified)
            {

            }
        }

        //[Test]
        //public void SerializableIndexItem_()
        //{
        //}

        [Test]
        public void SerializableIndexItem_Serialize_Matches_Expected_Format()
        {
            DateTime createDate = DateTime.Parse("2018/06/12 18:23:12");
            DateTime modifiedDate = DateTime.Parse("2018/06/22 08:00:52");

            ISerializableIndexItem indexItem = new TestSerializableIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Components and Libraries", createDate, modifiedDate);
            string serializedXml = Functions.ComparableXml(indexItem.Serialize().ToString());
            string expectedXml = Functions.ComparableXml(TxtFile.ReadText("SerializableIndexItemXML.txt"));

            Assert.That(expectedXml, Is.EqualTo(serializedXml));
        }

        [Test]
        public void SerializableIndexItem__PassModifyDateSmallerThanCreateDate_ThrowsApplicationException()
        {
            DateTime createDate = DateTime.Parse("2018/06/12 18:23:12");
            DateTime modifiedDate = DateTime.Parse("2018/06/12 18:23:11");

            Assert.That(
                () => new TestSerializableIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Test Title", createDate, modifiedDate),
                Throws.InstanceOf(typeof(ApplicationException))
            );
        }

        [Test]
        public void SerializableIndexItem_InitializedAsExisting_With_Dates_CorrectlyReflects_Dates()
        {
            DateTime createDate = DateTime.Parse("2018/06/12 18:23:12");
            DateTime modifiedDate = DateTime.Parse("2018/06/22 08:00:52");

            ISerializableIndexItem indexItem = new TestSerializableIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Test Title", createDate, modifiedDate);

            Assert.AreEqual(DateTime.Parse("2018/06/12 18:23:12"), indexItem.DateCreated);
            Assert.AreEqual(DateTime.Parse("2018/06/22 08:00:52"), indexItem.DateModified);
        }

        [Test]
        public void SerializableIndexItem_InitializedWithParameteredConstructor_ReturnsPassInIdString()
        {
            ISerializableIndexItem indexItem = new TestSerializableIndexItem("4ecac722-8ec5-441c-8e3e-00b192b30453", "Test Title", DateTime.Now, DateTime.Now);
            string id = indexItem.Id;
            Assert.That(id, Is.EqualTo("4ecac722-8ec5-441c-8e3e-00b192b30453"));
        }

        [Test]
        public void SerializableIndexItem_InitializedWithConstructorNoId_ReturnsNewIdString()
        {
            ISerializableIndexItem indexItem = new TestSerializableIndexItem("Test Title");
            Guid guid = new Guid(indexItem.Id);
            Assert.That(guid == Guid.Empty, Is.False);
        }
    }
}
