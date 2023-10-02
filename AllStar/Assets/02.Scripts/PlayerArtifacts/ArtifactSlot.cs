using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class ArtifactSlot
{
    [SerializeField]public ArtifactData data;
    public void StartArtifactsSetting()
    {
        data = new ArtifactData();
        data.itemnum = 254;
    }
}
