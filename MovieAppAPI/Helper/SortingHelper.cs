using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace MovieAppAPI.Helper
{
    public class SortingHelper<T> : ISortingHelper<T> where T : class
    {
        public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByParameters)
        {
            if(!entities.Any() || string.IsNullOrEmpty(orderByParameters)) 
            {
                return entities;
            }

            string[] orderParams = orderByParameters.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            StringBuilder orderByQueryBuilder = new StringBuilder();

            foreach(string param in orderParams)
            {
                if (string.IsNullOrEmpty(param))
                    continue;
                string propertyFromParam = param.Trim().Split(' ')[0].Trim();
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromParam, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty is null)
                    throw new Exception($"{typeof(T)} does not have a {propertyFromParam} property.");
                
                var sortingDirection = (param.Trim().EndsWith("desc")) ? "descending" : "ascending";

                orderByQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingDirection}, ");
            }

            var orderByQuery = orderByQueryBuilder.ToString().TrimEnd(',', ' ');
            return entities.OrderBy(orderByQuery);
        }
    }
}
