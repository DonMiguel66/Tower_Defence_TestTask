using System;
using Configs;
using Enums;
using Interfaces;
using Models;
using UnityEngine;
using Utils;
using Views;

namespace Controllers
{
    internal class PlayerBaseController : IExecutable, IFixedExecutable, IUpgradable
    {
        private PlayerBaseConfig _playerBaseConfig;
        private EnemyWaveConfig _enemyWaveConfig;
        private BaseShootingController _baseShootingController;
        private ViewServices _shellsViewServices;
        private ViewServices _enemiesViewServices;
        private MainBase _mainBaseView;
        
        private StatsLevel<int> _damageLevel;
        private StatsLevel<float> _shootingRangeLevel;
        private StatsLevel<int> _shootingSpeedLevel;
        
        public StatsLevel<int> DamageLevel
        {
            get => _damageLevel;
            set => _damageLevel = value;
        }

        public StatsLevel<float> ShootingRangeLevel
        {
            get => _shootingRangeLevel;
            set => _shootingRangeLevel = value;
        }

        public StatsLevel<int> ShootingSpeedLevel
        {
            get => _shootingSpeedLevel;
            set => _shootingSpeedLevel = value;
        }
        public BaseShootingController BaseShootingController => _baseShootingController;

        public MainBase MainBaseView => _mainBaseView;

        public event Action OnUpgradeStats;
        public event Action OnUpgradeShootingRange;

        public PlayerBaseController(ViewServices shellsViewServices, ViewServices enemiesViewServices, PlayerBaseConfig playerBaseConfig, EnemyWaveConfig enemyWaveConfig, MainBase mainBase)
        {
            _shellsViewServices = shellsViewServices;
            _enemiesViewServices = enemiesViewServices;
            _playerBaseConfig = playerBaseConfig;
            _enemyWaveConfig = enemyWaveConfig;
            _mainBaseView = mainBase;
            _baseShootingController = new BaseShootingController(_shellsViewServices,_enemiesViewServices,_playerBaseConfig, _enemyWaveConfig,MainBaseView);
            OnUpgradeStats += _baseShootingController.ResetAim;
            InitStats();
            _mainBaseView.BorderRenderer.DoRenderer(_playerBaseConfig.shootingRange);
        }

        private void InitStats()
        {
            _damageLevel.CurrentLevel = Level.Level1;
            _damageLevel.CurrentValue = _playerBaseConfig.shotDamage;
            MainBaseView.CurrentDamage = _damageLevel.CurrentValue;
            
            _shootingRangeLevel.CurrentLevel =Level.Level1;
            _shootingRangeLevel.CurrentValue =_playerBaseConfig.shootingRange;
            MainBaseView.ShootingRange = _shootingRangeLevel.CurrentValue;
            
            _shootingSpeedLevel.CurrentLevel =Level.Level1;
            _shootingSpeedLevel.CurrentValue =_playerBaseConfig.shootingDelay;
            MainBaseView.ShootingSpeedDelay =_shootingSpeedLevel.CurrentValue;
        }
        
        public void Execute()
        {
            if (_baseShootingController.ActiveShellsViews.Count <= 0) return;
            foreach (var shellsView in _baseShootingController.ActiveShellsViews.ToArray())
            {
                //Debug.Log("Check1");
                if(shellsView.isActiveAndEnabled)
                    _baseShootingController.SetShellDirection(shellsView); 
            }
        }

        public void FixedExecute()
        {
            _baseShootingController.FixedExecute();
            if (_baseShootingController.ActiveShellsViews.Count <= 0) return;
            foreach (var shellsView in _baseShootingController.ActiveShellsViews.ToArray())
            {
                //Debug.Log("Check2");
                if(shellsView.isActiveAndEnabled)
                    _baseShootingController.ShellMoving(shellsView); 
            }
        }
        
        public void UpgradeDamageTo(Level level)
        {
            int newDamage;
            switch (level)
            {
                case Level.Level1:
                    MainBaseView.CurrentDamage = _playerBaseConfig.shotDamage;
                    _damageLevel.CurrentLevel = Level.Level1;
                    Debug.Log(MainBaseView.CurrentDamage);
                    break;
                case Level.Level2:
                    newDamage = MainBaseView.CurrentDamage + _playerBaseConfig.increaseShotDamage;
                    Debug.Log($"New damage: {newDamage}");
                    MainBaseView.CurrentDamage = newDamage;
                    _damageLevel.CurrentLevel = Level.Level2;
                    Debug.Log(MainBaseView.CurrentDamage);
                    break;
                case Level.Level3:
                    newDamage = MainBaseView.CurrentDamage + _playerBaseConfig.increaseShotDamage;
                    Debug.Log($"New damage: {newDamage}");
                    MainBaseView.CurrentDamage =newDamage;
                    _damageLevel.CurrentLevel = Level.Level3;
                    Debug.Log(MainBaseView.CurrentDamage);
                    break;
            }
            OnUpgradeStats?.Invoke();
        }

        public void UpgradeShootingSpeedTo(Level level)
        {
            switch (level)
            {
                case Level.Level1:
                    MainBaseView.ShootingSpeedDelay = _playerBaseConfig.shootingDelay;
                    _shootingSpeedLevel.CurrentLevel = Level.Level1;
                    Debug.Log(MainBaseView.ShootingSpeedDelay);
                    break;
                case Level.Level2:
                    MainBaseView.ShootingSpeedDelay -= _playerBaseConfig.decreaseShootingDelayValue;
                    _shootingSpeedLevel.CurrentLevel = Level.Level2;
                    Debug.Log(MainBaseView.ShootingSpeedDelay);
                    break;
                case Level.Level3:
                    MainBaseView.ShootingSpeedDelay -= _playerBaseConfig.decreaseShootingDelayValue;
                    _shootingSpeedLevel.CurrentLevel = Level.Level3;
                    Debug.Log(MainBaseView.ShootingSpeedDelay);
                    break;
            }
            OnUpgradeStats?.Invoke();
        }

        public void UpgradeShootingRangeTo(Level level)
        {
            switch (level)
            {
                case Level.Level1:
                    MainBaseView.ShootingRange = _playerBaseConfig.shootingRange;
                    _shootingRangeLevel.CurrentLevel = Level.Level1;
                    Debug.Log(MainBaseView.ShootingRange);
                    break;
                case Level.Level2:
                    MainBaseView.ShootingRange += _playerBaseConfig.increaseShootingRangeValue;
                    _shootingRangeLevel.CurrentLevel = Level.Level2;
                    _mainBaseView.BorderRenderer.DoRenderer(MainBaseView.ShootingRange);
                    Debug.Log(MainBaseView.ShootingRange);
                    break;
                case Level.Level3:
                    MainBaseView.ShootingRange += _playerBaseConfig.increaseShootingRangeValue;
                    _shootingRangeLevel.CurrentLevel = Level.Level3;
                    _mainBaseView.BorderRenderer.DoRenderer(MainBaseView.ShootingRange);
                    Debug.Log(MainBaseView.ShootingRange);
                    break;
            }
            OnUpgradeStats?.Invoke();
            //ChangeChekZone();
        }

        private void ChangeChekZone()
        {
            var transform = _mainBaseView.CheckEnemyZone.transform;
            var checkZoneScale = transform.localScale;
            transform.localScale = new Vector3(checkZoneScale.x + _playerBaseConfig.increaseShootingRangeValue,
                checkZoneScale.y + _playerBaseConfig.increaseShootingRangeValue, checkZoneScale.z);
        }
    }
}