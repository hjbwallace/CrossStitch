using CrossStitch.Core.Extensions;
using CrossStitch.Core.Helpers;
using CrossStitch.Core.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Serilog;

namespace CrossStitch.Core.Data
{
    public abstract class DatabaseContextRepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly Func<DatabaseContext> _contextFunc;

        public DatabaseContextRepositoryBase()
        {
            var contextFactory = SimpleIoc.Default.GetInstance<IDatabaseContextService>();
            _contextFunc = contextFactory.GetContext();
        }

        protected virtual string TableName => typeof(T).Name + "s";

        public void Clear()
        {
            using (var ctx = GetContext())
            {
                var sql = $"DELETE FROM {TableName}";
                ctx.Database.ExecuteSqlCommand(sql);
            }
        }

        public T Get(object key)
        {
            using (var ctx = GetContext())
            {
                var source = GetQuery(ctx);
                return GetSingle(source, key);
            }
        }

        public T[] GetAll()
        {
            using (var ctx = GetContext())
                return GetQuery(ctx).ToArray();
        }

        public void Remove(params T[] entities)
        {
            using (var ctx = GetContext())
            {
                foreach (var entity in entities)
                {
                    Log.Information("Removing {Type} from the database: {Entity}", typeof(T).Name, entity.ToString());
                    ctx.Remove(entity);
                }

                ctx.SaveChanges();
            }
        }

        public void Save(params T[] entities)
        {
            using (var ctx = GetContext())
            {
                foreach (var entity in entities)
                {
                    Log.Information("Saving {Type} to the database: {Entity}", typeof(T).Name, entity.ToString());
                    ctx.AddOrUpdate(entity);
                }

                ctx.SaveChanges();
            }
        }

        public T[] Query(Expression<Func<T, bool>> query)
        {
            using (var ctx = GetContext())
                return GetQuery(ctx).Where(query).ToArray();
        }

        protected DatabaseContext GetContext() => _contextFunc.Invoke();

        protected virtual IQueryable<T> GetQuery(DatabaseContext ctx)
        {
            return ctx.Set<T>().AsNoTracking();
        }

        protected virtual IQueryable<T> GetSingleQuery(DatabaseContext ctx)
        {
            return GetQuery(ctx);
        }

        protected virtual T GetSingle(IQueryable<T> source, object key)
        {
            var keyProperty = KeyHelper.GetKeyProperty<T>();
            return source.SingleOrDefault(s => keyProperty.GetValue(s).Equals(key));
        }
    }
}