using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [SerializeField]
    protected Collider[] playerSence;
    public float Detect_Range;
    public float move_Detect_Range;
    public float idle_Detect_Range;
    public Status monsterStatus;
    public float attack_Distance;
    public bool chase_player;

    protected virtual void Awake()
    {
        monsterStatus.states.SetGeneralFSMDefault(ref monsterStatus.animator, this.gameObject);
        monsterStatus.nowState = monsterStatus.states["idle"];
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (monsterStatus.nowState != monsterStatus.states["die"])
        {
            playerSence = Physics.OverlapSphere(transform.position, Detect_Range, 128);
            if(playerSence.Length != 0)
            {
                Perceive_player();
                chase_player = true;
            }
            else
            {
                Status_Init();
                chase_player = false;
            }
        }
    }
    public void getDamage(float damage)
    {
        if (monsterStatus.HP - damage <= 0)
        {
            monsterStatus.HP -= damage;
            monsterStatus.nowState = monsterStatus.states["die"];
        }
        else
        {
            monsterStatus.HP -= damage;
            monsterStatus.nowState = monsterStatus.states["damaged"];
        }
    }
    #region 플레이어 따라가며 공격 로직
    public virtual void Perceive_player()
    {
        if(Detect_Range != move_Detect_Range)
        {
            Detect_Range = move_Detect_Range;
        }
    }
#endregion
    public void fsmChanger(BaseState BS)
    {
        if (BS != monsterStatus.nowState)
        {
            monsterStatus.nowState.OnStateExit();
            monsterStatus.nowState = BS;
            monsterStatus.nowState.OnStateEnter();

            if (BS == monsterStatus.states["attack"])
            {
                StartCoroutine(animTimer());
            }
        }
    }
    public void Status_Init()
    {
        if(Detect_Range != idle_Detect_Range)
        {
            Detect_Range = idle_Detect_Range;
        }
        //hp를 원래대로 돌리는 로직 추가
    }
    public IEnumerator animTimer()
    {
        yield return null;
        yield return new WaitUntil(() => monsterStatus.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        fsmChanger(monsterStatus.states["idle"]);
    }
}
