using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]private Transform playerTR;

    [SerializeField] private RectTransform Inventory;
    [SerializeField] private RectTransform escMenu;
    [Header("�÷��̹� �ؽ�Ʈ")]
    [SerializeField] private RectTransform flavorTextPanel;
    [SerializeField] private TextMeshProUGUI flavorText;
    [Header("���� �������̹���")]
    [SerializeField] private ItemUI.ItemIconSet[] artifactInvenSet = new ItemUI.ItemIconSet[6];
    public byte[] testNum = new byte[2];//0 �������ε���,1���� �ε��� �׽�Ʈ�ڵ�
    public Vector2[] artifactIconPosition = new Vector2[6];
    private float artifactIconSize;
    public byte tempby = 255; //
    [Header("UI���ͷ��� ����")]
    [SerializeField]GameObject optionPanel;
    public DraggingArrayOBJ drag = new DraggingArrayOBJ();   //������� �̹���
    public DraggingState dragState = DraggingState.none;

    [Header("���� ����â")]
    [SerializeField] private Image[] weaponInvenIMGs = new Image[3];
    public Vector2[] weaponIconPosition = new Vector2[3];
    private float weaponIconSize;
    [Header("�÷��̾� ����â")]
    [SerializeField] private RectTransform charactorInfo;
    [SerializeField]private Slider PlrHPBar;
    public TextMeshProUGUI[] statusValues = new TextMeshProUGUI[5];
    [Header("��ų����")]
    [SerializeField] private ItemUI.ItemIconSet[] skillSlotSet = new ItemUI.ItemIconSet[5];
    [SerializeField] private RectTransform skillSlots;
    public Vector2[] skillIconPosition = new Vector2[5];
    private float skillIconSIze;
    [SerializeField]private Animator SkillBarAnim;
    //0ü��,1���ݷ�,2����,3ġȮ,4ġ��
    private void Awake()
    {
        Managers.UI.hpbar = PlrHPBar;
        Managers.GameManager.OnIconChange += infoStatUpdate;
        Vector2 tempVec;
        for (byte i = 0; i < skillSlots.GetChild(0).childCount; i++)
        {
            Debug.Log("���� �ٽ�¥�ߵ�");
            artifactInvenSet[i].IconIMG = skillSlots.GetChild(0).GetChild(i).GetChild(0).GetComponent<Image>();
            Debug.Log(artifactInvenSet[i].IconIMG.gameObject.name);
            tempVec = new Vector2(artifactInvenSet[i].IconIMG.rectTransform.position.x, artifactInvenSet[i].IconIMG.rectTransform.position.y);
            artifactInvenSet[i].AmountText = artifactInvenSet[i].IconIMG.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            artifactIconPosition[i] = tempVec;
        }
        for (byte i = 0; i < skillSlots.GetChild(1).childCount; i++)
        {
            skillSlotSet[i].IconIMG = skillSlots.GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>();
            skillSlotSet[i].AmountText = skillSlots.GetChild(1).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            tempVec = new Vector2(skillSlotSet[i].IconIMG.rectTransform.position.x, skillSlotSet[i].IconIMG.rectTransform.position.y);
            skillIconPosition[i] = tempVec;
        }
        for (byte i = 0; i < Inventory.GetChild(0).childCount; i++)
        {
            weaponInvenIMGs[i] = Inventory.GetChild(0).GetChild(i).GetComponent<Image>();
            tempVec = new Vector2(weaponInvenIMGs[i].rectTransform.position.x, weaponInvenIMGs[i].rectTransform.position.y);
            weaponIconPosition[i] = tempVec;
        }
        artifactIconSize = artifactInvenSet[0].IconIMG.rectTransform.sizeDelta.x / 2;
        weaponIconSize = weaponInvenIMGs[0].rectTransform.sizeDelta.x / 2;
        skillIconSIze = skillSlotSet[0].IconIMG.rectTransform.sizeDelta.x / 2;
        Managers.UI.artifactSet = artifactInvenSet;
        Managers.UI.skillIconSet = skillSlotSet;
        Managers.UI.weaponSlotIMG = weaponInvenIMGs;
        playerTR = GameObject.Find("PlayerController").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIOpenAndClose(Inventory);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            UIOpenAndClose(charactorInfo);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.popUI(escMenu);
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            SkillBarAnim.SetTrigger("uiTrigger");
        }
        //�÷��̹� �ؽ�Ʈ ����� ���콺 ��ġ�� �ް� ���� ������ ���Թ޾� hashSet�׸���� �ϸ� �ɵ�? 
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            tempby = GetIMGPosition(artifactIconPosition, artifactIconSize);
            if (tempby <= (byte)artifactIconPosition.Length)
            {
                string tempFlavor = Managers.GameManager.playerArtifacts[tempby].data.flavortext;
                if (tempFlavor != default)
                {
                    UIOpenAndClose(flavorTextPanel);
                    flavorText.text = Managers.GameManager.playerArtifacts[tempby].data.flavortext;
                    flavorTextPanel.localPosition = Input.mousePosition-Inventory.transform.position;
                    Debug.Log(tempby);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && Inventory.gameObject.activeSelf)
        {

            /*            if (tempby == 255)
                        {
                            tempby = GetIMGPosition(artifactIconPosition, artifactIconSize);
                            if (tempby <= (byte)artifactIconPosition.Length)
                            {
                                dragState = DraggingState.artifact;
                                drag.array = tempby;
                                drag.originPos = artifactInvenSet[tempby].IconIMG.rectTransform.position;
                                drag.targetTR = artifactInvenSet[tempby].IconIMG.rectTransform;
                            }
                        }*/
            if (tempby <= (byte)weaponIconPosition.Length && IsMouseOnWhichTarget(weaponIconPosition, weaponIconSize))
            {
                tempby = GetIMGPosition(weaponIconPosition, weaponIconSize);
                dragState = DraggingState.weapon;
                drag.array = tempby;
                drag.originPos = weaponInvenIMGs[tempby].rectTransform.position;
                drag.targetTR = weaponInvenIMGs[tempby].rectTransform;
            }
            else if (!SkillBarAnim.GetCurrentAnimatorStateInfo(0).IsName("ActiveSkillBarFalse")&&IsMouseOnWhichTarget(skillIconPosition,skillIconSIze))
            {
                Debug.Log("��ų ����");
                tempby = GetIMGPosition(skillIconPosition, skillIconSIze);
                dragState = DraggingState.skills;
                drag.array = tempby;
                drag.originPos = skillSlotSet[tempby].IconIMG.rectTransform.position;
                drag.targetTR = skillSlotSet[tempby].IconIMG.rectTransform;
            }
        }
        else if (Input.GetKey(KeyCode.Mouse0) && drag.targetTR != null)
        {
            drag.targetTR.position = Input.mousePosition;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)&&drag.targetTR != null)
        {
            byte clickEndPoint;
            if (GetIMGPosition(weaponIconPosition, weaponIconSize) !=255/*|| GetIMGPosition(artifactIconPosition, artifactIconSize) != 255*/)
            {
/*                if (dragState == DraggingState.artifact)
                {
*//*                    clickEndPoint = GetIMGPosition(artifactIconPosition, artifactIconSize);
                    if (clickEndPoint != 255&& clickEndPoint != tempby)
                    {
                        Managers.GameManager.BothChageArtifact(Managers.GameManager.playerArtifacts[drag.array].data.itemnum, drag.array,
                                        Managers.GameManager.playerArtifacts[clickEndPoint].data.itemnum, clickEndPoint);
                    }*//*
                }*/
                if (dragState == DraggingState.weapon)
                {
                    clickEndPoint = GetIMGPosition(weaponIconPosition, weaponIconSize);
                    if (clickEndPoint != tempby && clickEndPoint != 255)
                    {
                        Managers.GameManager.ChangeWeaponArray(tempby, clickEndPoint);
                    }
                }
            }
            else if(GetIMGPosition(weaponIconPosition, weaponIconSize) !=255)
            {
/*                if (dragState == DraggingState.artifact)
                {
                    Managers.GameManager.ArtifactWaste(tempby, playerTR.position);
                }*/
                if (dragState == DraggingState.weapon)
                {
                    if (Managers.GameManager.playerWeapons[tempby].stat.weaponIndex !=254)
                    {
                        Managers.GameManager.playerWeapons[tempby].ResetGunSlot(playerTR.position);

                    }
                }
            }
            else if (IsMouseOnWhichTarget(skillIconPosition,skillIconSIze))
            {
                if (dragState == DraggingState.skills)
                {
                    clickEndPoint = GetIMGPosition(skillIconPosition, skillIconSIze);
                    Debug.Log("��ų �̵�");
                    if (clickEndPoint != tempby && clickEndPoint != 255)
                    {
                        Debug.Log("��ų �̵�");
                        Managers.GameManager.ChangeSkillArray(tempby, clickEndPoint);
                    }
                }
            }
            dragState = DraggingState.none;
            if (drag.targetTR != null)
            {
                drag.targetTR.position = drag.originPos;
                drag.targetTR = null;
                drag.array = 255;
                drag.originPos = default;
            }
        }

    }
    public void OnClickExitBTN()
    {
        Application.Quit();
    }
    public void OnClickToMainMenu()
    {
        Debug.Log("���ξ� �߰� �ʿ�");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    private void infoStatUpdate()
    {
        statusValues[0].text = Managers.GameManager.PlayerStat.maxHP.ToString();
        statusValues[1].text = Managers.GameManager.PlayerStat.attackDamage.ToString();
        statusValues[2].text = Managers.GameManager.PlayerStat.attackSpeed.ToString();
        statusValues[3].text = Managers.GameManager.PlayerStat.criticalChance.ToString();
        statusValues[4].text = Managers.GameManager.PlayerStat.criticalDamage.ToString();
    }
    private byte GetIMGPosition(Vector2[] targetArray, float iconSize)
    {
        float distance;
        byte i = 0;
        for (i = 0; i <= targetArray.Length + 1; i++)
        {
            if (targetArray.Length > i)
            {
                distance = Vector2.Distance(targetArray[i], Input.mousePosition);
                if (distance < iconSize)
                {
                    break;
                }
            }
            else if (targetArray.Length + 1 == i)
            {
                i = 255;
                break;
            }
        }
        return i;
    }
    private bool IsMouseOnWhichTarget(Vector2[] targetArray, float iconSize)
    {
        float distance;
        byte i = 0;
        for (i = 0; i <= targetArray.Length + 1; i++)
        {
            if (targetArray.Length > i)
            {
                distance = Vector2.Distance(targetArray[i], Input.mousePosition);
                if (distance < iconSize)
                {
                    return true;
                }
            }
            else if (targetArray.Length + 1 == i)
            {
                i = 255;
                return false;
            }
        }
        return false;
    }
    private void UIOpenAndClose(Transform target)
    {
        if (target.gameObject.activeSelf)
        {
            Managers.UI.CloseUI(target);
        }
        else
        {
            Managers.UI.OpenUI(target);
        }
    }
    public void OnBGMVolumeChange(float value)
    {
        Managers.Sound.BGMVolumeSave = value;
        Managers.Sound.bgm_Sound.volume = value;
    }
    public void OnSFXVolumeChange(float value)
    {
        Managers.Sound.SFXVolumeSave = value;
        Managers.Sound.sfx_Sound.volume = value;
    }
    public void OnOptionButton()
    {
        if (optionPanel.activeSelf) { Managers.UI.CloseUI(optionPanel.transform); }
        else { Managers.UI.OpenUI(optionPanel.transform); }
    }
}
public class DraggingArrayOBJ
{
    public RectTransform targetTR;
    public byte array;
    public Vector2 originPos;
}
