using Indusoft.Web.GridView.Services;
using Indusoft.Web.GridView.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Indusoft.Web.GridView
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGridView(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFilterQueryBuilder, FilterQueryBuilder>();
        }
    }
}
