using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerControler : MonoBehaviour
{
    [Header("컨트롤 부속")]
    public Rigidbody rb;
    public Vector2 playerDir;
    public Ray mouseRay;
    public GunBase[] playerWeapons = new GunBase[3];
    public int nowWeapon = 2;
    [Header("피직스 관련")]
    public Collider[] itemSencer; //아이템 인식
    public physicsPlus.EnhancedPhysics<IItemBase> physicsPlus = new physicsPlus.EnhancedPhysics<IItemBase>();
    [Header("플레이어 스텟")]
    public PlayerOnlyStatus stat;
    public float rotationSpeed = 500;
    [Header("타이머")]
    public float playerAttackTimer = 0;
    public float dodgeCooldown = 0 ;
    public bool nonControllable;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Managers.GameManager.BasicPlayerStats(()=> 
        {
            stat = Managers.GameManager.PlayerStat;
            stat.states.SetGeneralFSMDefault(ref stat.animator, this.gameObject);
            stat.states.SetPlayerFSMDefault(stat.animator, this.gameObject);
            stat.nowState = stat.states["idle"];
            Managers.DataManager.testTDataBase = Managers.DataManager.Datas["TempBulletTable"] as BulletSCROBJ;
        });
    }

    private void Update()
    {
        if (!nonControllable)
        {
            playerDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            Vector3 movement = new Vector3(playerDir.x, 0, playerDir.y) * stat.moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                Quaternion rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = rotation;
                if (stat.states.ContainsKey("run") && stat.nowState != stat.states["run"] && isInAttacking())
                {
                    fsmChanger(stat.states["run"]);
                }
            }
            else if (playerDir == Vector2.zero)
            {
                if (stat.states.ContainsKey("idle") && stat.nowState != stat.states["idle"]&& stat.nowState != stat.states["attack"])
                {
                    fsmChanger(stat.states["idle"]);
                }
            }

            if (stat.states.ContainsKey("dodge") && stat.nowState != stat.states["dodge"])
            {
                if (Input.GetKey(KeyCode.Mouse0) && (1f / (stat.attackSpeed+playerWeapons[nowWeapon].stat.fireSpeed)) < playerAttackTimer)
                {
                    GetMousePos();
                    fsmChanger(stat.states["attack"]);
                    playerAttackTimer = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && dodgeCooldown >= stat.dodgeCooltime)
            {
                dodgeCooldown = 0;
                fsmChanger(stat.states["dodge"]);
            } 
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            nowWeapon = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nowWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            nowWeapon = 2;
        }
        if (PlayerGetItem())
        {

        }
        playerAttackTimer += Time.deltaTime;
        dodgeCooldown += Time.deltaTime;

    }
    public void GetMousePos()
    {
        RaycastHit hit;
        float rotTemp = 0;
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay,out hit,Mathf.Infinity,8))
        {
            AttackPoint(hit.point, ref rotTemp, () => 
            {
                if (Managers.DataManager.Datas.TryGetValue(playerWeapons[nowWeapon].stat.name+"_Bullet", out UnityEngine.Object Result))
                {
                    Debug.Log(Result.ToString());
                    GameObject bulletTemp = Managers.Pool.Pop(Result.GameObject());
                    bulletTemp.GetComponent<Bullet>().BulletSetting(playerWeapons[nowWeapon].stat, playerWeapons[nowWeapon].GetTotalDamage(stat.attackDamage,stat.criticalDamage,stat.criticalChance));
                    bulletTemp.transform.position = transform.position;
                    bulletTemp.transform.rotation = Quaternion.Euler(bulletTemp.transform.rotation.eulerAngles.x, rotTemp, bulletTemp.transform.rotation.eulerAngles.z);
                }
                
            });
        }
    }
    public byte whatIsEmptySlot()
    {
        for (byte i = 0; i < playerWeapons.Length; i++)
        {
            if (playerWeapons[i].stat.isEmptySlot())
            {
                return i;
            }
        }
        return 255;
    }
    public bool PlayerGetItem()
    {
        Debug.Log("함수들어옴");
        if (physicsPlus.IsChangedInArray(itemSencer, transform.position , 2, 8))
        {
            Debug.Log("변경있음");
            itemSencer = Physics.OverlapSphere(transform.position , 2, 256);
            if (physicsPlus.SearchTheComponent(itemSencer,out IItemBase target,"Item"))
            {
                Debug.Log("빈칸찾음");
                if (whatIsEmptySlot()!= 255)
                {
                    Debug.Log("총먹음");
                    target.UseItem<GunBase>(ref playerWeapons[whatIsEmptySlot()]);
                    Debug.Log(target);
                    return false;
                }

                Debug.Log(target);
                return true;
            }
        }
        return false;
    }
    public void functionTest()
    {
        Debug.Log("asd");
    }
    public void AttackPoint(Vector3 TargetPos,ref float quatTemp,Action Time = null) 
    {
        TargetPos = new Vector3(TargetPos.x - transform.position.x,TargetPos.z - transform.position.z);
        quatTemp = ((MathF.Atan2(TargetPos.y, TargetPos.x)*Mathf.Rad2Deg)-90)*-1;
        transform.rotation = Quaternion.Euler(0, quatTemp, 0);
        Time?.Invoke();
    }
    #region fsm 중계기를 만들어서 변수로 참조해와야함
    public void fsmChanger(BaseState BS)
    {
        if (BS != stat.nowState)
        {
            stat.nowState.OnStateExit();
            stat.nowState = BS;
            stat.nowState.OnStateEnter();
            if (BS == stat.states["dodge"]/*여기다가 추후 추가될 정지 애니메이션*/)
            {
                nonControllable = true;
                rb.velocity = Vector3.zero;
                StartCoroutine(dodgeTimer());
            }
            else if(BS == stat.states["attack"])
            {
                StartCoroutine(animTimer());
            }
        }
    }

    public IEnumerator dodgeTimer()
    {
        transform.rotation = Quaternion.Euler(0, ((MathF.Atan2(playerDir.y, playerDir.x)*Mathf.Rad2Deg- 90)*-1), 0);
        if (playerDir.x== 0&&playerDir.y==0)
        {
            rb.AddForce(Vector3.forward * (stat.moveSpeed * 5), ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(new Vector3(playerDir.x, 0, playerDir.y) * (stat.moveSpeed*5), ForceMode.Impulse);
        }
        yield return null;
        Debug.Log(stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitUntil(() => stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        rb.velocity = Vector3.zero;
        Debug.Log("애님끝남");
        nonControllable = false;
        fsmChanger(stat.states["idle"]);
    }
    public IEnumerator animTimer()
    {
        yield return null;
        Debug.Log(stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitUntil(() => stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        fsmChanger(stat.states["idle"]);
    }
    public bool isInAttacking()
    {
        if (stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && stat.nowState==stat.states["attack"])
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    #endregion
}
