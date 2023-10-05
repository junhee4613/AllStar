using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class ArtifactSlot
{
    [SerializeField]public ArtifactData data;
    public void ResetArtifact(bool isStart = true)
    {
        if (isStart)
        {
            data = new ArtifactData();
        }
        data.itemnum = 254;
        data.name = default;
        data.statustype = default;
        data.value = default;
        data.flavortext = default;
        data.codename = default;
    }
}
