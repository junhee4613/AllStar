using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SGProjectileManager
{    
    private List<SGProjectile> projectileList = new List<SGProjectile>(2000);       //총알 리스트 2000개 관리
    private HashSet<SGProjectile> projectileHashSet = new HashSet<SGProjectile>();  //각각 HashSet으로 접근해서 관리

    public int activeprojectileCount { get { return projectileList.Count; } }
 
    public void Updateprojectiles(float deltaTime)
    {
        for (int i = projectileList.Count - 1; i >= 0; i--)
        {
            SGProjectile projectile = projectileList[i];
            if (projectile == null)
            {
                projectileList.Remove(projectile);
                continue;
            }
            projectile.UpdateMove(deltaTime);
        }
    }

    public void Addprojectile(SGProjectile projectile)
    {
        if (projectileHashSet.Contains(projectile))
        {           
            return;
        }
        projectileList.Add(projectile);
        projectileHashSet.Add(projectile);
    }

    public void Removeprojectile(SGProjectile projectile, bool destroy)
    {
        if (projectileHashSet.Contains(projectile) == false)
        {            
            projectile.reserveReleaseOnShot = true;
            projectile.reserveReleaseOnShotIsDestroy = destroy;
            return;
        }
        projectileList.Remove(projectile);
        projectileHashSet.Remove(projectile);
    }
}
