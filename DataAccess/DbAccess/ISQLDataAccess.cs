using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DbAccess
{
    public interface ISQLDataAccess
    {
        Task<IEnumerable<T>> LoadData<T>(string sql, string connectionId = "DefaultConnection");
        Task<IEnumerable<T>> LoadData<T, U>(string sql, U data, string connectionId = "DefaultConnection");
        Task ExecuteProcedure(string sql, string connectionId = "DefaultConnection");
        Task SaveData<T>(string sql, T data, string connectionId = "DefaultConnection");
    }
}