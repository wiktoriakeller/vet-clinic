using DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Access
{
    public interface ISpecieDataAccess
    {
        Task DeleteSpecie(int specieId);
        Task<Specie> GetSpecie(int specieId);
        Task<IEnumerable<Specie>> GetSpecies();
        Task InsertSpecie(Specie specie);
        Task UpdateSpecie(Specie specie);
    }
}