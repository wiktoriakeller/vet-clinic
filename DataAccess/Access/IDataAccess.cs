using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IDataAccess<T>
    {
        Task Delete(int id);
        Task<T> Get(int id);
        Task<IEnumerable<T>> Get();
        Task Insert(T entity);
        Task Update(T entity);
    }
}
