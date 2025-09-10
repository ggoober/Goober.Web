using System.Collections.Generic;

namespace Indusoft.Web.VueJs.Example.Models.User
{
    public class SearchUsersRequest
    {
        public string TextQuery { get; set; }

        public int Count { get; set; }

        public List<int> ScopeIds { get; set; } = new List<int>();
    }
}
