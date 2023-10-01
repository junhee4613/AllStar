using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IItemBase : MonoBehaviour
{
    [SerializeField]public byte itemIndex;
    public abstract void UseItem<T>(ref T changeOriginValue);
    public void SetItemModel(Material mat, Mesh mesh, byte index) 
    {
        itemIndex = index;
        this.gameObject.GetComponent<MeshRenderer>().material = mat;
        this.gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
}

