using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Objects;

namespace LastFMSearch.LastFM_Search_Library
{
    public class Search
    {

        private string ApiKey { get; set; }
        private string ApiSecret { get; set; }
        private ILastAuth Auth { get; set; }
        private IUserApi userApi { get; set; }

        public Search(string ApiKey, string ApiSecret, IUserApi userApi = null)
        {

                
            this.ApiKey = ApiKey;
            this.ApiSecret = ApiSecret;

            Auth = new LastAuth(ApiKey, ApiSecret);
            if (userApi == null)
                this.userApi = new IF.Lastfm.Core.Api.UserApi(Auth);
            else
                this.userApi = userApi;
        }

        public async Task<List<LastAlbum>> PerformTopAlbumSearch(string username)
        {
            var response = await userApi.GetTopAlbums(username, IF.Lastfm.Core.Api.Enums.LastStatsTimeSpan.Overall);
            return response.Content;
        }
    }
}
