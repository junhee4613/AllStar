using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using GeneralFSM;
using PlayerSkills.Skills;
using PlayerSkills.SkillProbs;

public class PlayerControler : MonoBehaviour
{
    [Header("컨트롤 부속")]
    public Rigidbody rb;
    public Vector2 playerDir;
    public Ray mouseRay;
    public GunBase[] playerWeapons;
    public int nowWeapon = 2;
    [SerializeField] private Transform firePos;
    [Header("피직스 관련")]
    public Collider[] itemSencer; //아이템 인식
    public physicsPlus.EnhancedPhysics<IItemBase> physicsPlus = new physicsPlus.EnhancedPhysics<IItemBase>();
    [Header("플레이어 스텟")]
    public PlayerOnlyStatus stat;
    public ArtifactSlot[] ownArtifacts;
    public float rotationSpeed = 500;
    [Header("타이머")]
    public float playerAttackTimer = 0;
    public float dodgeCooldown = 0;
    public bool nonControllable;
    [Header("쿨타임UI")]
    [SerializeField] private UnityEngine.UI.Image[] coolTimeIMG;//0공격,1회피,2345 스킬 각 qerv

    [Header("스킬")]
    [SerializeField] public SkillBase SkillTest = new SkillBase();
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerWeapons = Managers.GameManager.playerWeapons;
        ownArtifacts = Managers.GameManager.playerArtifacts;
        Managers.GameManager.BasicPlayerStats(() =>
        {
            for (int i = 0; i < playerWeapons.Length; i++)
            {
                playerWeapons[i] = new GunBase();
                playerWeapons[i].StartSetting();
            }
            for (int i = 0; i < ownArtifacts.Length; i++)
            {
                ownArtifacts[i] = new ArtifactSlot();
                ownArtifacts[i].ResetArtifact();
            }
            stat = Managers.GameManager.PlayerStat;
            stat.states.SetGeneralFSMDefault(ref stat.animator, this.gameObject);
            stat.states.SetPlayerFSMDefault(stat.animator, this.gameObject);
            stat.nowState = stat.states["idle"];
        });
    }

    private void Update()
    {
        if (!nonControllable && stat.nowHP > 0)
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
                if (stat.states.ContainsKey("idle") && stat.nowState != stat.states["idle"] && stat.nowState != stat.states["attack"])
                {
                    fsmChanger(stat.states["idle"]);
                }
            }

            if (stat.states.ContainsKey("dodge") && stat.nowState != stat.states["dodge"] && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetKey(KeyCode.Mouse0) && (1f / (stat.attackSpeed + playerWeapons[nowWeapon].stat.fireSpeed)) < playerAttackTimer && playerWeapons[nowWeapon].stat.weaponIndex != 254)
                {
                    GetMousePos();
                    fsmChanger(stat.states["attack"]);
                    playerAttackTimer = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && skillCoolTimes[0] >= skills[0].skillInfo.coolTime)
            {
                skillCoolTimes[0] = 0;
                skills[0].UseSkill();
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
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (playerWeapons[nowWeapon].stat.weaponIndex != 254)
                {
                    playerWeapons[nowWeapon].ResetGunSlot(transform.position);
                }
            }
            if (PlayerGetItem())
            {

            }

        }
        else if (stat.nowHP < 0 && stat.nowState != stat.states["die"])
        {
            fsmChanger(stat.states["die"]);
            Debug.Log("플레이어 죽음판정");
        }
        playerAttackTimer += Time.deltaTime;
        for (int i = 0; i < skillCoolTimes.Length; i++)
        {
            skillCoolTimes[i] += Time.deltaTime;
        }
        if (playerWeapons[nowWeapon] != null)
        {
            if (playerWeapons[nowWeapon].stat.weaponIndex != 254)
            {
                coolTimeIMG[0].fillAmount = ((stat.attackSpeed + playerWeapons[nowWeapon].stat.fireSpeed) / 1f) * playerAttackTimer;
            }
            else if ((playerWeapons[nowWeapon].stat.weaponIndex == 254 || playerWeapons[nowWeapon] != null))
            {
                coolTimeIMG[0].fillAmount = 0;
            }
        }
        coolTimeIMG[1].fillAmount = dodgeCooldown / stat.dodgeCooltime;

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log(Managers.DataManager.skillTable[0].skillName+ Managers.DataManager.skillTable[0].codeName);

            SkillTest.SkillSetting(Managers.DataManager.skillTable[0]);
        }        
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SkillTest.UseSkill();
        }
    }
    public void GetMousePos()
    {
        RaycastHit hit;
        float rotTemp = 0;
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, 8))
        {
            AttackPoint(hit.point, ref rotTemp, () =>
            {
                playerWeapons[nowWeapon].GunShot(firePos.position, rotTemp);
                /*if (Managers.DataManager.Datas.TryGetValue(playerWeapons[nowWeapon].stat.codeName+ "_Bullet", out UnityEngine.Object Result))
                {
                    Debug.Log(Result.ToString());
                    GameObject bulletTemp = Managers.Pool.Pop(Result.GameObject());
*//*                    bulletTemp.GetComponent<Bullet>().BulletSetting(in playerWeapons[nowWeapon].stat, playerWeapons[nowWeapon].GetTotalCollDamage(stat.attackDamage, stat.criticalDamage, stat.criticalChance));
                    if (playerWeapons[nowWeapon].stat.bulletType == bulletTypeEnum.explosion)
                    {
                        bulletTemp.GetComponent<Bullet>().BulletSetting(in playerWeapons[nowWeapon].stat, playerWeapons[nowWeapon].GetTotalCollDamage(stat.attackDamage, stat.criticalDamage, stat.criticalChance), playerWeapons[nowWeapon].GetTotalExDamage(stat.attackDamage, stat.criticalDamage, stat.criticalChance));
                    }*//*
                    bulletTemp.transform.position = firePos.position;
                    bulletTemp.transform.rotation = Quaternion.Euler(bulletTemp.transform.rotation.eulerAngles.x, rotTemp, bulletTemp.transform.rotation.eulerAngles.z);
                }*/

            });
        }
    }
    public byte whatIsEmptySlot(byte index)
    {
        for (byte i = 0; i < playerWeapons.Length; i++)
        {
            if (playerWeapons[i].stat.weaponIndex == index)
            {
                break;
            }
            else
            {
                if (playerWeapons[i].stat.isEmptySlot())
                {
                    return i;
                }
            }
        }
        return 255;
    }
    public bool PlayerGetItem()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            /*if (physicsPlus.IsChangedInArray(itemSencer, transform.position, 2, 8))
            {
                Debug.Log("변경있음");
                itemSencer = Physics.OverlapSphere(transform.position, 2, 256);
                if (physicsPlus.SearchTheComponent(itemSencer, out IItemBase target, "Item"))
                {
                    Debug.Log("빈칸찾음");
                    if (whatIsEmptySlot(target.itemIndex) != 255)
                    {
                        Debug.Log("총먹음");
                        target.UseItem<GunBase>(ref playerWeapons[whatIsEmptySlot(target.itemIndex)]);
                        Debug.Log(target);
                        return false;
                    }

                    Debug.Log(target);
                    return true;
                }
            }*/
            itemSencer = Physics.OverlapSphere(transform.position, 2, 256);
            if (physicsPlus.SearchTheComponent(itemSencer, out IItemBase target, "Item"))
            {

                Debug.Log("빈칸찾음");
                if (whatIsEmptySlot(target.itemIndex) != 255 && target.type == ItemTypeEnum.weapon)
                {
                    Debug.Log("총먹음");
                    target.UseItem<GunBase>(ref playerWeapons[whatIsEmptySlot(target.itemIndex)]);
                    Debug.Log(target);
                    return false;
                }
                else if (target.type == ItemTypeEnum.artifacts)
                {
                    foreach (var item in ownArtifacts)
                    {
                        if (item.data.itemnum == 254)
                        {
                            Managers.GameManager.ArtifactEquipOnly(target.itemIndex);
                            target.OBJPushOnly();
                            break;
                        }
                    }
                }
                else if (target.type == ItemTypeEnum.skill)
                {
                    SkillItem tempItem;
                    tempItem = target as SkillItem;
                    SkillBase tempSkill = FindSkillArray(target.itemIndex);
                    if (tempSkill.skillInfo.codeName != Managers.DataManager.skillTable[target.itemIndex].codeName)
                    {
                        tempSkill.playerTR = transform;
                        tempItem.UseItem<SkillBase>(ref tempSkill);
                    }
                    else
                    {
                        tempItem.UseItemToUpGrade(tempSkill);
                        Debug.Log("여기다가 중복스킬 처리");
                    }
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
    public void AttackPoint(Vector3 TargetPos, ref float quatTemp, Action Time = null)
    {
        TargetPos = new Vector3(TargetPos.x - transform.position.x, TargetPos.z - transform.position.z);
        quatTemp = ((MathF.Atan2(TargetPos.y, TargetPos.x) * Mathf.Rad2Deg) - 90) * -1;
        transform.rotation = Quaternion.Euler(0, quatTemp, 0);
        Time?.Invoke();
    }
    #region 스킬관련 함수
    public SkillBase[] skills = new SkillBase[6];
    public float[] skillCoolTimes = new float[6];
    private SkillBase FindSkillArray(int targetItemIndex)
    {
        SkillBase tempSkill = null;
        byte tempNum = 255;
        for (int i = skills.Length - 1; i >= 0; i--)
        {
            if (skills[i] == null)
            {
                skills[i] = new SkillBase();
            }
            if (skills[i].skillInfo.codeName == string.Empty)
            {
                tempNum = (byte)i;
                tempSkill = skills[i];
            }
            else if (skills[i].skillInfo.codeName == Managers.DataManager.skillTable[targetItemIndex].codeName)
            {
                tempNum = 255;
                tempSkill = skills[i];
                break;
            }
        }
        if (tempNum !=255)
        {
            skillCoolTimes[tempNum] = Managers.DataManager.skillTable[targetItemIndex].coolTime;
        }
        return tempSkill;
    }
    #endregion
    #region fsm 중계기를 만들어서 변수로 참조해와야함
    public void fsmChanger(BaseState BS,float numberalValue = 0,float duration = 0)
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
                StartCoroutine(dodgeTimer(numberalValue,duration));
            }
            else if (BS == stat.states["attack"])
            {
                StartCoroutine(animTimer());
            }
        }
    }

    public IEnumerator dodgeTimer(float distance,float duration)
    {
        transform.rotation = Quaternion.Euler(0, ((MathF.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg - 90) * -1), 0);
        if (playerDir.x == 0 && playerDir.y == 0)
        {
            rb.AddForce(Vector3.forward * (distance), ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(new Vector3(playerDir.x, 0, playerDir.y) * (distance), ForceMode.Impulse);
        }
        yield return null;
        Debug.Log(stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        stat.animator.speed = stat.animator.GetCurrentAnimatorStateInfo(0).length / duration;
        yield return new WaitUntil(() => stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
        rb.velocity = Vector3.zero;
        Debug.Log("애님끝남");
        nonControllable = false;
        fsmChanger(stat.states["idle"]);
        stat.animator.speed = 1;
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
        if (stat.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && stat.nowState == stat.states["attack"])
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
