using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IItemBase : MonoBehaviour
{
    public byte itemIndex;
    public ItemTypeEnum type;
    public abstract void UseItem<T>(ref T changeOriginValue);
    public void SetItemModel(Material mat, Mesh mesh, byte index) 
    {
        itemIndex = index;
        this.gameObject.GetComponent<MeshRenderer>().material = mat;
        this.gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    public void OBJPushOnly()
    {
        Managers.Pool.Push(this.gameObject);
    }
}

