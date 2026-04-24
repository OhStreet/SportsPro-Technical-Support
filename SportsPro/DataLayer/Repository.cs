using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.DataLayer
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected SportsProContext context { get; set; }
        private DbSet<T> dbset { get; set; }

        public Repository(SportsProContext ctx)
        {
            context = ctx;
            dbset = ctx.Set<T>();
        }

        public virtual IEnumerable<T> List(QueryOptions<T> options)
        {
            IQueryable<T> query = dbset;

            // Apply each include
            foreach (string include in options.GetIncludes())
            {
                if (!string.IsNullOrEmpty(include))
                    query = query.Include(include);
            }

            // Apply all WHERE clauses
            foreach (var where in options.WhereClauses)
            {
                query = query.Where(where);
            }

            // Apply ordering
            if (options.HasOrderBy)
                query = query.OrderBy(options.OrderBy);

            if (options.HasThenOrderBy)
                query = ((IOrderedQueryable<T>)query).ThenBy(options.ThenOrderBy);

            return query.ToList();
        }

        public virtual T? Get(int id) => dbset.Find(id);

        public virtual T? Get(QueryOptions<T> options)
        {
            IQueryable<T> query = dbset;

            foreach (string include in options.GetIncludes())
            {
                if (!string.IsNullOrEmpty(include))
                    query = query.Include(include);
            }

            foreach (var where in options.WhereClauses)
            {
                query = query.Where(where);
            }

            return query.FirstOrDefault();
        }

        public virtual void Insert(T entity) => dbset.Add(entity);
        public virtual void Update(T entity) => dbset.Update(entity);
        public virtual void Delete(T entity) => dbset.Remove(entity);
        public virtual void Save() => context.SaveChanges();
    }
}
