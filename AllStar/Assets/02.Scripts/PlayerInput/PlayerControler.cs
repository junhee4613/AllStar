using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerControler : MonoBehaviour
{
    public Rigidbody rb;
    public Vector2 playerDir;
    public Ray mouseRay;
    public Status stat;
    
    public float playerAttackTimer = 0;
    public float dodgeCooldown = 0 ;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stat.animator = GetComponent<Animator>();
        Managers.GameManager.BasicPlayerStats(()=> 
        {
            stat = Managers.GameManager.PlayerStat;
            stat.states.SetGeneralFSMDefault(ref stat.animator, this.gameObject);
            stat.states.SetPlayerFSMDefault(stat.animator, this.gameObject);
        });
    }

    private void Update()
    {
        rb.velocity = new Vector3(stat.moveSpeed * playerDir.x, 0,stat.moveSpeed * playerDir.y);
        if (stat.states.ContainsKey("dodge")&&stat.nowState != stat.states["dodge"])
        {
            if (Input.GetButton("Horizontal")&& Input.GetButton("Vertical"))
            {
                playerDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; ;
            }
            else
            {
                playerDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
            if (Input.GetKey(KeyCode.Mouse0)&&1f/stat.attackSpeed <playerAttackTimer)
            {
                GetMousePos();
                playerAttackTimer = 0;
            }
        }
        playerAttackTimer += Time.deltaTime;
        dodgeCooldown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space)&&dodgeCooldown >= stat.skillCoolTime)
        {
            dodgeCooldown = 0;
            stat.nowState = stat.states["dodge"];
        }
    }
    public void GetMousePos()
    {
        RaycastHit hit;
        float rotTemp = 0;
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay,out hit,Mathf.Infinity,8))
        {
            AttackPoint(hit.point, ref rotTemp, () => 
            {
                if (Managers.DataManager.Datas.TryGetValue("Bullet_Test",out UnityEngine.Object Result))
                {
                    GameObject bulletTemp = Managers.Pool.Pop(Result.GameObject());
                    bulletTemp.transform.position = transform.position;
                    bulletTemp.transform.rotation = Quaternion.Euler(0, rotTemp, 0);
                }
                
            });
        }
    }
    public void AttackPoint(Vector3 TargetPos,ref float quatTemp,Action Time) 
    {
        TargetPos = new Vector3(TargetPos.x - transform.position.x,TargetPos.z - transform.position.z);
        quatTemp = ((MathF.Atan2(TargetPos.y, TargetPos.x)*Mathf.Rad2Deg)-90)*-1;
        Time?.Invoke();
    }
}
