using System.Linq.Expressions;

namespace SportsPro.DataLayer
{
    public class QueryOptions<T>
    {
        // Sorting
        public Expression<Func<T, Object>> OrderBy { get; set; } = null!;
        public Expression<Func<T, Object>> ThenOrderBy { get; set; } = null!;

        // Multiple WHERE clauses 
        private List<Expression<Func<T, bool>>> whereClauses = new List<Expression<Func<T, bool>>>();
        public IEnumerable<Expression<Func<T, bool>>> WhereClauses => whereClauses;

        // Write-only property to add a WHERE clause to the list
        public Expression<Func<T, bool>> Where
        {
            set => whereClauses.Add(value);
        }

        // Includes
        private string[] includes = Array.Empty<string>();
        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(',');
        }
        public string[] GetIncludes() => includes;

        // Read-only helpers
        public bool HasWhere => whereClauses.Count > 0;
        public bool HasOrderBy => OrderBy != null;
        public bool HasThenOrderBy => ThenOrderBy != null;
    }
}
