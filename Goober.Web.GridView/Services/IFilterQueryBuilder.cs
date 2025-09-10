using Indusoft.Web.GridView.Models;
using System;

namespace Indusoft.Web.GridView.Services
{
    public interface IFilterQueryBuilder 
    {
        Func<T, bool> BuildFilterFunc<T>(FilterModelDto filterModel) where T : class;
    }
}
