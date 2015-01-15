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
            
            var GetUsersContent = new List<LastAlbum>() { new LastAlbum() { Name = "bob" } };
            var UserResponse = new PageResponse<LastAlbum>(GetUsersContent);

            var TopAlbumsReturnValue = Task.Factory.StartNew(() => UserResponse);

            mockUserApi.Setup(x => x.GetTopAlbums(It.Is<string>(s => s == "PCurd"), It.IsAny<IF.Lastfm.Core.Api.Enums.LastStatsTimeSpan>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(TopAlbumsReturnValue);

            
            
            Mock<ILibraryApi> mockLibraryApi = new Mock<ILibraryApi>();

            var GetTracksContent = new List<LastTrack>() { new LastTrack() { Name = "BestTrack" } };
            var TracksResponse = new PageResponse<LastTrack>(GetTracksContent);

            var TracksReturnValue = Task.Factory.StartNew(() => TracksResponse);

            
            mockLibraryApi.Setup(x => x.GetTracks(It.Is<string>(s => s == "PCurd"), It.Is<string>(s => s == "Madonna"), It.Is<string>(s => s == "The First Album"), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(TracksReturnValue);

            search = new Search("fcd69d7ed7d0ce3363019425b287dd93", "abc35d79708cc3fd4a3f93412230ce35", mockUserApi.Object, mockLibraryApi.Object);
        }

        [TestMethod]
        public void Test_AlwaysPasses()
        {
            Assert.AreEqual(1,1);
        }

        public string ListToString<T>(IReadOnlyList<T> LastFMObject, Func<T, string> action)
        {
            var sb = new System.Text.StringBuilder();

            foreach (var lastFMObject in LastFMObject)
            { 
                sb.AppendFormat("{0}", action(lastFMObject));
            }

            return sb.ToString();
        }

        [TestMethod]
        public async Task Album_Name_Is_Accessible()
        {
            var test = await search.PerformTopAlbumSearch("PCurd");

            Func<IF.Lastfm.Core.Objects.LastAlbum, string> ValueToReturn = (x) => x.Name;

            Assert.AreEqual("bob",ListToString(test, ValueToReturn));
        }

        [TestMethod]
        public async Task Top_Track_Name_Is_Accessible()
        {
            var test = await search.PerformTopTrackSearch("PCurd","Madonna", "The First Album", new DateTimeOffset(new DateTime(2010,1,1)));

            Func<IF.Lastfm.Core.Objects.LastTrack, string> ValueToReturn = (x) => x.Name;

            Assert.AreEqual("BestTrack", ListToString(test, ValueToReturn));
        }
    }
}
