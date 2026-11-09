using System.Linq.Expressions;
using CY_DM;
using CY_WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CY_WebApi.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseDM
    {
        private readonly CyContext _db;
        private CyContext context;

        public Repository(CyContext context)
        {
            this.context = context;
            _db = context;
        }

        //public Repository(CyContext db)
        //{
        //    _db = db;
        //}
        //public Repository()
        //{
        //    _db = new CyContext();
        //}
        public async Task<T> Find(Guid id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> Insert(T entity)
        {
            //try
            //{

            if (entity == null)
                throw new ArgumentNullException("entity");
            if (entity.ID == null || entity.ID == Guid.Empty)
                entity.ID = Guid.NewGuid();
            var tt = _db.Set<T>().Add(entity);
            entity.CreateDate = DateTime.Now;
            entity.IsVisible = true;
            //  entity = _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    throw new Exception(dbEx.Message, dbEx);
            //}
            return tt.Entity;
        }

        public T InsertWithoutSave(T entity)
        {
            //try
            //{
            if (entity == null)
                throw new ArgumentNullException("entity");
            entity.CreateDate = DateTime.Now;
            entity.IsVisible = true;
            var tt = _db.Set<T>().Add(entity);
            //  entity = _db.Set<T>().Add(entity);
            //await _db.SaveChangesAsync();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    throw new Exception(dbEx.Message, dbEx);
            //}
            return tt.Entity;
        }

        public async Task<int> Update(T entity)
        {
            //try
            //{
            if (entity == null)
                throw new ArgumentNullException("entity is Null?!");

            //var id = entity.GetType().GetProperty("ID");
            var entity2 = await this.Find(entity.ID);
            if (entity2 == null) throw new NullReferenceException("entity not found");// _db.Set<T>().Find(id.GetValue(entity, null));
            entity.CreateDate = entity2.CreateDate;
            entity.IsVisible = true;
            entity.LastModified = DateTime.Now;
            //if (entity.GetType().GetProperty("CyHsPs") != null)
            //    entity.GetType().GetProperty("CyHsPs")?.SetValue(selu, item.Val);
            _db.Entry(entity2).CurrentValues.SetValues(entity);
            //entity.LastModified = DateTime.Now;
            //_db.Entry(entity).State = EntityState.Modified;

            return await _db.SaveChangesAsync();
            //return true;
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    throw new Exception(dbEx.Message, dbEx);
            //}
        }

        public async Task<int> Delete(Guid id)
        {
            //try
            //{
            //if (id == null)
            //    throw new ArgumentNullException("ID");
            var entity = await this.Find(id);
            entity.IsVisible = false;
            return await _db.SaveChangesAsync();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    throw new Exception(dbEx.Message, dbEx);
            //}
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return _db.Set<T>();
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return _db.Set<T>().AsNoTracking().Where(c => c.IsVisible);
            }
        }

        public DateTime GetDbTime()
        {
            var con = _db.Database.GetDbConnection();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT GETDATE()";
            con.Open();
            var datetime = (DateTime)cmd.ExecuteScalar();
            con.Close();

            //var dQuery = _db.Database.CreateQuery<DateTime>("CurrentDateTime() ");
            //DateTime dbDate = dQuery.AsEnumerable().First();

            //var formatedString = FormattableString.
            //var dateQuery = _db.Database.SqlQuery<DateTime>(fo "SELECT getdate()");
            //DateTime serverDate = dateQuery.AsEnumerable().First();
            return datetime;
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void DetectChanges()
        {
            _db.ChangeTracker.DetectChanges();
        }

        //public Task<int> Update(T entity)
        //{
        //    throw new NotImplementedException();
        //}
    }

}
