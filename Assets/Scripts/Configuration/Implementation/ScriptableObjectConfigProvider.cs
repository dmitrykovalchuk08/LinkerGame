using System.Threading.Tasks;
using Configuration.DataStructures;
using Configuration.Interfaces;
using UnityEngine;

namespace Configuration.Implementation
{
    public class ScriptableObjectConfigProvider : IConfigurationProvider
    {
        private ScriptableGameConfiguration configInstance;

        public ScriptableObjectConfigProvider()
        {
            configInstance = Resources.Load<ScriptableGameConfiguration>("GameConfiguration");
        }
        
        public Task<LevelsData> GetLevelsConfiguration()
        {
            var tcs = new TaskCompletionSource<LevelsData>();
            tcs.SetResult( configInstance.LevelData);
            return tcs.Task;
        }
        
        public Task<GameVisualData> GetVisualConfiguration()
        {
            var tcs = new TaskCompletionSource<GameVisualData>();
            tcs.SetResult( configInstance.VisualConfig);
            return tcs.Task;
        }
    }
}
