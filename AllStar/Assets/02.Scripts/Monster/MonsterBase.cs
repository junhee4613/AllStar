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
        if (monsterStatus.nowHP - damage <= 0)
        {
            monsterStatus.nowHP -= damage;
            monsterStatus.nowState = monsterStatus.states["die"];
        }
        else
        {
            monsterStatus.nowHP -= damage;
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
        //여기에 플레이어를 바로보고 네비메쉬가 실행되게 해야된다.(bool 등으로) 지금 네비메쉬가 일정거리이상 다가가면 공격이 나오는데 거리는 유지한채 뒤로 가면 플레이어를 보지 않고 공격을 한다.
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
        //상태를 초기화하는 로직 추가
    }
    public IEnumerator animTimer()
    {
        yield return null;
        yield return new WaitUntil(() => monsterStatus.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        fsmChanger(monsterStatus.states["idle"]);
    }
}
