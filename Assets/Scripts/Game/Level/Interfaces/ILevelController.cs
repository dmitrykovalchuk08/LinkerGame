using System.Threading.Tasks;
using Configuration.DataStructures;

namespace Game.Level.Interfaces
{
    public interface ILevelController
    {
        Task Initialize(ILevelView view);
        Task<Task<bool>> PlayLevel(LevelConfiguration config);
    }
}