using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullets : ClassManaging
{
    public ProjectileType bulletType;
    //세팅 
    public override void BasicValue()
    {
        Debug.Log(this.GetType());
        //확인 결과 클래스 이름을 가져올 수 있으니 데이터테이블에 총알 스텟을 딕셔너리화 하여 총알 클래스 이름을 기획서와 맞춰 데이터를 불러는식으로 진행
    }
}
public class Santan : Bullets
{
    
}
public class MachineGun : Bullets
{

}

