using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseState
{
    public Animator animator;
    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}
namespace FSMSetUp
{

}
namespace GeneralFSM
{
    public class Run : BaseState
    {
        public override void OnStateEnter()
        {
            animator.Play("run",0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            
        }
    }
    public class Attack : BaseState
    {
        public override void OnStateEnter()
        {
            animator.Play("attack", 0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Die : BaseState
    {
        public override void OnStateEnter()
        {
            animator.Play("die", 0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Damaged : BaseState
    {
        public override void OnStateEnter()
        {
            animator.Play("damaged", 0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Idle : BaseState
    {
        public override void OnStateEnter()
        {
            Debug.Log("idle¡¯¿‘");
            animator.Play("idle", 0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
}
namespace EpicMonsterFSM
{

}
namespace CommonMonsterFSM
{

}
namespace PlayerFSM
{
    public class Dodge : BaseState
    {
        public override void OnStateEnter()
        {
            animator.Play("dodge", 0);
        }
        public override void OnStateUpdate()
        {
            
        }
        public override void OnStateExit()
        {
            
        }
    }
}