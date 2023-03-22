using System;
using UnityEngine;

namespace Interfaces
{
    internal interface IShot
    {
        int Damage { get; }

        void InitShot(int damage, GameObject target, Action destroyShot);
        /*void SetDamage(int damage);
        void SetTarget(GameObject target);*/
    }
}