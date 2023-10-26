using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IItemBase : MonoBehaviour
{
    public byte itemIndex;
    public ItemTypeEnum type;
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
        Managers.Pool.Push(this.gameObject);
    }
}

