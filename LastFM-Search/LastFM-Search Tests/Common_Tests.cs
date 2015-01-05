using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LastFMSearch.LastFM_Search_Library;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;

namespace LastFMSearch.LastFM_Search_Tests
{
    [TestClass]
    public class Common_Tests
    {

        public Search search {get;private set;}

        [TestInitialize]
        public void init()
        {
            Mock<IUserApi> mockUserApi = new Mock<IUserApi>();

            var pageResponse = new PageResponse<LastAlbum>();

            pageResponse.Content.Add(new LastAlbum() { Name = "bob" });

            var returnValue = Task.Factory.StartNew( () => pageResponse);

            mockUserApi.Setup(x => x.GetTopAlbums(It.Is<string>(s => s == "PCurd"), It.IsAny<IF.Lastfm.Core.Api.Enums.LastStatsTimeSpan>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(returnValue);
            search = new Search("fcd69d7ed7d0ce3363019425b287dd93", "abc35d79708cc3fd4a3f93412230ce35", mockUserApi.Object);
        }

        [TestMethod]
        public void Test_AlwaysPasses()
        {
            Assert.AreEqual(1,1);
        }

        public string ListToString<T>(List<T> LastFMObject, Func<T, string> action)
        {
            var sb = new System.Text.StringBuilder();
            LastFMObject.ForEach(x => sb.AppendFormat("{0}", action(x)));

            return sb.ToString();
        }

        [TestMethod]
        public async Task Album_Name_Is_Accessible()
        {
            var test = await search.PerformTopAlbumSearch("PCurd");

            Func<IF.Lastfm.Core.Objects.LastAlbum, string> ValueToReturn = (x) => x.Name;

            Assert.AreEqual("bob",ListToString(test, ValueToReturn));
        }
    }
}
