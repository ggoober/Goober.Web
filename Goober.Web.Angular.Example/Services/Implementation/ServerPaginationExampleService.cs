using Indusoft.Web.Angular.Example.Models.Api.ServerPaginationExample;
using Indusoft.Web.GridView.Enums;
using Indusoft.Web.GridView.Helpers;
using Indusoft.Web.GridView.Models;
using Indusoft.Web.GridView.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indusoft.Web.Angular.Example.Services.Implementation
{
    public class ServerPaginationExampleService : IServerPaginationExampleService
    {
        private readonly List<ServerPaginationExampleDto> _dtos;

        private readonly IFilterQueryBuilder _filterQueryBuilder;

        public ServerPaginationExampleService(IFilterQueryBuilder filterQueryBuilder)
        {
            _dtos = GenerateDtos();

            _filterQueryBuilder = filterQueryBuilder;
        }

        public async Task<int> GetCount(FilterModelDto filterModel)
        {
            if (filterModel == null || filterModel.Filters == null || filterModel.Filters.Length == 0)
                return _dtos.Count;

            var func = _filterQueryBuilder.BuildFilterFunc<ServerPaginationExampleDto>(filterModel);

            var result = _dtos.Where(func)
                              .ToList();

            return result.Count;
        }

        public async Task<List<ServerPaginationExampleDto>> GetData(int pageNumber, int pageSize, FilterModelDto filterModel, SortModelDto sortModel)
        {
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            if (filterModel == null || filterModel.Filters == null || filterModel.Filters.Length == 0)
                return _dtos.OrderByDirection(sortModel?.Sorts?.FirstOrDefault()?.SortType ?? SortTypeEnum.ASC, sortModel?.Sorts?.FirstOrDefault()?.FieldName)
                            .Skip(skip)
                            .Take(take)
                            .ToList();

            var func = _filterQueryBuilder.BuildFilterFunc<ServerPaginationExampleDto>(filterModel);

            var result = _dtos.Where(func)
                              .OrderByDirection(sortModel?.Sorts?.FirstOrDefault()?.SortType ?? SortTypeEnum.ASC, sortModel?.Sorts?.FirstOrDefault()?.FieldName)
                              .Skip(skip)
                              .Take(take)
                              .ToList();

            return result;
        }

        private static List<ServerPaginationExampleDto> GenerateDtos(int quantity = 1000)
        {
            var result = new List<ServerPaginationExampleDto>();

            for (var cnt = 0; cnt < quantity; cnt++)
            {
                result.Add(new ServerPaginationExampleDto()
                {
                    Id = cnt + 1,
                    Name = "Name_" + cnt,
                    Number = 100 * cnt,
                    CreationDate = DateTime.Now.AddDays(-1 * cnt),
                    IsValid = cnt % 2 == 0
                });
            }

            return result;
        }
    }
}
