using System.Collections.Generic;
using Configs;
using Interfaces;
using UnityEngine;
using Utils;
using Views;

namespace Controllers
{
    internal class EnemiesController : IExecutable, IFixedExecutable
    {
        private EnemySpawnController _enemySpawnController;
        private EnemySpawnConfig _enemySpawnConfig;
        private EnemyWaveConfig _enemyWaveConfig;
        private ViewServices _enemiesViewServices;
        public EnemySpawnController EnemySpawnController => _enemySpawnController;
        private SpriteAnimatorController _spriteAnimatorController;

        private List<EnemyShip> _enemyShipsViews;
        public EnemiesController(ViewServices enemiesViewServices, EnemySpawnConfig enemySpawnConfig, EnemyWaveConfig enemyWaveConfig, SpriteAnimatorController spriteAnimatorController)
        {
            _enemiesViewServices = enemiesViewServices;
            _spriteAnimatorController = spriteAnimatorController;
            _enemySpawnConfig = enemySpawnConfig;
            _enemyWaveConfig = enemyWaveConfig;
            _enemySpawnController = new EnemySpawnController(_enemiesViewServices,_enemySpawnConfig, _enemyWaveConfig,_spriteAnimatorController);
            _enemyShipsViews = _enemySpawnController.EnemyShipsViews;
        }

        public async void Init()
        {
            await _enemySpawnController.EnemyRespawn();
        }
        
        public void Execute()
        {
            _spriteAnimatorController.Execute();
            foreach (var enemyShipsView in _enemyShipsViews.ToArray())
            {
                if(enemyShipsView.isActiveAndEnabled)
                    SetEnemyDirection(enemyShipsView);
            }
        }

        public void FixedExecute()
        {
            foreach (var enemyShipsView in _enemyShipsViews.ToArray())
            {
                if(enemyShipsView.isActiveAndEnabled)
                    EnemyMoving(enemyShipsView);
            }
        }

        private void SetEnemyDirection(EnemyShip enemyShipView)
        {
            Vector3 direction = enemyShipView.Target.transform.position - enemyShipView.transform.position;
            direction.Normalize();
            enemyShipView.Movement = direction;
        }

        private void EnemyMoving(EnemyShip enemyShipView)
        {
            enemyShipView.Rigidbody.MovePosition((Vector2)enemyShipView.transform.position + (enemyShipView.Movement * (enemyShipView.Speed * Time.deltaTime)));
        }
    }
}