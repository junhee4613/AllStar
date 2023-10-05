using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [SerializeField]
    protected Collider[] playerSence;
    public float Detect_Range = 0;
    public float move_Detect_Range;
    public float idle_Detect_Range;
    public GameObject player = null;
    public Status monsterStatus;
    public bool chase_player;
    public bool look_player = false;
    public float attack_time = 0f;

    protected virtual void Awake()
    {
        monsterStatus.states.SetGeneralFSMDefault(ref monsterStatus.animator, this.gameObject);
        monsterStatus.nowState = monsterStatus.states["idle"];
        Detect_Range = idle_Detect_Range;
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (Detect_Range != move_Detect_Range)
        {
            Detect_Range = move_Detect_Range;
        }
        Debug.Log("움직임");
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
                Debug.Log("공격 시작");
                StartCoroutine(animTimer());
            }
        }
    }
    public void Status_Init()
    {
        if (Detect_Range != idle_Detect_Range)
        {
            Detect_Range = idle_Detect_Range;
        }
        Debug.Log("초기화");
    }
    public float LookPlayer(GameObject hit)
    {
        float target = Mathf.Atan2(transform.position.z - hit.transform.position.z, hit.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90;
        return target;
    }
    public IEnumerator animTimer()
    {
        yield return null;
        yield return new WaitUntil(() => monsterStatus.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        look_player = false;
        attack_time = 0;
        fsmChanger(monsterStatus.states["idle"]);
    }
}