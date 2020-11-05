using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image EnemyPanel;
    public Image CharacterPanel;
    
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }
}