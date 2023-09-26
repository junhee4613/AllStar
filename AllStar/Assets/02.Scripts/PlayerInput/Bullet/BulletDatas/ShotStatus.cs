using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotStatus
{
    
}
public class MultiShot : ShotStatus
{
    public float fragmentCount;
    public float fireAngle;
}
public class SingleShot : ShotStatus
{

}
