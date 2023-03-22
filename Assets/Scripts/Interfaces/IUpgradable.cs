using Enums;

namespace Interfaces
{
    internal interface IUpgradable
    {
        void UpgradeDamageTo(Level level);
        void UpgradeShootingSpeedTo(Level level);
        void UpgradeShootingRangeTo(Level level);
    }
}