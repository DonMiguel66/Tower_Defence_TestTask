using System;
using Controllers;
using Interfaces;
using UnityEngine;

namespace Models
{
    internal abstract class Enemy : BasedObject, IHit, IMove
    {
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] private GameObject _target;
        [SerializeField] protected Vector3 _direction;
        [SerializeField] private float _speed;
        [SerializeField] private int _hp;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public Vector2 Movement { get; set; }
        public float Speed => _speed;
        public GameObject Target => _target;

        public Vector3 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        public Rigidbody2D Rigidbody => _rigidbody;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public event Action OnBaseHit;
        public event Action<int> OnDamageReceived;
        public void SetRigidbody()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }
        
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        
        public void SetTarget(GameObject target)
        {
            _target = target;
            //Debug.Log(_target.name);
        }
        
        public void InjectHealth(Health hp)
        {
            Health = hp;
        }
        
        public void Hit()
        {
            OnBaseHit?.Invoke();
        }

        protected void Damaged(int damage)
        {
            //Debug.Log(Health.Current);
            OnDamageReceived?.Invoke(damage);
        }

        public abstract void Move();
        public abstract void SetDirection();
    }
}