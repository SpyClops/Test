using System;
using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private CharacterData characterData;
    public CharacterData Data => characterData;

    public enum State
    {
        DEFAULT,
        ATTACKING
    }
    [HideInInspector] public State CurrentState;
    
    private bool isDead = false;
    private float deathTimer;
    private Vector3 playerSpawn;
    
    public UnitData currentTargetCharacterData; //Текущая цель игрока
    private UnitData lastTegetCharacterData;
    
    private UIManager uiManager;
    private Renderer _renderer;
    
    private Camera mainCamera;
    public LayerMask ClickableLayer;
    public NavMeshAgent agent;
    
    private float curCoolDown;

    
    
    
    public void Awake() {
        agent = GetComponent<NavMeshAgent>();
        characterData = GetComponent<CharacterData>();
        mainCamera = Camera.main;
        characterData.Init();
        AttackState._characterControl = this;
        _renderer = GetComponent<Renderer>();
    }

    public void Start() {
        playerSpawn = transform.position;
        curCoolDown = 0;
    }

    public void Update() {
        // Key and mouse movement control
        //
        if (!isDead) {
            float horisontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horisontal, 0f, vertical);
        if (movement.normalized.magnitude >= 0.1f) {
            Vector3 moveDestination = transform.position + movement;
            agent.destination = moveDestination;
            if (CurrentState == State.ATTACKING) {
                CurrentState = State.DEFAULT;
                currentTargetCharacterData = null;
            }
            
            
        }
        else {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 50, ClickableLayer.value)) {
                
                bool enemyHit = false;
            
                if (hit.collider.gameObject.tag == "enemy") {
                    enemyHit = true;
                }
                
                if (Input.GetMouseButtonDown(0)) {
                    if (enemyHit) {
                        Transform enemyPos = hit.collider.gameObject.transform;
                        agent.destination = enemyPos.position;
                        if (currentTargetCharacterData) {
                            lastTegetCharacterData = currentTargetCharacterData;
                        }

                        currentTargetCharacterData = enemyPos.GetComponent<UnitData>();
                    }
                    else {
                        agent.destination = hit.point;
                        lastTegetCharacterData = currentTargetCharacterData;
                        currentTargetCharacterData = null;
                        if (CurrentState == State.ATTACKING) {
                            CurrentState = State.DEFAULT;
                            currentTargetCharacterData = null;
                        }

                    }
            
                }
            }
        }

        if (currentTargetCharacterData && characterData.CanAttackReach(currentTargetCharacterData) && CurrentState != State.ATTACKING) {
            agent.Stop();
            agent.ResetPath();
                    
            var targetDirection = (currentTargetCharacterData.transform.position - transform.position).normalized;
            targetDirection.y = 0f;
            transform.rotation = Quaternion.LookRotation(targetDirection);
                
            
            CurrentState = State.ATTACKING;
            
        }

        if (CurrentState == State.ATTACKING ) {
            characterData.Attack(currentTargetCharacterData);
            if (!characterData.TargetIsLive(currentTargetCharacterData)) {
                CurrentState = State.DEFAULT;
                currentTargetCharacterData = null;
            }
            
        }
        }
        
        
        
        
        //Respawn
        else if (isDead) {
            CurrentState = State.DEFAULT;
            deathTimer += Time.deltaTime;
            if (deathTimer > 3.0f)
                GoToRespawn();

            return;
        }

        if (characterData.Stats.CurrentHealth == 0) {
            isDead = true;
            _renderer.enabled = false;
            characterData.StartingWeapon.GetComponentInChildren<Renderer>().enabled = false;
            deathTimer = 0.0f;
        }
        //




    }
    
    void GoToRespawn() {
        _renderer.enabled = true;
        characterData.StartingWeapon.GetComponentInChildren<Renderer>().enabled = true;
        agent.Warp(playerSpawn);
        agent.isStopped = true;
        agent.ResetPath();
            
        isDead = false;
        currentTargetCharacterData = null;
        CurrentState = State.DEFAULT;
        
        characterData.Stats.ChangeHealth(characterData.Stats.stats.health);
    }
    

    
}
