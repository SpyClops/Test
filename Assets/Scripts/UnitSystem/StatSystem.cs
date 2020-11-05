using System;
using UnityEngine;

namespace UnitSystem
{
    
    [Serializable]
    public class StatSystem
    {
        public enum DamageType {
            Physical
        }
        
        [Serializable]
        public class Stats
        {
            public int health;
            public int defense;
            public int strength;
            public int agility;
            
            public void Copy(Stats other)
            {
                health = other.health;
                defense = other.defense;
                strength = other.strength;
            }
        }
        
        public Stats baseStats;
        public Stats stats { get; set; } = new Stats();
        public int CurrentHealth => currentHealth;
        public int currentHealth;
        
        UnitData m_Owner;
        
        public void Init(UnitData owner)
        {
            stats.Copy(baseStats);
            currentHealth = stats.health;
            m_Owner = owner;
        }
        
        public void ChangeHealth(int amount) {
            currentHealth = Mathf.Clamp(CurrentHealth + amount, 0, stats.health);
            
            float fillAmount = (float)CurrentHealth / stats.health;
            m_Owner.hpBar.fillAmount = fillAmount;
        }
        
        public void Damage(Weapon.AttackData attackData)
        {
            int totalDamage = attackData.GetFullDamage();
            ChangeHealth(-totalDamage);
            
        }
    }
}


