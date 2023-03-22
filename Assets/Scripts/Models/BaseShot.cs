using System;
using Interfaces;
using UnityEngine;

namespace Models
{
    internal class BaseShot : MonoBehaviour, IHit, IShot
    {
        public int Damage {get; set;}
        protected GameObject _target;
        protected Rigidbody2D _rigidbody;
        
        public event Action OnBaseHit;
        public event Action CurrentDestroyAction;
        public GameObject Target
        {
            get => _target;
            set => _target = value;
        }

        public Rigidbody2D Rigidbody
        {
            get => _rigidbody;
            set => _rigidbody = value;
        }

        public void InitShot(int damage, GameObject target, Action destroyShot )
        {
            Damage = damage;
            Target = target;
            CurrentDestroyAction = destroyShot;
            OnBaseHit += CurrentDestroyAction;
        }
        
        public void Hit()
        {
            OnBaseHit?.Invoke();
            OnBaseHit -= CurrentDestroyAction;
        }
    }
}