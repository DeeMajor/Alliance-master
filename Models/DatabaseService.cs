using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class DatabaseService<TEntity> : IDatabaseService<TEntity> where TEntity : class
    {
        private ApplicationDbContext _context;
        private const string _timezoneSettingsKey = "timezone";

        private IDbSet<TEntity> Entities
        {
            get { return this._context.Set<TEntity>(); }
        }
        public DatabaseService(ApplicationDbContext context)
        {
            this._context = context;
        }
        //Get list of details in the table
        public IQueryable<TEntity> Get()
        {
            return Entities.AsQueryable();
        }
        //Get Details of entities
        public TEntity Get(object id)
        {
            return Entities.Find(id);
        }
        //Insert data into tables
        public bool Insert(TEntity entity)
        {
            Entities.Add(entity);
            return this._context.SaveChanges() > 0;
        }
        //Edit your table data
        public bool Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _context.Entry(entity).State = EntityState.Modified;
            return this._context.SaveChanges() > 0;
        }
        public bool Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Entities.Remove(entity);
            return this._context.SaveChanges() > 0;
        }
        public void Delete(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
        public IQueryable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            return Entities.Where(predicate).AsQueryable();

        }
        //GetCurrentSystemDate
        private DateTime GetCurrentSystemDate()
        {
            var timezone = ConfigurationManager.AppSettings.Get(_timezoneSettingsKey).ToString();
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(timezone));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }
    }
}