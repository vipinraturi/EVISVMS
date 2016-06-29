/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Contract;
using Evis.VMS.Data.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.Context
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        protected VMSContext DataContext;

        #region Constructor and Destructors

        /// <summary>
        /// Initialize a new instance of Repository<T/>
        /// </summary>
        public Repository(VMSContext dataContext)
        {
            //DataContext = new SGHSolutionContext();

            DataContext = dataContext;

            // Gets the DbSet of the specific type, so information stored in database for 
            // that entity can be retrieve.
            _dbSet = DataContext.Set<T>();
        }

        #endregion

        #region IRepository<T> Members

        /// <summary>
        /// Adds a new entity to the database
        /// </summary>
        /// <param name="entity"> Entity to be added</param>
        public T Insert(T entity)
        {
            _dbSet.Add(entity);
            //DataContext.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Return a queryable collection on the basis of the passed filter criteria
        /// </summary>
        /// <param name="predicate">Filter criteria</param>
        /// <returns>Returns the filtered collection</returns>
        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        /// <summary>
        /// Returns all the instance of the entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// Gets an entity by its id
        /// </summary>
        /// <param name="id">Id of the object to be retrieved</param>
        /// <returns>Returns the selected entity</returns>
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Saves data to database
        /// </summary>
        public virtual void Update(T entity, bool isDeleted = false)
        {
            if (entity == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            var entry = DataContext.Entry<T>(entity);

            if (entry.State == EntityState.Detached || entry.State == EntityState.Modified)
            {
                object pkey;

                if (entity.GetType().GetProperty("Id") != null)
                {
                    pkey = entity.GetType().GetProperty("Id").GetValue(entity);
                }
                else
                {
                    pkey = entity.GetType().GetProperty("ID").GetValue(entity);
                }

                var set = DataContext.Set<T>();
                T attachedEntity = _dbSet.Find(pkey); // You need to have access to key

                if (attachedEntity != null)
                {
                    var attachedEntry = DataContext.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
                //DataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Saves data to database
        /// </summary>
        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            var entry = DataContext.Entry<T>(entity);

            object pkey;

            if (entity.GetType().GetProperty("Id") != null)
            {
                pkey = entity.GetType().GetProperty("Id").GetValue(entity);
            }
            else
            {
                pkey = entity.GetType().GetProperty("ID").GetValue(entity);
            }

            var set = DataContext.Set<T>();
            set.Remove(entity);
            entry.State = EntityState.Deleted; // This should attach entity                
            //DataContext.SaveChanges();
        }

        /// <summary>
        /// Delete Enity Record by Id's
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteById(int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = _dbSet.Find(id);

                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    //DataContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete Enity Record by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                //DataContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Delete Enity Record by LINQ Query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var objects = _dbSet.Where(predicate).AsEnumerable();
            foreach (var obj in objects)
                _dbSet.Remove(obj);
        }

        #endregion
    }
}
