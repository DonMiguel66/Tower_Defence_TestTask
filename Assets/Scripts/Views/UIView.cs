using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Views
{
    internal class UIView : MonoBehaviour
    {
        [SerializeField] private Button _upgradeDamageBtn;
        [SerializeField] private Button _upgradeSpeedBtn;
        [SerializeField] private Button _upgradeRangeBtn;

        [SerializeField] private TMP_Text _currentSpeedLabelInfo;
        [SerializeField] private TMP_Text _currentRangeLabelInfo;
        [SerializeField] private TMP_Text _currentDamageLabelInfo;

        public TMP_Text CurrentSpeedLabelInfo => _currentSpeedLabelInfo;

        public TMP_Text CurrentRangeLabelInfo => _currentRangeLabelInfo;

        public TMP_Text CurrentDamageLabelInfo => _currentDamageLabelInfo;

        public void Init(UnityAction doUpgradeDamage, UnityAction doUpgradeSpeed, UnityAction doUpgradeRange)
        {
            _upgradeDamageBtn.onClick.AddListener(doUpgradeDamage);
            _upgradeSpeedBtn.onClick.AddListener(doUpgradeSpeed);
            _upgradeRangeBtn.onClick.AddListener(doUpgradeRange);
        }
    }
}