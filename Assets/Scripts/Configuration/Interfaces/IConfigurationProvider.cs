using System.Threading.Tasks;
using Configuration.DataStructures;

namespace Configuration.Interfaces
{
    public interface IConfigurationProvider
    {
        Task<LevelsData> GetLevelsConfiguration();
        Task<GameVisualData> GetVisualConfiguration();
    }
}