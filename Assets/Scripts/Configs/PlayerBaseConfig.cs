using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerBaseConfig", menuName = "Configs / Player Base Config", order = 3)]
    public class PlayerBaseConfig : ScriptableObject
    {
        public GameObject shotPrefab;
        public int shotDamage;
        [Header("In milliseconds")]
        public int shootingDelay;
        public float shootingRange;
        [Header("Upgrade Shooting Damage Value")]
        public int increaseShotDamage;
        [Header("Upgrade Shooting Delay Value")]
        public int decreaseShootingDelayValue;
        [Header("Upgrade Shooting Range Value")]
        public float increaseShootingRangeValue;
    }
}