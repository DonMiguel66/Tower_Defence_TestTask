using System.Globalization;
using Enums;
using UnityEngine;
using Views;

namespace Controllers
{
    internal class UIController
    {
        private UIView _uiView;
        private MainBase _mainBaseView;
        private PlayerBaseController _playerBaseController;

        public UIController(UIView uiView, PlayerBaseController playerBaseController, MainBase mainBaseView)
        {
            _uiView = uiView;
            _mainBaseView = mainBaseView;
            _playerBaseController = playerBaseController;
            _uiView.Init(UpgradeShootingDamage, UpgradeShootingSpeed, UpgradeShootingRange);
            _uiView.CurrentDamageLabelInfo.text = _mainBaseView.CurrentDamage.ToString();
            _uiView.CurrentSpeedLabelInfo.text = _mainBaseView.ShootingSpeedDelay.ToString();
            _uiView.CurrentRangeLabelInfo.text = _mainBaseView.ShootingRange.ToString(CultureInfo.InvariantCulture);
        }
        
        private void UpgradeShootingSpeed()
        {
            switch (_playerBaseController.ShootingSpeedLevel.CurrentLevel)
            {
                case Level.Level1:
                    _playerBaseController.UpgradeShootingSpeedTo(Level.Level2);
                    _uiView.CurrentSpeedLabelInfo.text =_mainBaseView.ShootingSpeedDelay.ToString();
                    Debug.Log("Level 2");
                    break;
                case Level.Level2:
                    _playerBaseController.UpgradeShootingSpeedTo(Level.Level3);
                    _uiView.CurrentSpeedLabelInfo.text = _mainBaseView.ShootingSpeedDelay.ToString();
                    Debug.Log("Level 3");
                    break;
                case Level.Level3:
                    _uiView.CurrentSpeedLabelInfo.text = _mainBaseView.ShootingSpeedDelay.ToString();
                    Debug.Log("Maximum level reached");
                    break;
            }
        }
        
        private void UpgradeShootingRange()
        {
            switch (_playerBaseController.ShootingRangeLevel.CurrentLevel)
            {
                case Level.Level1:
                    _playerBaseController.UpgradeShootingRangeTo(Level.Level2);
                    _uiView.CurrentRangeLabelInfo.text = _mainBaseView.ShootingRange.ToString(CultureInfo.InvariantCulture);
                    Debug.Log("Level 2");
                    break;
                case Level.Level2:
                    _playerBaseController.UpgradeShootingRangeTo(Level.Level3);
                    _uiView.CurrentRangeLabelInfo.text = _mainBaseView.ShootingRange.ToString(CultureInfo.InvariantCulture);
                    Debug.Log("Level 3");
                    break;
                case Level.Level3:
                    _uiView.CurrentRangeLabelInfo.text = _mainBaseView.ShootingRange.ToString(CultureInfo.InvariantCulture);
                    Debug.Log("Maximum level reached");
                    break;
            }
        }
        
        private void UpgradeShootingDamage()
        {
            switch (_playerBaseController.DamageLevel.CurrentLevel)
            {
                case Level.Level1:
                    _playerBaseController.UpgradeDamageTo(Level.Level2);
                    _uiView.CurrentDamageLabelInfo.text = _mainBaseView.CurrentDamage.ToString();
                    Debug.Log("Level 2");
                    break;
                case Level.Level2:
                    _playerBaseController.UpgradeDamageTo(Level.Level3);
                    _uiView.CurrentDamageLabelInfo.text = _mainBaseView.CurrentDamage.ToString();
                    Debug.Log("Level 3");
                    break;
                case Level.Level3:
                    _uiView.CurrentDamageLabelInfo.text = _mainBaseView.CurrentDamage.ToString();
                    Debug.Log("Maximum level reached");
                    break;
            }
        }
    }
}