using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Utils;
using Views;

namespace Controllers
{
    internal class BaseShootingController : IShooting, IFixedExecutable
    {
        private ViewServices _shellsViewServices;
        private ViewServices _enemiesViewServices;
        private PlayerBaseConfig _playerBaseConfig;
        private EnemyWaveConfig _enemyWaveConfig;
        private MainBase _mainBase;

        public List<Shell> ActiveShellsViews => _activeShellsViews;

        private readonly Transform _shootPoint;
        private readonly GameObject _shellPrefab;

        private List<Shell> _activeShellsViews = new List<Shell>();
        private List<Transform> _reachableEnemyTargets = new List<Transform>();
        private GameObject _currentAim;
        private EnemyShip _currentEnemyView;
        private IShooting _shootingImplementation;

        public BaseShootingController(ViewServices shellsViewServices,ViewServices enemiesViewServices,PlayerBaseConfig playerBaseConfig, EnemyWaveConfig  enemyWaveConfig, MainBase mainBase)
        {
            _mainBase = mainBase;
            _playerBaseConfig = playerBaseConfig;
            _enemyWaveConfig = enemyWaveConfig;
            _shellPrefab = _playerBaseConfig.shotPrefab;
            _shootPoint = _mainBase.transform;
            _shellsViewServices = shellsViewServices;
            _enemiesViewServices = enemiesViewServices;
            _mainBase.CurrentRadius = _mainBase.ShootingRange;
            CreatePoolOfShells(_enemyWaveConfig.countOfEnemies);
            _mainBase.CheckEnemyZone.OnEnemyEnter += CheckAreaForAim;
            _enemiesViewServices.OnViewDestroy += ResetAim;
        }

        private async void Shooting()
        {
            _currentEnemyView = _currentAim.GetComponent<EnemyShip>();
            for (int i = _mainBase.CurrentDamage; i <= _currentEnemyView.Health.Max; i++)
            {
                if (!_currentEnemyView.enabled) return;
                DoShot();
                await UniTask.Delay(_mainBase.ShootingSpeedDelay);
            }
        }

        public async void ResetAim()
        {
            await ResetAimTask();
        }

        private async UniTask ResetAimTask()
        {
            _currentAim = null;
            await UniTask.WaitForEndOfFrame(_mainBase);
            CheckAreaForAim();
        }
        public void FixedExecute()
        {
            
        }
        private void CreatePoolOfShells(int countOfShells)
        {
            for (int i = 0; i < countOfShells; i++)
            {
                var shell = _shellsViewServices.Instantiate(_shellPrefab).GetComponent<Shell>();
                _activeShellsViews.Add(shell);
            }
            for (int i = 0; i < countOfShells; i++)
            {
                _shellsViewServices.Destroy(_activeShellsViews[i].gameObject);
            }
        }
        private void CheckAreaForAim()
        {
            _mainBase.CheckEnemyZone.testValue = _mainBase.ShootingRange;
            if (_currentAim != null && _currentAim.activeSelf)
            {
                //Debug.Log("Check stopped");
                return;
            }
            var hitColliders = Physics2D.OverlapCircleAll(_mainBase.transform.position, _mainBase.ShootingRange);
            if (hitColliders.Length > 0)
            {
                GetReachableTargets(hitColliders);
                if(_reachableEnemyTargets.Count>0)
                {
                    _currentAim = GetNearestTarget().gameObject;
                    Shooting();
                }
                else
                {
                    _currentAim = null;
                }
            }
        }
        
        private void GetReachableTargets(Collider2D[] hitColliders)
        {
            _reachableEnemyTargets.Clear();
            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.GetComponent<EnemyShip>() && hitCollider.gameObject.activeSelf)
                    _reachableEnemyTargets.Add(hitCollider.transform);
            }
        }
        
        private Transform GetNearestTarget()
        {
            Transform nearestTarget = null;
            float nearestDistance = 0f;
            for (int i = 0; i < _reachableEnemyTargets.Count; i++)
            {
                float currentDistance = Vector3.Distance(_mainBase.transform.position, _reachableEnemyTargets[i].position);
                if (i == 0)
                {
                    nearestTarget = _reachableEnemyTargets[i];
                    nearestDistance = currentDistance;
                    continue;
                }
                if (currentDistance < nearestDistance)
                {
                    nearestTarget = _reachableEnemyTargets[i];
                    nearestDistance = currentDistance;
                }
            }
            return nearestTarget;
        }
        
        public void SetShellDirection(Shell shellView)
        {
            if(!ReferenceEquals(shellView.Target, null))
            {
                Vector3 direction = shellView.Target.transform.position - shellView.transform.position;
                direction.Normalize();
                shellView.Movement = direction;
            }
            else
            {
                _shellsViewServices.Destroy(shellView.gameObject);
            }
        }

        public void ShellMoving(Shell shellView)
        {
            shellView.Rigidbody.MovePosition((Vector2)shellView.transform.position + (shellView.Movement * (shellView.Speed * Time.deltaTime)));
        }

        private void OnShellHit(Shell shell)
        {
            _shellsViewServices.Destroy(shell.gameObject);
        }

        public void DoShot()
        {
            var currentShot = _shellsViewServices.Instantiate(_shellPrefab).GetComponent<Shell>();
            ActiveShellsViews.Add(currentShot);
            var transform = currentShot.transform;
            transform.position = _shootPoint.position;
            transform.rotation = _shootPoint.rotation;
            currentShot.InitShot(_mainBase.CurrentDamage, _currentAim, () =>OnShellHit(currentShot));
        }
    }
}