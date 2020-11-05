using System;
using Character;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    public static PlayerController _characterControl;

    public void Awake() {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterControl.CurrentState = PlayerController.State.DEFAULT;
    }

}
