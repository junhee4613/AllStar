using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [SerializeField]
    protected Collider[] playerSence;
    public float Detect_Range = 0;
    [Header("공격모드 일 때 감지 범위")]
    public float move_Detect_Range;
    [Header("일반모드 일 때 감지 범위")]
    public float idle_Detect_Range;
    public GameObject player = null;
    public Status monsterStatus;
    public bool chase_player;
    //public bool look_player = false;
    public float attack_time = 0f;
    public bool action_start = true;
    public float action_delay = 0;
    public float action_delay_init = 0;
    public enum Monster_type    //몬스터 타입에 따라 아이템 드랍하는 종류를 정해주기 위해 enum을 씀
    {
        DOG,
        POLICE,
        DRAGUN,
        TURRET,
    }
    public Monster_type monster_type;


    protected virtual void Awake()
    {
        monsterStatus.states.SetGeneralFSMDefault(ref monsterStatus.animator, this.gameObject);
        monsterStatus.nowState = monsterStatus.states["idle"];
        Detect_Range = idle_Detect_Range;
        player = GameObject.FindGameObjectWithTag("Player");
        action_delay = action_delay_init;
        this.gameObject.name = this.gameObject.GetHashCode().ToString();
        Managers.GameManager.monstersInScene.Add(this.gameObject.name, monsterStatus);
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (monsterStatus.nowHP <= 0)
        {
            MonsterDie();
        }
        else
        {
            monsterStatus.nowState = monsterStatus.states["damaged"];
        }
        if(monster_type != Monster_type.TURRET)
        {
            attack_time += Time.deltaTime;
        }
        if (monsterStatus.nowState != monsterStatus.states["die"])
        {
            playerSence = Physics.OverlapSphere(transform.position, Detect_Range, 128);
            if (playerSence.Length != 0)
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
    #region 플레이어 따라가며 공격 로직
    public virtual void Perceive_player()
    {
        if (Detect_Range != move_Detect_Range)
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
                Debug.Log("공격 시작");
                action_start = false;
                fsmChanger(monsterStatus.states["idle"]);
            }
        }
    }
    public void Status_Init()
    {
        if (Detect_Range != idle_Detect_Range)
        {
            Detect_Range = idle_Detect_Range;
        }
    }
    public float LookPlayer(GameObject hit)
    {
        float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        return target;
    }

    /*public IEnumerator animTimer()
    {
        yield return null;
        yield return new WaitUntil(() => monsterStatus.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        look_player = false;
        attack_time = 0;
        fsmChanger(monsterStatus.states["idle"]);
    }*/
    public virtual void MonsterDie()
    {
        monsterStatus.nowState = monsterStatus.states["die"];
        Debug.Log("죽음2");
        Managers.Pool.Push(this.gameObject);
    }
    public void WeaponDropKind()
    {
        switch (monster_type)
        {
            case Monster_type.DOG:

                break;
            case Monster_type.POLICE:

                break;
            case Monster_type.DRAGUN:

                break;
            case Monster_type.TURRET:

                break;
            default:
                break;
        }
    }
    public void MonsterPoolPush()
    {
       
    }
}