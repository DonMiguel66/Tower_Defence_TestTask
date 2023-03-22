using System;
using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;
using Views;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Controllers
{
    internal class EnemySpawnController : IDisposable
    {
        private EnemySpawnConfig _enemySpawnConfig;
        private EnemyWaveConfig _enemyWaveConfig;
        private ViewServices _enemiesViewServices;

        private SpriteAnimatorController _spriteAnimatorController;
        
        private GameObject _enemyPrefab;
        private GameObject _target;

        private List<EnemyShip> _enemyShipsViews = new List<EnemyShip>();

        public List<EnemyShip> EnemyShipsViews
        {
            get => _enemyShipsViews;
            set => _enemyShipsViews = value;
        }
        public EnemySpawnController(ViewServices viewServices, EnemySpawnConfig enemySpawnConfig, EnemyWaveConfig enemyWaveConfig, SpriteAnimatorController spriteAnimatorController)
        {
            _spriteAnimatorController = spriteAnimatorController;
            _enemiesViewServices = viewServices;
            _enemySpawnConfig = enemySpawnConfig;
            _enemyWaveConfig = enemyWaveConfig;
            _target = GameObject.Find(_enemySpawnConfig.targetName);
            _enemyPrefab = _enemySpawnConfig.enemyPrefab;
            CreatePoolOfEnemies(_enemyWaveConfig.countOfEnemies);
        }

        private void CreatePoolOfEnemies(int countOfEnemies)
        {
            for (int i = 0; i < countOfEnemies; i++)
            {
                var enemyShip = _enemiesViewServices.Instantiate(_enemyPrefab).GetComponent<EnemyShip>();
                enemyShip.SetTarget(_target);
                enemyShip.InjectHealth(new Health(3,3));
                enemyShip.SetSpeed(_enemySpawnConfig.enemySpeed);
                enemyShip.SetRigidbody();
                _enemyShipsViews.Add(enemyShip);
                enemyShip.OnDamageReceived += damage =>  enemyShip.Health.ReceiveDamage(damage);
            }
            for (int i = 0; i < countOfEnemies; i++)
            {
                _enemiesViewServices.Destroy(_enemyShipsViews[i].gameObject);
            }
        }
        public async UniTask EnemyRespawn()
        {
            foreach (var enemyShipsView in _enemyShipsViews.ToArray())
            {
                var nextSpawnTime = _enemyWaveConfig.spawnRate + Random.Range(-_enemyWaveConfig.variance, _enemyWaveConfig.variance);
                await UniTask.Delay(TimeSpan.FromSeconds(nextSpawnTime));
                LaunchEnemy(enemyShipsView);
            }
        }
        
        private void LaunchEnemy(EnemyShip enemyShipView)
        {
            var calcEnemyPosition = Random.insideUnitSphere * _enemyWaveConfig.radius;
            var targetPosition = enemyShipView.Target.transform.position;
                
            calcEnemyPosition += targetPosition;
            calcEnemyPosition.y = 0f;
            
            Vector3 direction = calcEnemyPosition - targetPosition;
            direction.Normalize();

            var forward = enemyShipView.Target.transform.forward;
            float dotProduct = Vector3.Dot(forward, direction);
            float dotProductAngle = Mathf.Acos(dotProduct / forward.magnitude * direction.magnitude);
            
            calcEnemyPosition.x = Mathf.Cos(dotProductAngle) * _enemyWaveConfig.radius + targetPosition.x;
            calcEnemyPosition.y = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * _enemyWaveConfig.radius + targetPosition.y;
            calcEnemyPosition.z = targetPosition.z;

            var newEnemy= _enemiesViewServices.Instantiate(enemyShipView.gameObject).GetComponent<EnemyShip>();
            newEnemy.InjectHealth(new Health(3,3));
            newEnemy.Health.OnLethalDamage += () => DestroyEnemy(newEnemy);
            _enemyShipsViews.Add(newEnemy);
            newEnemy.transform.position = calcEnemyPosition;
                
            float angle = Mathf.Atan2(targetPosition.y - newEnemy.transform.position.y, targetPosition.x- newEnemy.transform.position.x) * Mathf.Rad2Deg;
            newEnemy.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle+90));
        }
        
        private void DestroyEnemy(EnemyShip enemyShipView)
        {
            _enemyShipsViews.Remove(enemyShipView); 
            _enemiesViewServices.Destroy(enemyShipView.gameObject);
        }

        public void Dispose()
        {
            foreach (var enemy in Object.FindObjectsOfType<EnemyShip>())
            {
                _enemyShipsViews.Remove(enemy);
                Object.Destroy(enemy.gameObject);
            }
        }
    }
}