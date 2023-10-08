using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform inventory;
    [SerializeField] private RectTransform charactorInfo;
    [SerializeField] private RectTransform escMenu;
    [Header("플레이버 텍스트")]
    [SerializeField] private RectTransform flavorTextPanel;
    [SerializeField] private TextMeshProUGUI flavorText;
    [Header("유물 아이콘이미지")]
    [SerializeField] private Image[] artifactInvenIMGs = new Image[20];
    public byte[] testNum = new byte[2];//0 아이템인덱스,1슬롯 인덱스 테스트코드
    public Vector2[] artifactIconPosition = new Vector2[20];
    private float IconSize;
    
    private void Start()
    {
        for (byte i = 0; i < inventory.GetChild(0).childCount; i++)
        {
            artifactInvenIMGs[i] = inventory.GetChild(0).GetChild(i).GetComponent<Image>();
            Debug.Log(artifactInvenIMGs[i].gameObject.name);
            Vector2 tempVec = new Vector2(artifactInvenIMGs[i].rectTransform.position.x, artifactInvenIMGs[i].rectTransform.position.y);
            artifactIconPosition[i] = tempVec;
        }
        IconSize = artifactInvenIMGs[0].rectTransform.sizeDelta.x / 2;
        Managers.UI.artifactSlotIMG = artifactInvenIMGs;
    }
    private byte GetMouseDistance()
    {
        float distance;
        byte i = 0;
        for (i = 0; i <= artifactIconPosition.Length+1; i++)
        {
            if (artifactIconPosition.Length > i)
            {
                distance = Vector2.Distance(artifactIconPosition[i], Input.mousePosition);
                if (distance < IconSize)
                {
                    break;
                }
            }
            else if (artifactIconPosition.Length+1 == i)
            {
                i = 255;
                break;
            }
        }
        return i;
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //플레이버 텍스트 출력은 마우스 위치를 받고 유물 순서를 대입받아 hashSet그리드로 하면 될듯? 
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                byte tempby = GetMouseDistance();
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
            Managers.GameManager.ChageArtifact(testNum[0],testNum[1]);
        }
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