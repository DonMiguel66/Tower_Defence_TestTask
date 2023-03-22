using Configs;
using UnityEngine;
using Utils;
using Views;

namespace Controllers
{
    internal class GameController : MonoBehaviour
    {
        [SerializeField] private EnemySpawnConfig _enemySpawnConfig;
        [SerializeField] private EnemyWaveConfig _enemyWaveConfig;
        [SerializeField] private PlayerBaseConfig _playerBaseConfig;
        [SerializeField] private SpriteAnimatorConfig _spriteAnimatorConfig;

        private EnemiesController _enemiesController;
        private PlayerBaseController _playerBaseController;
        private UIController _uiController;
        private SpriteAnimatorController _spriteAnimatorController;
        
        private ViewServices _enemiesViewServices;
        private ViewServices _shellsViewServices;
        private MainBase _mainBaseView;
        private UIView _uIView;
        private void Awake()
        {
            _mainBaseView = GameObject.Find("MainBase").GetComponent<MainBase>();
            _uIView = GameObject.Find("UICanvas").GetComponent<UIView>();
            if (_spriteAnimatorConfig)
                _spriteAnimatorController = new SpriteAnimatorController(_spriteAnimatorConfig);
            _enemiesViewServices = new ViewServices();
            _shellsViewServices = new ViewServices();
            _enemiesController = new EnemiesController(_enemiesViewServices,_enemySpawnConfig,_enemyWaveConfig,_spriteAnimatorController);
            _playerBaseController = new PlayerBaseController(_shellsViewServices,_enemiesViewServices,_playerBaseConfig, _enemyWaveConfig,_mainBaseView);
            _uiController = new UIController(_uIView,_playerBaseController, _mainBaseView);
            _enemiesController.Init();
            _mainBaseView.OnEnemyHitBase += OnGameOver;
        }

        private void Update()
        {
            _enemiesController.Execute();
            _playerBaseController.Execute();
        }

        private void FixedUpdate()
        {
            _enemiesController.FixedExecute();
            _playerBaseController.FixedExecute();
        }

        private void OnGameOver()
        {
            Time.timeScale = 0;
        }
    }
}