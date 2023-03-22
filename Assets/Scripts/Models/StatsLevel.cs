using Enums;

namespace Models
{
    internal struct StatsLevel<T>
    {
        public Level CurrentLevel;
        public T CurrentValue;
    }
}