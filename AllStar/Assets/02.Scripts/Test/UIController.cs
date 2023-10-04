using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform inventory;
    [SerializeField] private RectTransform charactorInfo;
    [SerializeField] private RectTransform ESCMenu;
    [SerializeField] private Text flavorText;
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
            Managers.UI.popUI(ESCMenu);
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //�÷��̹� �ؽ�Ʈ ����� ���콺 ��ġ�� �ް� ���� ������ ���Թ޾� hashSet�׸���� �ϸ� �ɵ�? 
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