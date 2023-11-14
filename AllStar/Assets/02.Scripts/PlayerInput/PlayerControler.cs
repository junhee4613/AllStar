using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using GeneralFSM;
using PlayerSkills.Skills;
using PlayerSkills.SkillProbs;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]private bool isLoaddineDone = false;
    [Header("��Ʈ�� �μ�")]
    public Rigidbody rb;
    public Vector2 playerDir;
    public Ray mouseRay;
    public GunBase[] playerWeapons;
    public int nowWeapon = 2;
    [SerializeField] private Transform firePos;
    [Header("������ ����")]
    public Collider[] itemSencer; //������ �ν�
    public physicsPlus.EnhancedPhysics<IItemBase> physicsPlus = new physicsPlus.EnhancedPhysics<IItemBase>();
    [Header("�÷��̾� ����")]
    public PlayerOnlyStatus stat;
    public ArtifactSlot[] ownArtifacts;
    public float rotationSpeed = 500;
    [Header("Ÿ�̸�")]
    public float playerAttackTimer = 0;
    public float dodgeCooldown = 0;
    public bool nonControllable;
    [Header("��Ÿ��UI")]
    [SerializeField] private UnityEngine.UI.Image[] coolTimeIMG;//0����,1ȸ��,2345 ��ų �� qerv

    [Header("��ų")]
    [SerializeField] private Image[] skillCoolIcons = new Image[5];
    public SkillBase[] skills;
    public float[] skillCoolTimes = new float[5];

    private void Start()
    {
        rb = null;
        rb = GetComponent<Rigidbody>();
        playerWeapons = Managers.GameManager.playerWeapons;

        ownArtifacts = Managers.GameManager.playerArtifacts;
        skills = Managers.GameManager.playerSkills;
        Managers.GameManager.playerCooltimes = skillCoolTimes;
        for (byte i = 0; i < playerWeapons.Length; i++)
        {
            if (playerWeapons[i].stat == null)
            {
                //playerWeapons[i] = new GunBase();
                playerWeapons[i].StartSetting();
            }
            else
            {
                Managers.UI.WeaponInventoryImageChanges(i, playerWeapons[i].stat.codeName);
            }
        }
        for (byte i = 0; i < ownArtifacts.Length; i++)
        {
            if (ownArtifacts[i] == null)
            {
                ownArtifacts[i] = new ArtifactSlot();
                ownArtifacts[i].ResetArtifact();
            }
            else if (ownArtifacts[i].artifactAmount !=0)
            {
                Managers.UI.ArtifactInventoryImageChanges(i, ownArtifacts[i].data.codename);
            }
        }

        stat = Managers.GameManager.PlayerStat;
        stat.states.SetGeneralFSMDefault(ref stat.animator, this.gameObject);
        stat.states.SetPlayerFSMDefault(stat.animator, this.gameObject);
        stat.nowState = stat.states["idle"];
        for (byte i = 0; i < playerWeapons.Length; i++)
        {
            playerWeapons[i].playerWeapon.Item1 = firePos.transform.GetChild(1).GetComponent<MeshFilter>();
            playerWeapons[i].playerWeapon.Item2 = firePos.transform.GetChild(1).GetComponent<MeshRenderer>();
        }
        isLoaddineDone = true;
        for (byte i = 0; i < skillCoolIcons.Length; i++)
        {
            skillCoolIcons[i] = Managers.UI.skillIconSet[i].IconIMG.transform.GetChild(0).GetComponent<Image>();
            if (skills[i].skillInfo != null && skills[i].skillInfo.skillLevel !=0)
            {
                Managers.UI.SetSkillIcons(i, skills[i].skillInfo.codeName);
                skills[i].ChangedSetting(transform);
            }
        }

    }

    private void Update()
    {
        if (!isLoaddineDone)
        {
            return; 
        }
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                inputSkill(0);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                inputSkill(1);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                inputSkill(2);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                inputSkill(3);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                inputSkill(4);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowWeapon = 0;
                playerWeapons[nowWeapon].ChangeWeaponModel();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowWeapon = 1;
                playerWeapons[nowWeapon].ChangeWeaponModel();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowWeapon = 2;
                playerWeapons[nowWeapon].ChangeWeaponModel();
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
            Debug.Log("�÷��̾� ��������");
        }
        playerAttackTimer += Time.deltaTime;
        for (int i = 0; i < skillCoolTimes.Length; i++)
        {
            skillCoolTimes[i] += Time.deltaTime;
            skillCoolIcons[i].fillAmount = 1 - skillCoolTimes[i] / skills[i].skillInfo.coolTime;
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

    public void SetWeaponModel()
    {

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
    IItemBase interactableItem;
    public bool PlayerGetItem()
    {
        /*if (physicsPlus.IsChangedInArray(itemSencer, transform.position, 2, 8))
{
    Debug.Log("��������");
    itemSencer = Physics.OverlapSphere(transform.position, 2, 256);
    if (physicsPlus.SearchTheComponent(itemSencer, out IItemBase target, "Item"))
    {
        Debug.Log("��ĭã��");
        if (whatIsEmptySlot(target.itemIndex) != 255)
        {
            Debug.Log("�Ѹ���");
            target.UseItem<GunBase>(ref playerWeapons[whatIsEmptySlot(target.itemIndex)]);
            Debug.Log(target);
            return false;
        }

        Debug.Log(target);
        return true;
    }
}*/
        itemSencer = Physics.OverlapSphere(transform.position, 1, 256);

        if (itemSencer.Length !=0)
        {
            if (physicsPlus.SearchTheComponent(itemSencer, out IItemBase target, "Item"))
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 0.4 && (interactableItem == null || interactableItem.gameObject.activeSelf))
                {
                    interactableItem = target;
                    target.InteractionWindowOpen();
                }
                else if (interactableItem != null && Vector3.Distance(transform.position, interactableItem.transform.position) >= 0.4)
                {
                    Debug.Log("���� �����ʿ�");
                    interactableItem.InteractionWindowClose();
                    interactableItem = null;
                }
                if (Input.GetKeyDown(KeyCode.F))
                {

                    Debug.Log("��ĭã��");
                    if (whatIsEmptySlot(target.itemIndex) != 255 && target.type == ItemTypeEnum.weapon)
                    {
                        byte tempIndex = whatIsEmptySlot(target.itemIndex);
                        Debug.Log("�Ѹ���");
                        target.UseItem<GunBase>(ref playerWeapons[tempIndex]);
                        if (tempIndex == nowWeapon)
                        {
                            //���⿡ �ѱ� �ش��ȣ�� �Լ� �����ʿ�
                            playerWeapons[tempIndex].ChangeWeaponModel();
                        }
                        Debug.Log(target);
                        return false;
                    }
                    else if (target.type == ItemTypeEnum.artifacts&&ownArtifacts[target.itemIndex].artifactAmount<9)
                    {
                        Managers.GameManager.ArtifactEquipOnly(target.itemIndex);
                        target.OBJPushOnly();
                    }
                    else if (target.type == ItemTypeEnum.skill)
                    {
                        SkillItem tempItem;
                        tempItem = target as SkillItem;
                        (SkillBase, byte) tempSkill = FindSkillArray(target.itemIndex);

                        if (tempSkill.Item2 !=255&&tempSkill.Item1.skillInfo.skillLevel <9)
                        {
                            if (tempSkill.Item1.skillInfo.codeName != Managers.DataManager.skillTable[target.itemIndex].codeName)
                            {
                                tempSkill.Item1.playerTR = transform;
                                tempItem.UseItem<SkillBase>(ref tempSkill.Item1);
                            }
                            else
                            {
                                tempItem.UseItemToUpGrade(tempSkill.Item1);
                                Debug.Log("����ٰ� �ߺ���ų ó��");
                            }
                            Managers.UI.SetSkillIcons(tempSkill.Item2, tempSkill.Item1.skillInfo.codeName);
                        }
                    }
                    else if (target.type == ItemTypeEnum.portal)
                    {
                        string aa = string.Empty;
                        target.UseItem<string>(ref aa);
                    }
                    Debug.Log(target);
                    return true;
                }
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
    #region ��ų���� �Լ�

    private (SkillBase, byte) FindSkillArray(int targetItemIndex)
    {
        SkillBase tempSkill = null;
        byte tempNum = 255;
        byte tempNum2 = 255;
        for (int i = skills.Length - 1; i >= 0; i--)
        {
            if (skills[i] == null)
            {
                skills[i] = new SkillBase();
            }
            if (skills[i].skillInfo.skillLevel == 0)
            {
                tempNum2 = (byte)i;
                tempNum = (byte)i;
                tempSkill = skills[i];
            }
            else if (skills[i].skillInfo.codeName == Managers.DataManager.skillTable[targetItemIndex].codeName)
            {
                tempNum = 255;
                tempNum2 = (byte)i;
                tempSkill = skills[i];
                break;
            }
        }
        if (tempNum != 255)
        {
            skillCoolTimes[tempNum] = Managers.DataManager.skillTable[targetItemIndex].coolTime;
        }
        return (tempSkill, tempNum2);
    }
    public void inputSkill(int tempInt)
    {
        if (skills[tempInt].DetailTypes != null&& skillCoolTimes[tempInt] >= skills[tempInt].skillInfo.coolTime)
        {
            skillCoolTimes[tempInt] = 0;
            skills[tempInt].UseSkill();
        }
    }
    #endregion
    #region fsm �߰�⸦ ���� ������ �����ؿ;���
    public void fsmChanger(BaseState BS, float numberalValue = 0, float duration = 0)
    {
        if (BS != stat.nowState)
        {
            stat.nowState.OnStateExit();
            stat.nowState = BS;
            stat.nowState.OnStateEnter();
            if (BS == stat.states["dodge"]/*����ٰ� ���� �߰��� ���� �ִϸ��̼�*/)
            {
                nonControllable = true;
                rb.velocity = Vector3.zero;
                StartCoroutine(dodgeTimer(numberalValue, duration));
            }
            else if (BS == stat.states["attack"])
            {
                StartCoroutine(animTimer());
            }
        }
    }

    public IEnumerator dodgeTimer(float distance, float duration)
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
        Debug.Log("�ִԳ���");
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
