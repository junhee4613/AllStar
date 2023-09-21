using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemBase
{
    public void UseItem<T>(ref T changeOriginValue);
}

