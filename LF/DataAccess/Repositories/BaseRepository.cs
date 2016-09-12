using LF.Context;
using LF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace LF.DataAccess.Repositories
{
    public class BaseRepository<T> where T : class, new()
    {
        //Represent the context of the database.
        public ApplicationDbContext Context { get; set; }

        //Represent a virtual table of the database.
        protected IDbSet<T> DbSet { get; set; }

        //Represent an instance of the class - UnitOfWork.
        public UnitOfWork _UnitOfWork { get; set; }

        //Represents base(empty) constructor of the base repository.
        public BaseRepository()
        {
            this.Context = new ApplicationDbContext();
            this.DbSet = this.Context.Set<T>();
        }

        //Represents a constructor that accepts UnitOfWork class as a parameter.
        //This constructor is called when we have to save data in more than one tables.
        public BaseRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentException("An instance of the UnitOfWork class is null", "UnitOfWork class miss");
            }
            this.Context = unitOfWork.Context;
            this.DbSet = this.Context.Set<T>();
            this._UnitOfWork = unitOfWork;
        }

        public IObjectContextAdapter GetObjectContextAdapter()
        {
            return (IObjectContextAdapter)this.Context;
        }

        public virtual Task<T> GetById(Guid id)
        {
            return Task.FromResult(DbSet.Find(id));
        }

        public virtual Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToListAsync();
        }

        #region CRUD OPERATIONS
        public virtual async Task Insert(T item)
        {
            DbSet.Add(item);
            await Context.SaveChangesAsync();
        }

        public virtual async Task Update(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public virtual async Task Delete(T item)
        {
            DbSet.Remove(item);
            await Context.SaveChangesAsync();
        }

        #endregion

        public virtual void Dispose()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }
    }
}