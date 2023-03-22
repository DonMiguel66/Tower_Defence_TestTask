using System;
using UnityEngine;

namespace Views
{
    internal class CheckEnemyZone : MonoBehaviour
    {
        public event Action OnEnemyEnter;

        [SerializeField]private CircleCollider2D _collider2D;

        public float testValue = 0f;
        public CircleCollider2D Collider2D
        {
            get => _collider2D;
            set => _collider2D = value;
        }

        private void Awake()
        {
            _collider2D = gameObject.GetComponent<CircleCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //Debug.Log($"CollisionEnter + {other.gameObject.name}");
            //OnEnemyEnter?.Invoke();
        }
        
        private void OnCollisionStay2D(Collision2D other)
        {
            Debug.Log($"CollisionSTay + {other.gameObject.name}");
            if(other.gameObject.GetComponent<EnemyShip>())
                OnEnemyEnter?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, testValue);
        }
    }
}