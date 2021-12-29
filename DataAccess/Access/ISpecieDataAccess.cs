using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface ISpecieDataAccess
    {
        Task Delete(int specieId);
        Task<Specie> Get(int specieId);
        Task<IEnumerable<Specie>> Get();
        Task Insert(Specie specie);
        Task Update(Specie specie);
    }
}