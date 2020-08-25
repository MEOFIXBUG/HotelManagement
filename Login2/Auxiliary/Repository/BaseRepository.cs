using Login2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Login2.Auxiliary.Repository
{
    class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class

    {

        //internal hotelEntities context;

        internal DbSet<TEntity> dbSet;

        //public BaseRepository()
        //{
        //    this.context = new hotelEntities();
        //    dbSet = context.Set<TEntity>();
        //}
        //public BaseRepository(hotelEntities context)
        //{
        //    this.context = context;
        //    this.dbSet = context.Set<TEntity>();
        //}

        public IEnumerable<TEntity> GetAll()
        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                return dbSet.ToList();
            }
        }
        public virtual IEnumerable<TEntity> GetWithRawSql(string query,

            params object[] parameters)
        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                return dbSet.SqlQuery(query, parameters).ToList();
            }
        }

        public virtual IEnumerable<TEntity> Get(

            Expression<Func<TEntity, bool>> filter = null,

            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            string includeProperties = "")

        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties.Split

                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return query.ToList();
                }
            }

        }

        public virtual TEntity GetByID(object id)
        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                return dbSet.Find(id);
            }
        }

        public virtual void Insert(TEntity entity)
        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                dbSet.Add(entity);
            }
        }

        public virtual void Delete(object id)
        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                TEntity entityToDelete = dbSet.Find(id);
                Delete(entityToDelete);
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                if (context.Entry(entityToDelete).State == EntityState.Detached)

                {
                    dbSet.Attach(entityToDelete);

                }
                dbSet.Remove(entityToDelete);
            }

        }


        public virtual void Update(TEntity entityToUpdate)
        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                dbSet.Attach(entityToUpdate);

                context.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }
        public void Save()
        {
            using (var context = new hotelEntities())
            {
                dbSet = context.Set<TEntity>();
                context.SaveChanges();
            }
        }
        ~BaseRepository()
        {

        }
    }
}
