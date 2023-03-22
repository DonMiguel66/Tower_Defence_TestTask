using Models;
using UnityEngine;

namespace Views
{
    internal sealed class EnemyShip : Enemy
    {
        public override void SetDirection()
        {
            Vector3 direction = Target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            direction.Normalize();
            Movement = direction;
        }

        public override void Move()
        {
            Vector3 direction = Target.transform.position - transform.position;
            direction.Normalize();
            Vector2 movement = direction;
            Rigidbody.MovePosition((Vector2)transform.position + (movement * (Speed * Time.deltaTime)));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject == Target.gameObject)
            {
                //Debug.Log("Collision");               
                Hit();
            }
            if (other.gameObject.GetComponent<BaseShot>())
            {
                //Debug.Log("Damaged");
                Damaged(other.gameObject.GetComponent<BaseShot>().Damage);        
            }
        }
        
        
        /*private Transform target;
        private Rigidbody2D rb;
        private Vector2 movement;
        private int speed = 2;
       
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            target = GameObject.Find("MainBase").transform;
        }
        void Update()
        {
            Vector3 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            direction.Normalize();
            movement = direction;
        }
        private void FixedUpdate()
        {
            MoveChar(movement);
        }
        private void MoveChar(Vector2 direction)
        {
            rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
        }*/

    }
}