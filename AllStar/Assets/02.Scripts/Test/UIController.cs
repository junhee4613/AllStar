using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIController : MonoBehaviour
{
    private Transform playerTR;
    [SerializeField] private RectTransform inventory;
    [SerializeField] private RectTransform escMenu;
    [Header("플레이버 텍스트")]
    [SerializeField] private RectTransform flavorTextPanel;
    [SerializeField] private TextMeshProUGUI flavorText;
    [Header("유물 아이콘이미지")]
    [SerializeField] private Image[] artifactInvenIMGs = new Image[20];
    public byte[] testNum = new byte[2];//0 아이템인덱스,1슬롯 인덱스 테스트코드
    public Vector2[] artifactIconPosition = new Vector2[20];
    private float artifactIconSize;
    public byte tempby = 255; //
    [Header("UI인터렉션 관련")]
    public DraggingArrayOBJ drag = new DraggingArrayOBJ();   //끌어당기는 이미지
    public DraggingState dragState = DraggingState.none;
    [Header("무기 정보창")]
    [SerializeField] private Image[] weaponInvenIMGs = new Image[3];
    public Vector2[] weaponIconPosition = new Vector2[3];
    private float weaponIconSize;
    [Header("플레이어 정보창")]
    [SerializeField] private RectTransform charactorInfo;
    public TextMeshProUGUI[] statusValues = new TextMeshProUGUI[5];
    //0체력,1공격력,2공속,3치확,4치뎀

    private void Awake()
    {
        Managers.GameManager.OnIconChange += infoStatUpdate;

        Vector2 tempVec;
        for (byte i = 0; i < inventory.GetChild(0).childCount; i++)
        {
            artifactInvenIMGs[i] = inventory.GetChild(0).GetChild(i).GetComponent<Image>();
            Debug.Log(artifactInvenIMGs[i].gameObject.name);
            tempVec = new Vector2(artifactInvenIMGs[i].rectTransform.position.x, artifactInvenIMGs[i].rectTransform.position.y);
            artifactIconPosition[i] = tempVec;
        }
        for (byte i = 0; i < inventory.GetChild(1).childCount; i++)
        {
            weaponInvenIMGs[i] = inventory.GetChild(1).GetChild(i).GetComponent<Image>();
            tempVec = new Vector2(weaponInvenIMGs[i].rectTransform.position.x, weaponInvenIMGs[i].rectTransform.position.y);
            weaponIconPosition[i] = tempVec;
        }
        artifactIconSize = artifactInvenIMGs[0].rectTransform.sizeDelta.x / 2;
        weaponIconSize = weaponInvenIMGs[0].rectTransform.sizeDelta.x / 2;
        Managers.UI.artifactSlotIMG = artifactInvenIMGs;
        Managers.UI.weaponSlotIMG = weaponInvenIMGs;
        GameObject.Find("PlayerController");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIOpenAndClose(inventory);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            UIOpenAndClose(charactorInfo);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.popUI(escMenu);
        }
        //플레이버 텍스트 출력은 마우스 위치를 받고 유물 순서를 대입받아 hashSet그리드로 하면 될듯? 
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
                    flavorTextPanel.anchoredPosition = Input.mousePosition;
                    Debug.Log(tempby);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            tempby = GetIMGPosition(weaponIconPosition, weaponIconSize);
            if (tempby == 255)
            {
                tempby = GetIMGPosition(artifactIconPosition, artifactIconSize);
                if (tempby <= (byte)artifactIconPosition.Length)
                {
                    dragState = DraggingState.artifact;
                    drag.array = tempby;
                    drag.originPos = artifactInvenIMGs[tempby].rectTransform.position;
                    drag.targetTR = artifactInvenIMGs[tempby].rectTransform;
                }
            }
            else
            {
                if (tempby <= (byte)weaponIconPosition.Length)
                {
                    dragState = DraggingState.weapon;
                    drag.array = tempby;
                    drag.originPos = weaponInvenIMGs[tempby].rectTransform.position;
                    drag.targetTR = weaponInvenIMGs[tempby].rectTransform;
                }
            }
        }
        else if (Input.GetKey(KeyCode.Mouse0) && drag.targetTR != null)
        {
            drag.targetTR.position = Input.mousePosition;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            byte clickEndPoint;
            if (EventSystem.current.IsPointerOverGameObject())
                //마우스를 계속 UI가 따라오기에 위 함수로 구분 불가 UI위치 외에 다른부분 좌표를 구해야할듯
            {
                if (dragState == DraggingState.artifact)
                {
                    clickEndPoint = GetIMGPosition(artifactIconPosition, artifactIconSize);
                    if (clickEndPoint != 255&& clickEndPoint != tempby)
                    {
                        Managers.GameManager.BothChageArtifact(Managers.GameManager.playerArtifacts[drag.array].data.itemnum, drag.array,
                                        Managers.GameManager.playerArtifacts[clickEndPoint].data.itemnum, clickEndPoint);
                    }
                }
                else if (dragState == DraggingState.weapon)
                {
                    clickEndPoint = GetIMGPosition(weaponIconPosition, weaponIconSize);
                    if (clickEndPoint != tempby && clickEndPoint != 255)
                    {
                        Managers.GameManager.ChangeWeaponArray(tempby, clickEndPoint);
                    }
                }
            }
            else if(!EventSystem.current.IsPointerOverGameObject())
            //마우스를 계속 UI가 따라오기에 위 함수로 구분 불가 UI위치 외에 다른부분 좌표를 구해야할듯
            {
                if (dragState == DraggingState.artifact)
                {

                }
                else if (dragState == DraggingState.weapon)
                {
                    if (Managers.GameManager.playerWeapons[tempby].stat.weaponIndex !=254)
                    {
                        Managers.GameManager.playerWeapons[tempby].ResetGunSlot(playerTR.position);
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
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Managers.GameManager.ArtifactEquipOnly(testNum[0]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Managers.GameManager.ArtifactRemoveOnly(testNum[1]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Managers.GameManager.ChageArtifact(testNum[0], testNum[1]);
        }
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
}
public class DraggingArrayOBJ
{
    public RectTransform targetTR;
    public byte array;
    public Vector2 originPos;
}
