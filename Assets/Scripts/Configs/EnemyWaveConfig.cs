using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyWaveConfig", menuName = "Configs / Enemy Wave Config", order = 2)]
    public class EnemyWaveConfig: ScriptableObject
    {
        public float radius;
        public float spawnRate;
        public float variance;
        public int countOfEnemies;
    }
}