using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.UI;

public class EnemyData : UnitData
{
    private Image _enemyPanel;

    private void Start()
    {
        _enemyPanel = UIManager.Instance.EnemyPanel;
        hpBar = _enemyPanel;
    }
}
