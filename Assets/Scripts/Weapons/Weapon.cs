using Character;
using UnitSystem;
using UnityEngine;
using Random = System.Random;

public class Weapon : MonoBehaviour
{
    public class AttackData
        {
            public UnitData Target => m_Target;
            public UnitData Source => m_Source;
            
            UnitData m_Target;
            UnitData m_Source;
        
            int[] m_Damages = new int[System.Enum.GetValues(typeof(StatSystem.DamageType)).Length];
           
            public AttackData(UnitData target, UnitData source = null)
            {
                m_Target = target;
                m_Source = source;
            }

            public int AddDamage(StatSystem.DamageType damageType, int amount)
            {
                int addedAmount = amount;

                if (damageType == StatSystem.DamageType.Physical)
                {
                    if(m_Source !=  null)
                        addedAmount += Mathf.FloorToInt(addedAmount * m_Source.Stats.stats.strength * 0.01f);
                
                    addedAmount = Mathf.Max(addedAmount - m_Target.Stats.stats.defense, 1);
                }
            
            
                m_Damages[(int)damageType] += addedAmount;

                return addedAmount;
            }

            public int GetDamage(StatSystem.DamageType damageType)
            {
                return m_Damages[(int)damageType];
            }

            public int GetFullDamage()
            {
                int totalDamage = 0;
                for (int i = 0; i < m_Damages.Length; ++i)
                {
                    totalDamage += m_Damages[i];
                }

                return totalDamage;
            }
        }
    
        [System.Serializable]
        public struct Stat
        {
            public float Speed;
            public int MinimumDamage;
            public int MaximumDamage;
            public float MaxRange;
            public float AttackCoolDown;
        }

        public Stat Stats = new Stat(){ Speed = 1.0f, MaximumDamage = 1, MinimumDamage = 1};

        public void Attack(UnitData attacker, UnitData target)
        {
            AttackData attackData = new AttackData(target, attacker);

            int damage = new Random().Next(Stats.MinimumDamage, Stats.MaximumDamage + 1);

            attackData.AddDamage(StatSystem.DamageType.Physical, damage);
            
            target.Damage(attackData);
        }
        
        public bool CanHit(UnitData attacker, UnitData target)
        {
            if (Vector3.SqrMagnitude(attacker.transform.position - target.transform.position) < Stats.MaxRange * Stats.MaxRange)
            {
                return true;
            }

            return false;
        }
}
