using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IItemBase : MonoBehaviour
{
    public byte itemIndex;
    public ItemTypeEnum type;
    public Transform pressText;
    public abstract void UseItem<T>(ref T changeOriginValue);
    protected virtual void Start()
    {
        
    }
    public void SetItemModel(Material mat, Mesh mesh, byte index) 
    {
        itemIndex = index;
        this.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = mat;
        this.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    public void OBJPushOnly()
    {
        InteractionWindowClose();
        Managers.Pool.Push(this.gameObject);
    }
    public void InteractionWindowOpen()
    {
        if (pressText == null)
        {
            pressText = Managers.Pool.UIPop(Managers.DataManager.Datas["PressFKey"] as GameObject).transform;
            pressText.position = transform.position + Vector3.up;
        }
    }
    public void InteractionWindowClose()
    {

        if(pressText != null)
        {
            Managers.Pool.UIPush(pressText.gameObject);
            pressText = null;
        }

    }
}

