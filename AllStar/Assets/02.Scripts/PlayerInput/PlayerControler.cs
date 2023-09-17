using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerControler : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 playerAhead;
    public Vector2 playerDir;
    public Transform aa;
    public Ray mouseRay;
    public Status stat;
    public float playerAttackTimer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Managers.GameManager.BasicPlayerStats(()=> 
        {
            stat = Managers.GameManager.PlayerStat;
        });
        stat.states.SetGeneralFSMDefault(stat.animator,this.gameObject);
        stat.states.SetPlayerFSMDefault(stat.animator, this.gameObject);
    }

    private void Update()
    {
        rb.velocity = playerAhead;
        playerDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Horizontal")|| Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
            {
                playerDir = playerDir.normalized;
                playerAhead = new Vector3(Managers.GameManager.PlayerStat.moveSpeed * playerDir.x, 0, Managers.GameManager.PlayerStat.moveSpeed * playerDir.y);
            }
            else
            {
                playerAhead = new Vector3(Managers.GameManager.PlayerStat.moveSpeed * Input.GetAxisRaw("Horizontal"), 0, Managers.GameManager.PlayerStat.moveSpeed * Input.GetAxisRaw("Vertical"));
            }

        }
        else if(!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
        {
            playerAhead = new Vector3(0, rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.Mouse0)&&1f/stat.attackSpeed <playerAttackTimer)
        {
            //여기다가 이프문으로 딜레이를 넣을까?
            GetMousePos();
            playerAttackTimer = 0;
        }
        playerAttackTimer += Time.deltaTime;
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
                    
                    Instantiate<GameObject>(Result.GameObject(), transform.position, Quaternion.Euler(0, rotTemp, 0));
                }
                
            });
        }
    }
    public void AttackPoint(Vector3 TargetPos,ref float quatTemp,Action Time) 
    {
        TargetPos = new Vector3(TargetPos.x - transform.position.x,TargetPos.z - transform.position.z);
        quatTemp = ((MathF.Atan2(TargetPos.y, TargetPos.x)*Mathf.Rad2Deg)-90)*-1;
        Time?.Invoke();
        //localPos,WorldPos쪽 문제가 아닐지 한번
    }
}
