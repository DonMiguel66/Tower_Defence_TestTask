using System;
using Models;

namespace Interfaces
{
    internal interface IHit
    {
        event Action OnBaseHit;
        void Hit();
    }
}