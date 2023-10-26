using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour,IPotion
{
    public bool groundCheck = false;
    public float time;
    public int dir = 1;
    Rigidbody rb;
    public float hp_heal;
    public Collider[] test;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!groundCheck)
        {
            transform.Rotate(0, 0, 360 * Time.deltaTime);
            test = Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity, 512);
            if(test.Length != 0)
            {
                groundCheck = true;
            }
        }
        else if (groundCheck)
        {
            rb.isKinematic = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void Use()
    {
        Managers.GameManager.PlayerStat.nowHP += hp_heal;
    }
    //http://theeye.pe.kr/archives/2764 ÀÌ°Å Unity RayCast, BoxCast, SphereCast 
}
