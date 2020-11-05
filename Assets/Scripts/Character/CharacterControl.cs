using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class CharacterControl : MonoBehaviour
    {
        public static CharacterControl Instance { get; protected set; }
        public UnitData Data => characterData;
        
        public enum State
        {
            DEFAULT,
            ATTACKING
        }
        [HideInInspector] public State CurrentState;

        [SerializeField] private float _speed = 10.0f;
        
        private NavMeshAgent agent;
        private CharacterData characterData;
        
        private UnitData currentTargetCharacterData; //Текущая цель игрока
        private UnitData lastTegetCharacterData; //Предыдущая цель игрока

        private Vector3 playerSpawn;
        
        private bool isDead;
        
        private float turnSmoothVelocity;
        private float turnSmoothTime = 0.01f;
        private float deathTimer;

        private List<UnitData> enemieDatas = new List<UnitData>(); //Массив всех врагов поблизости 
        private UIManager uiManager;
        private Animator animator;
        private Weapon weapon;
        
        private void Awake()
        {
            Instance = this;
            characterData = GetComponent<CharacterData>();
            
            characterData.Init();
        }
        
        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();

            agent.speed = _speed;
            playerSpawn = transform.position;
            uiManager = UIManager.Instance;
            
            CurrentState = State.DEFAULT;
        }

        private void Update()
        {



            if (Input.GetMouseButtonDown(0))
            {
                TryFoundEnemy();

                if (currentTargetCharacterData && characterData.CanAttackReach(currentTargetCharacterData))
                {
                    StopAgent();
                    
                    var targetDirection = (currentTargetCharacterData.transform.position - transform.position).normalized;
                    targetDirection.y = 0f;
                    transform.rotation = Quaternion.LookRotation(targetDirection);
                
                    animator.SetTrigger("Attack");
                    CurrentState = State.ATTACKING;
                    characterData.Attack(currentTargetCharacterData);
                    characterData.AttackTriggered();
                }
            }
            else
                StopAgent();
        }

        
        
        private void StopAgent()
        {
            agent.velocity = Vector3.zero;
        }
        
        private void TryFoundEnemy()
        {
            if (enemieDatas.Count == 0 ||
                currentTargetCharacterData && enemieDatas.Contains(currentTargetCharacterData))
            {
                if (currentTargetCharacterData)
                    SwitchTarget();
                return;
            }
            
            
            float minDistance = 10000;

            foreach (var enemieData in enemieDatas)
            {
                if (!enemieData) continue;
                
                float enemyDistance = Vector3.SqrMagnitude(enemieData.transform.position - transform.position);

                if (!(enemyDistance < minDistance)) continue;
                
                minDistance = enemyDistance;
                currentTargetCharacterData = enemieData;
                SwitchTarget();
            }
        }

        private void SwitchTarget()
        {
            uiManager.EnemyPanel.gameObject.SetActive(true);
            uiManager.EnemyPanel.fillAmount = (float) currentTargetCharacterData.Stats.CurrentHealth /
                                                        currentTargetCharacterData.Stats.stats.health;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            UnitData target = other.GetComponent<UnitData>();
            if (target)
            {
                enemieDatas.Add(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            UnitData target = other.GetComponent<UnitData>();
            if (target)
            {
                enemieDatas.Remove(target);
            }
        }
    }
}
