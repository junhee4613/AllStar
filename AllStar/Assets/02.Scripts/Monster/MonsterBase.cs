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
    public bool look_player = false;
    public float attack_time = 0f;

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
        attack_time += Time.deltaTime;
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
                look_player = true;
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
        look_player = false;
        attack_time = 0;
        fsmChanger(monsterStatus.states["idle"]);
    }
    /*public void Follow()
    {

        if (Physics.SphereCast(transform.position, Detect_Range_Free, transform.forward, out RaycastHit hit, Detect_Range_Fix, 128))
            test = Physics.OverlapSphere(transform.position, Detect_Range_Free, 128);
        foreach (var item in test)
        {
            Debug.Log(hit.collider.gameObject.name);
            monster_Motion = MonsterPaattern.RUN;
            gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(hit), transform.rotation.z);
            if (item.name == "Player")
            {
                monster_Motion = MonsterPaattern.RUN;
                gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, LookPlayer(item), transform.rotation.z);
                break;
            }

        }
        else
        if (test.Length == 0)
        {
            monster_Motion = MonsterPaattern.STOP;
        }
    }
    public float LookPlayer(RaycastHit hit)
    {           //-90을 해준다 인스펙터 창에서 전역 기준으로 앞을 볼 때 Y축 회전이 0인데 0.1의 아크탄젠트를 각도로 표현하면 90이 나오기 때문이다.
        //몬스터 기준으로 타겟의 좌표를 구해야돼서 이거 생각해서 수정하기
        float target = Mathf.Atan2(hit.transform.position.z - transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
        public float LookPlayer(Collider hit)
        {

            float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
            float distanceX = hit.transform.position.x - transform.position.x;
            float distancez = hit.transform.position.z - transform.position.z;
            if (Mathf.Abs(distanceX) < 1 && Mathf.Abs(distancez) < 1)
            {
                monster_Motion = MonsterPaattern.STOP;
            }
            else
            {
                transform.position += transform.forward * 5 * Time.deltaTime;
            }
            *//*if (Mathf.Abs(gameObject.transform.position.z - hit.transform.position.z) < 1
                && Mathf.Abs(hit.transform.position.x - transform.position.x) < 1)
            {
            }*//*
            return target;
        }*/
    }
