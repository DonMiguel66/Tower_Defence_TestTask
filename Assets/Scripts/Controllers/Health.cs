using System;

namespace Controllers
{
    internal sealed class Health
    {
        public int Max { get; private set; }
        public int Current { get; private set; }

        public event Action OnLethalDamage;
        
        public Health(int max, int current)
        {
            Max = max;
            Current = current;
        }

        private void ChangeCurrentHealth(int hp)
        {
            Current = hp;
        }

        private void SetNewMaxHealth(int hp)
        {
            Max = hp;
        }

        public void ReceiveDamage(int damage)
        {
            ChangeCurrentHealth(Current-damage);
            //Debug.Log(Current);
            if(Current <= 0)
                OnLethalDamage?.Invoke();
        }
    }
}