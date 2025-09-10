using System.Collections.Generic;

namespace Indusoft.Web.VueJs.Example.Models.User
{
    public class SearchUsersScopeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SearchUsersClaimModel> Claims { get; set; } = new List<SearchUsersClaimModel>();
    }
}
