using Microsoft.VisualStudio.TestTools.UnitTesting;
using Open5ECreatureDownloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open5eCreatureDownloader.Test
{
    [TestClass]
    public class CreatureDownloaderTest
    {
        [TestMethod]
        public void ListingWorks()
        {
            var downloader = new CreatureDownloader();

            var list = downloader.ListMonsters();

            Assert.IsTrue(list.Any());
        }
    }
}
