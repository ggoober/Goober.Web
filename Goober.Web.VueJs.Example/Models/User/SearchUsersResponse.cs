using System.Collections.Generic;

namespace Goober.Web.VueJs.Example.Models.User
{
    public class SearchUsersResponse
    {
        public int Count { get; set; }

        public List<SearchUsersSingleModel> Users { get; set; } = new List<SearchUsersSingleModel>();
    }
}
