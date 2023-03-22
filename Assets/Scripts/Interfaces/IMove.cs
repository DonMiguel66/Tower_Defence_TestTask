using UnityEngine;

namespace Interfaces
{
    internal interface IMove
    {
        Vector2 Movement { get; set; }
        float Speed { get; }
    }
}