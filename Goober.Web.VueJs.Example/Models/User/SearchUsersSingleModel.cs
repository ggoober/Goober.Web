using System;
using System.Collections.Generic;

namespace Goober.Web.VueJs.Example.Models.User
{
    public class SearchUsersSingleModel
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime? DateOfDelete { get; set; }

        public List<SearchUsersScopeModel> Scopes { get; set; } = new List<SearchUsersScopeModel>();

    }
}
