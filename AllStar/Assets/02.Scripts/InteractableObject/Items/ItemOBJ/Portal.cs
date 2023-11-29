using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : IItemBase
{
    public override void UseItem<T>(ref T changeOriginValue)
    {
        Managers.Pool.Clear();
        Managers.UI.ResetUI();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
        //Managers.Sound.bgmList.Add(Managers.DataManager.Datas["Stage_00"+ SceneManager.GetActiveScene().buildIndex+1] as AudioClip);
        Debug.Log("æ¿¿¸»Ø");
    }
}
