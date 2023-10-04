using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Stack<Transform> uiStack = new Stack<Transform>();
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
