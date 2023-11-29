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

            if (animator.gameObject.activeSelf)
            {
                Debug.Log("달리기");
                animator.Play("run", 0);
            }
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
            Debug.Log("여기다가 UI매니저에서 사망 메뉴 호출");
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
            if (animator.gameObject.activeSelf)
            {
                animator.Play("idle", 0);
            }
            
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
namespace MonsterFSM 
{
    public class Walk : BaseState
    {
        public override void OnStateEnter()
        {
            animator.Play("walk", 0);
            Debug.Log("도착");
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
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
namespace BossFSM
{
    
    public class Simple_pattern1 : BaseState
    {
        string pattern_name;
        public Simple_pattern1(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Simple_pattern2 : BaseState
    {
        string pattern_name;
        public Simple_pattern2(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Simple_pattern3 : BaseState
    {
        string pattern_name;
        public Simple_pattern3(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Simple_pattern4 : BaseState
    {
        string pattern_name;
        public Simple_pattern4(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Simple_pattern5 : BaseState
    {
        string pattern_name;
        public Simple_pattern5(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Simple_pattern6 : BaseState
    {
        string pattern_name;
        public Simple_pattern6(string name)
        {
            pattern_name = name;
            Debug.Log(name);
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Attack_to_Idle : BaseState
    {
        string pattern_name;
        public Attack_to_Idle(string name)
        {
            pattern_name = name;
            Debug.Log(name);
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class IdleToAttack : BaseState
    {
        string pattern_name;
        public IdleToAttack(string name)
        {
            pattern_name = name;
            Debug.Log(name);
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Form_return : BaseState
    {
        string pattern_name;
        public Form_return(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0, 0);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Heal_idle : BaseState      //나중에 다른 네임스페이스로 옮길 것
    {
        
        public override void OnStateEnter()
        {
            Debug.Log("힐 아이들");
            animator.Play("heal_idle", 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Hard_pattern1 : BaseState
    {
        string pattern_name;
        public Hard_pattern1(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Hard_pattern2 : BaseState
    {
        string pattern_name;
        public Hard_pattern2(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Hard_pattern3 : BaseState
    {
        string pattern_name;
        public Hard_pattern3(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
    public class Hard_pattern4 : BaseState
    {
        string pattern_name;
        public Hard_pattern4(string name)
        {
            pattern_name = name;
        }
        public override void OnStateEnter()
        {
            animator.Play(pattern_name, 0);

        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }
}