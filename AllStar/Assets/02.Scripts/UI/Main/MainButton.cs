using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    public GameObject optionWin;
    private void Start()
    {
        Managers.Sound.bgmList.Add(Resources.Load<AudioClip>("MainTitle"));
        Managers.GameManager.PlayerStat = null;
        Managers.Sound.BGM_Sound("MainTitle");
    }
    public void OnGameStartButton()
    {
        Managers.DataManager.Init(() =>
        {
            Managers.Sound.bgmList.Add(Managers.DataManager.Datas["Stage_001"] as AudioClip);
            SceneManager.LoadSceneAsync(1);
        });
    }
    public void OnBGMVolumeChange(float value)
    {
        Managers.Sound.BGMVolumeSave = value;
        Managers.Sound.bgm_Sound.volume = value;
    }
    public void OnSFXVolumeChange(float value)
    {
        Managers.Sound.SFXVolumeSave = value;
        Managers.Sound.sfx_Sound.volume = value;
    }
    public void OnOptionButton()
    {
        if (optionWin.activeSelf) { optionWin.SetActive(false); }
        else { optionWin.SetActive(true); }
    }
    public void OnGameExitButton()
    {
        Application.Quit();
    }
}
