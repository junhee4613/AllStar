using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    private Stack<Transform> uiStack = new Stack<Transform>();
    public ItemUI.ItemIconSet[] artifactSet = new ItemUI.ItemIconSet[20];
    public Image[] weaponSlotIMG = new Image[3];
    public Slider hpbar;
    public Slider loadBar;
    public void ArtifactInventoryImageChanges(byte itemIndex,string codeName)
    {
        Sprite a = Managers.DataManager.Load<Sprite>(codeName + "_ICON");

        artifactSet[itemIndex].IconIMG.sprite = a;
        artifactSet[itemIndex].AmountText.text = Managers.GameManager.playerArtifacts[itemIndex].artifactAmount != 0? Managers.GameManager.playerArtifacts[itemIndex].artifactAmount.ToString():"";

        Debug.Log(a);
    }
    public void WeaponInventoryImageChanges(byte arrayNum,string codeName)
    {
        Sprite a = Managers.DataManager.Load<Sprite>(codeName + "_ICON");
        weaponSlotIMG[arrayNum].sprite = a;
        Debug.Log(a);
    }
    public void OpenUI(Transform targetTR)
    {
        targetTR.gameObject.SetActive(true);
        uiStack.Push(targetTR);
    }
    public void CloseUI(Transform targetTR)
    {
        if (uiStack.Peek() != targetTR)
        {
            while (uiStack.Peek().gameObject.activeSelf == false)
            {
                uiStack.Pop();
                if (uiStack.Peek() == targetTR)
                {
                    uiStack.Pop();
                }
            }
        }
        else
        {
            uiStack.Pop();
        }
        targetTR.gameObject.SetActive(false);
    }
    public void popUI(Transform menuTR)
    {
        if (uiStack.Count>0)
        {
            if (uiStack.Peek().gameObject.activeSelf)
            {
                uiStack.Pop().gameObject.SetActive(false);
            }
            while (uiStack.Count > 0 && uiStack.Peek().gameObject.activeSelf == false)
            {
                uiStack.Pop();
            }
        }
        else
        {
            uiStack.Push(menuTR);
            menuTR.gameObject.SetActive(true);
        }
    }
}
