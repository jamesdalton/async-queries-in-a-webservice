using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Interfaces
{
    public interface IAnimeDataAccess
    {
        IEnumerable<int> Get();
        Anime Get(int id);
        Task<Anime> GetAsync(int id);
    }
}