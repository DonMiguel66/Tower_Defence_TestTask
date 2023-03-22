using System;
using Interfaces;
using UnityEngine;
using Views;

namespace Models
{
    internal class Base : BasedObject
    {
        private Rigidbody _rigidbody;
        public int ShootingSpeedDelay
        {
            get => _shootingSpeedDelay;
            set => _shootingSpeedDelay = value;
        }

        public float ShootingRange
        {
            get => _shootingRange;
            set => _shootingRange = value;
        }

        public int CurrentDamage
        {
            get => _currentDamage;
            set => _currentDamage = value;
        }

        private int _shootingSpeedDelay;
        private float _shootingRange;
        
        private int _currentDamage;

        public event Action OnEnemyHitBase;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<EnemyShip>())
                OnEnemyHitBase?.Invoke();
        }

    }
}