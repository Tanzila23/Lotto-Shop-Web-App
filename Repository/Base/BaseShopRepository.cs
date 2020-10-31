﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository.Base
{
    public class BaseShopRepository<T> : IBaseShopRepository<T> where T : class
    {

        private DbContext _db;
        public BaseShopRepository(DbContext db)
        {
            _db = db;
        }
        public DbSet<T> Table
        {
            get { return _db.Set<T>(); }
        }
        public virtual void Add(T entity)
        {
            Table.Add(entity);
            //return _db.SaveChanges() > 0;


        }
        public virtual T Save(T entity)
        {
            Table.Add(entity);
     
            return entity;

        }

        public void Add(params T[] entities)
        {
            Table.AddRange(entities);
        }

        public void Add(IEnumerable<T> entities)
        {
            Table.AddRange(entities);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            return Table;
        }

        public virtual T GetById(int id)
        {
            return Table.Find(id);

        }
        public T Single(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            return _db.Set<T>().Where(predicate).SingleOrDefault<T>();
        }

        public IQueryable<T> Query(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public virtual bool Remove(T entity)
        {
            Table.Remove(entity);
            return true;
            // return _db.SaveChanges() > 0;
        }

        public void Remove(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                Table.Remove(item);
            }
        }

        public void Remove(params T[] entities)
        {
            Table.RemoveRange(entities);
        }

        public void Remove(IEnumerable<T> entities)
        {
            Table.RemoveRange(entities);
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }



        public virtual bool Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            // return _db.SaveChanges() > 0;
            return true;
        }

        public void Update(params T[] entities)
        {
            Table.UpdateRange(entities);
        }

        public void Update(IEnumerable<T> entities)
        {
            Table.UpdateRange(entities);

        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            return _db.Set<T>().Where(predicate).SingleOrDefault<T>();
        }
    }
}
