using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactItem : IItemBase
{
    
    public override void UseItem<T>(ref T t)
    {
        Managers.Pool.Push(this.gameObject);
    }
}
