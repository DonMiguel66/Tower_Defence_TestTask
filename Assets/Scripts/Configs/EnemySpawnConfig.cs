using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemySpawnConfig", menuName = "Configs / Enemy Spawn Config", order = 1)]
    public class EnemySpawnConfig : ScriptableObject
    {
        public GameObject enemyPrefab;
        public float enemySpeed;
        public string targetName;
       
    }
}