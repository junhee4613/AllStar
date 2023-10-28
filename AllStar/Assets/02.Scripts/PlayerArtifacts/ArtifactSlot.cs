using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class ArtifactSlot
{
    [SerializeField] public ArtifactData data;
    public int artifactAmount = 0;
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
    public void SetArtifact(in ArtifactData tempArti)
    {
        data.itemnum = tempArti.itemnum;
        data.name = tempArti.name;
        data.statustype = tempArti.statustype;
        data.value = tempArti.value;
        data.flavortext = tempArti.flavortext;
        data.codename = tempArti.codename;
    }
}
