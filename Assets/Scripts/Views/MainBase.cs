using Models;
using UnityEngine;
using Utils;

namespace Views
{
    internal class MainBase : Base
    {
        private CheckEnemyZone _checkCheckEnemyZone;

        private CircleBorderRenderer _circleBorderRenderer;
        public float CurrentRadius { get; set; }
        public CheckEnemyZone CheckEnemyZone => _checkCheckEnemyZone;

        public CircleBorderRenderer BorderRenderer => _circleBorderRenderer;

        private void Awake()
        {
            _checkCheckEnemyZone = GameObject.Find("CheckEnemyZone").GetComponent<CheckEnemyZone>();
            _circleBorderRenderer = GetComponent<CircleBorderRenderer>();
        }
    }
}