using Interfaces;
using Models;
using UnityEngine;

namespace Views
{
    internal class Shell : BaseShot, IMove
    {
        public Vector2 Movement { get; set; }
        public float Speed => 10;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.GetComponent<Enemy>())
                Hit();
        }
    }
}