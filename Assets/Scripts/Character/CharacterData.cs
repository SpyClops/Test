using UnitSystem;
using UnityEngine.UI;

public class CharacterData : UnitData
{
    private Image CharacterPanel;

    private void Start()
    {
        CharacterPanel = UIManager.Instance.CharacterPanel;
        hpBar = CharacterPanel;
        ShowHp();
        OnRegen += ShowHp;
    }

    private void ShowHp()
    {
        hpBar.fillAmount = (float) Stats.CurrentHealth / Stats.stats.health;
    }
}
