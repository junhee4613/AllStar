using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class DataTable
{

}
[System.Serializable]
public class WeaponData : DataTable
{
    public byte itemnum;
    public string name;
    public float bulletspeed;
    public float firespeed;
    public float removetimer;
    public float collisiondamage;
    public bulletTypeEnum bullettype;
    public float explosiondamage;
    public float explosionrange;
    public ShotType shottype;
    public byte fragmentCount;
    public float fireAngle;
    public string codename;
}
[System.Serializable]
public class ArtifactData : DataTable
{
    public byte itemnum;
    public string name;
    public statType statustype;
    public float value;
    public string flavortext;
    public string codename;
}
[System.Serializable]
public class ConsumableData : DataTable
{
    public byte itemnum;
    public string name;
    public statType statustype;
    public float value;
    public string codename;
}