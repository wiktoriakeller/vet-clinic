using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface IProcedureDataAccess<T, U>
    {
        Task Insert(T entity, U additionalData);
    }
}
