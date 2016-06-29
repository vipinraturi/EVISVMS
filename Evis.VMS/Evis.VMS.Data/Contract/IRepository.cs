/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.Contract
{
    public interface IRepository<T>
    {
        T Insert(T entity);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        T GetById(int id);
        void Update(T entity, bool isDeleted = false);
        void Delete(T entity);
        void DeleteById(int[] ids);
        bool DeleteById(int id);
        void Delete(Expression<Func<T, bool>> predicate);
    }
}
